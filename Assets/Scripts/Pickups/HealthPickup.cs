using UnityEngine;

public class HealthPickup : Pickup
{
    [SerializeField] int healAmount = 1;

    protected override void OnPickup(ActiveWeapon activeWeapon)
    {
        PlayerHealth playerHealth = activeWeapon.GetComponentInParent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.Heal(healAmount);
        }
    }
}
