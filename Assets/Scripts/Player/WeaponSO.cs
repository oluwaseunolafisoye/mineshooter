using UnityEngine;

[CreateAssetMenu(fileName = "WeaponSO", menuName = "Scriptable Objects/WeaponSO")]
public class WeaponSO : ScriptableObject
{
    public GameObject WeaponPrefab;
    public int Damage = 1;
    public float FireRate = 0.5f;
    public GameObject HitVFX;
    public bool IsAutomatic = false;
    public bool CanZoom = false;
    public float ZoomFOV = 30f;
    public float ZoomRotationSpeed = .3f;
    public int MagazineSize = 10;
    public AudioClip FireSound;
    [Range(0f, 1f)] public float FireSoundVolume = 1f;
}
