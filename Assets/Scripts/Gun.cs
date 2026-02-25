using StarterAssets;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] ParticleSystem muzzleFlashSpark;
    [SerializeField] ParticleSystem muzzleFlashGlow;

    [SerializeField] int damage = 1;
    StarterAssetsInputs starterAssetsInputs;

    const string SHOOT_STRING = "Shoot";


    void Awake()
    {
        starterAssetsInputs = GetComponentInParent<StarterAssetsInputs>();
    }

    void Update()
    {
        PlayerShoot();
    }

    void PlayerShoot()
    {
        if (!starterAssetsInputs.shoot) return;

        starterAssetsInputs.ShootInput(false);

        muzzleFlashSpark.Play();
        muzzleFlashGlow.Play();

        animator.Play(SHOOT_STRING, 0, 0f);

        RaycastHit hit;

        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, Mathf.Infinity))
        {
            EnemyHealth enemyHealth = hit.collider.GetComponent<EnemyHealth>();

            enemyHealth?.TakeDamage(damage);

            Debug.Log("Hit " + hit.collider.name);
        }


    }
}
