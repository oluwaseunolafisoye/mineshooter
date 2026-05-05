using Cinemachine;
using StarterAssets;
using TMPro;
using UnityEngine;

public class ActiveWeapon : MonoBehaviour
{
    [SerializeField] WeaponSO starterWeapon;
    [SerializeField] CinemachineVirtualCamera playerCamera;
    [SerializeField] GameObject zoomOverlay;
    [SerializeField] TMP_Text ammoText;

    WeaponSO currentWeaponSO;

    Animator animator;
    StarterAssetsInputs starterAssetsInputs;
    FirstPersonController firstPersonController;
    Weapon activeWeapon;

    const string SHOOT_STRING = "Shoot";

    float timeSinceLastShot = 0f;
    float defaultFOV;
    float defaultRotationSpeed;
    int currentAmmo;

    void Awake()
    {
        starterAssetsInputs = GetComponentInParent<StarterAssetsInputs>();
        firstPersonController = GetComponentInParent<FirstPersonController>();
        animator = GetComponent<Animator>();
        defaultFOV = playerCamera.m_Lens.FieldOfView;
        defaultRotationSpeed = firstPersonController.RotationSpeed;
    }

    void Start()
    {
        SwitchWeapon(starterWeapon);
    }

    void Update()
    {
        PlayerShoot();
        PlayerZoom();
    }

    public void AdjustAmmo(int ammoAmount)
    {
        currentAmmo = Mathf.Max(0, currentAmmo + ammoAmount);

        if (currentAmmo > currentWeaponSO.MagazineSize)
        {
            currentAmmo = currentWeaponSO.MagazineSize;
        }

        ammoText.text = currentAmmo.ToString("D2");
    }

    public void SwitchWeapon(WeaponSO weaponSO)
    {
        if (activeWeapon)
        {
            Destroy(activeWeapon.gameObject);
        }

        Weapon newWeapon = Instantiate(weaponSO.WeaponPrefab, transform).GetComponent<Weapon>();
        activeWeapon = newWeapon;
        currentWeaponSO = weaponSO;
        timeSinceLastShot = 0f;
        currentAmmo = currentWeaponSO.MagazineSize;
        ammoText.text = currentAmmo.ToString("D2");
    }

    void PlayerShoot()
    {
        timeSinceLastShot += Time.deltaTime;

        if (!starterAssetsInputs.shoot)
            return;

        if (currentAmmo <= 0)
            return;

        if (timeSinceLastShot < currentWeaponSO.FireRate)
            return;

        activeWeapon.PlayerShoot(currentWeaponSO);
        animator.Play(SHOOT_STRING, 0, 0f);
        timeSinceLastShot = 0f;
        AdjustAmmo(-1);

        if (!currentWeaponSO.IsAutomatic)
        {
            starterAssetsInputs.ShootInput(false);
        }
    }

    void PlayerZoom()
    {
        if (!currentWeaponSO.CanZoom) return;

        if (starterAssetsInputs.zoom)
        {
            playerCamera.m_Lens.FieldOfView = currentWeaponSO.ZoomFOV;
            zoomOverlay.SetActive(true);
            firstPersonController.ChangeRotationSpeed(currentWeaponSO.ZoomRotationSpeed);
        }
        else
        {
            playerCamera.m_Lens.FieldOfView = defaultFOV;
            zoomOverlay.SetActive(false);
            firstPersonController.ChangeRotationSpeed(defaultRotationSpeed);
        }
    }
}
