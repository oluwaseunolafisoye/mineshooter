using UnityEngine;

public class PlayerFootsteps : MonoBehaviour
{
    [SerializeField] AudioClip footstepSound;
    [Range(0f, 1f)][SerializeField] float volume = 0.5f;
    [SerializeField] float minMoveSpeed = 0.1f;

    AudioSource audioSource;
    CharacterController characterController;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        characterController = GetComponent<CharacterController>();

        audioSource.clip = footstepSound;
        audioSource.loop = true;
    }

    void Update()
    {
        bool isMovingOnGround = characterController.isGrounded && characterController.velocity.magnitude > minMoveSpeed;

        if (isMovingOnGround && !audioSource.isPlaying)
        {
            audioSource.volume = volume;
            audioSource.Play();
        }
        else if (!isMovingOnGround && audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }
}
