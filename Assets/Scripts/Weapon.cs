using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] ParticleSystem muzzleFlashSpark;
    [SerializeField] ParticleSystem muzzleFlashGlow;
    public void PlayerShoot(WeaponSO weaponSO)
    {

        muzzleFlashSpark.Play();

        muzzleFlashGlow.Play();

        RaycastHit hit;

        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, Mathf.Infinity, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore))
        {
            Instantiate(weaponSO.HitVFX, hit.point, Quaternion.identity);

            EnemyHealth enemyHealth = hit.collider.GetComponent<EnemyHealth>();

            enemyHealth?.TakeDamage(weaponSO.Damage);
        }
    }
}
