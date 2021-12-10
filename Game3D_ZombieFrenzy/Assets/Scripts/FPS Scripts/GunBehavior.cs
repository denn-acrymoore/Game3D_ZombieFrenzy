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

    private bool gunTriggerLifted = true;
    private bool isReloading = false;
    private int currentAmmo;
    private bool gunReadyToFire = true;

    private void Start()
    {
        currentAmmo = maxAmmo;
    }

    void Update()
    {
        UpdateHUDText();

        if (GameManagerScript.isPlayerAlive && !GameManagerScript.isPlayerWin)
        {
            GetShootAndReloadInput();
        }
    }

    void UpdateHUDText()
    {
        ammoUI.SetText(currentAmmo.ToString() + " / ∞"); //menampilkan current ammo pada layar
    }

    void GetShootAndReloadInput()
    {
        // Kita gunakan GetButton() agar penerimaan input lebih responsif dan
        // gunTriggerLifted agar pemain tidak bisa menembak hanya dengan menahan tombol mouse.
        if (Input.GetButton("Fire1") && gunReadyToFire
            && gunTriggerLifted && !isReloading)
        {
            gunTriggerLifted = false;

            if (currentAmmo > 0)
            {
                gunReadyToFire = false;
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

    public void GunShootAnimFinished()
    {
        gunReadyToFire = true;
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
                target.TakeDamage(damage, hitInfoEnvironment.collider);
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
            Debug.Log(hitInfoEnemy.collider.transform.name);

            GameObject impactGO = Instantiate(bloodEffectPrefab, hitInfoEnemy.point
                , Quaternion.LookRotation(hitInfoEnemy.normal));

            impactGO.transform.parent = hitInfoEnemy.transform;
            Destroy(impactGO, 2f);

            IDamageable damageable=  hitInfoEnemy.transform.root
                .GetComponent<ZombieBehavior>();

            if (damageable != null)
            {
                damageable.TakeDamage(damage, hitInfoEnemy.collider);
            }
        }
    }
}
