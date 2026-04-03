using Cinemachine;
using StarterAssets;
using UnityEngine;

public class ActiveWeapon : MonoBehaviour
{
    [SerializeField] WeaponSO weaponSO;
    [SerializeField] CinemachineVirtualCamera playerCamera;
    [SerializeField] GameObject zoomOverlay;
    Animator animator;
    StarterAssetsInputs starterAssetsInputs;
    FirstPersonController firstPersonController;
    Weapon activeWeapon;

    const string SHOOT_STRING = "Shoot";

    float timeSinceLastShot = 0f;
    float defaultFOV;
    float defaultRotationSpeed;

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
        activeWeapon = GetComponentInChildren<Weapon>();
    }

    void Update()
    {
        PlayerShoot();
        PlayerZoom();
    }

    public void SwitchWeapon(WeaponSO weaponSO)
    {
        if (activeWeapon)
        {
            Destroy(activeWeapon.gameObject);
        }

        Weapon newWeapon = Instantiate(weaponSO.WeaponPrefab, transform).GetComponent<Weapon>();
        activeWeapon = newWeapon;
        this.weaponSO = weaponSO;
    }

    void PlayerShoot()
    {
        timeSinceLastShot += Time.deltaTime;

        if (!starterAssetsInputs.shoot)
            return;

        if (timeSinceLastShot < weaponSO.FireRate)
            return;

        activeWeapon.PlayerShoot(weaponSO);
        animator.Play(SHOOT_STRING, 0, 0f);
        timeSinceLastShot = 0f;

        if (!weaponSO.IsAutomatic)
        {
            starterAssetsInputs.ShootInput(false);
        }
    }

    void PlayerZoom()
    {
        if (!weaponSO.CanZoom) return;

        if (starterAssetsInputs.zoom)
        {
            playerCamera.m_Lens.FieldOfView = weaponSO.ZoomFOV;
            zoomOverlay.SetActive(true);
            firstPersonController.ChangeRotationSpeed(weaponSO.ZoomRotationSpeed);
        }
        else
        {
            playerCamera.m_Lens.FieldOfView = defaultFOV;
            zoomOverlay.SetActive(false);
            firstPersonController.ChangeRotationSpeed(defaultRotationSpeed);
        }
    }
}
