using UnityEngine;
using TMPro;

public class GunBehavior : MonoBehaviour
{
    [Header("Ammo UI")]
    public TextMeshProUGUI ammoUI;

    [Header("Gun Shot Properties")]
    [SerializeField] float damage = 1f;
    [SerializeField] float range = 100f;
    [SerializeField] float impactForce = 60f;
    [Tooltip("Number of bullet fired per second")] [SerializeField] float fireRate = 3f;
    [SerializeField] int maxAmmo = 7;

    [Header("FPS Cam")]
    [SerializeField] Camera fpsCam;

    [Header("Layer Masks")]
    [SerializeField] LayerMask enemiesMask;
    [SerializeField] LayerMask environmentMask;

    [Header("Prefabs")]
    [SerializeField] GameObject bulletImpactEffectPrefab;
    [SerializeField] GameObject bloodEffectPrefab;

    [Header("References")]
    [SerializeField] SimpleShoot simpleShoot;

    float nextTimeToFire = 0f;
    bool gunTriggerLifted = true;
    bool isReloading = false;
    int currentAmmo;

    private void Start()
    {
        currentAmmo = maxAmmo;
    }

    void Update()
    {
        ammoUI.SetText(currentAmmo.ToString() + " / ∞"); //menampilkan current ammo pada layar

        // Kita gunakan GetButton() agar penerimaan input lebih responsif dan
        // gunTriggerLifted agar pemain tidak bisa menembak hanya dengan menahan tombol mouse.
        if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire 
            && gunTriggerLifted && !isReloading)
        {
            gunTriggerLifted = false;
            nextTimeToFire = Time.time + 1f / fireRate;

            if (currentAmmo > 0)
            {
                simpleShoot.StartShootAnim();
                ShootBehavior();
            }
            else if (currentAmmo <= 0)
            {
                isReloading = true;
                simpleShoot.StartReloadAnim();
            }
        }

        if (Input.GetKeyDown(KeyCode.R) && !isReloading 
            && currentAmmo != maxAmmo)
        {
            isReloading = true;
            simpleShoot.StartReloadAnim();
        }

        if (Input.GetButtonUp("Fire1"))
        {
            gunTriggerLifted = true;
        }
    }

    public void FinishedReloadingBehavior()
    {
        currentAmmo = maxAmmo;
        isReloading = false;
    }

    // NOTE: 
    // - Jangan pakai nama Shoot() karena itu sudah dipakai untuk animasi gun pada SimpleShoot.cs.
    // - Jika pakai Shoot(), maka fungsi ini akan dipanggil dua kali.
    void ShootBehavior()
    {
        RaycastHit hitInfoEnvironment;
        RaycastHit hitInfoEnemy;

        --currentAmmo;

        // Cek apakah kita menabrak objek pada layer "Ground" yang memiliki collider.
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward
            , out hitInfoEnvironment, range, environmentMask))
        {
            //Debug.Log("At Ground Mask: " + hitInfo.transform.name);

            IDamageable target = hitInfoEnvironment.transform.GetComponent<IDamageable>();
            if (target != null)
            {
                target.TakeDamage(damage);
            }

            if (hitInfoEnvironment.rigidbody != null)
            {
                hitInfoEnvironment.rigidbody.AddForce(-hitInfoEnvironment.normal * impactForce);
            }

            GameObject impactGO = Instantiate(bulletImpactEffectPrefab, hitInfoEnvironment.point
                , Quaternion.LookRotation(hitInfoEnvironment.normal));
            Destroy(impactGO, 2f);
        }

        // Cek apakah kita menabrak objek pada layer "Enemy" yang memiliki collider.
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward
            , out hitInfoEnemy, range, enemiesMask))
        {
            //Debug.Log(hitInfoEnemy.transform.name);

            GameObject impactGO = Instantiate(bloodEffectPrefab, hitInfoEnemy.point
                , Quaternion.LookRotation(hitInfoEnemy.normal));

            impactGO.transform.parent = hitInfoEnemy.transform;
            Destroy(impactGO, 2f);

            IDamageable damagable=  hitInfoEnemy.transform.root
                .GetComponent<ZombieBehavior>();

            if (damagable != null)
            {
                damagable.TakeDamage(damage);
            }
        }
    }
}
