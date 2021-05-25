using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class CookingPanSFXController : MonoBehaviour
{
    [SerializeField] private CookingController cookingController;
    [SerializeField] private AudioClip cookingClip;
    [SerializeField] private AudioClip cookingDoneClip;
    private AudioSource audioSource;
    // Start is called before the first frame update
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void PlayCookingSound()
    {
        audioSource.clip = cookingClip;
        audioSource.Play();
    }
    private void PlayCookingDoneSound()
    {
        audioSource.Stop();
        audioSource.clip = cookingDoneClip;
        audioSource.Play();
    }

    private void OnEnable()
    {
        cookingController.OnStartCooking += PlayCookingSound;
        cookingController.OnCookingDone += PlayCookingDoneSound;
    }
    private void OnDisable()
    {
        cookingController.OnStartCooking -= PlayCookingSound;
        cookingController.OnCookingDone -= PlayCookingDoneSound;
    }
}
