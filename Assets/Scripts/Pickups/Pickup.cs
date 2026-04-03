using UnityEngine;

public abstract class Pickup : MonoBehaviour
{
    [SerializeField] float bobHeight = 0.15f;
    [SerializeField] float bobSpeed = 2f;
    const string PLAYER_STRING = "Player";
    Vector3 startPosition;

    void Awake()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        float yOffset = Mathf.Sin(Time.time * bobSpeed) * bobHeight;
        transform.position = startPosition + Vector3.up * yOffset;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(PLAYER_STRING))
        {
            ActiveWeapon activeWeapon = other.GetComponentInChildren<ActiveWeapon>();
            OnPickup(activeWeapon);
            Destroy(this.gameObject);
        }
    }

    protected abstract void OnPickup(ActiveWeapon activeWeapon);
}
