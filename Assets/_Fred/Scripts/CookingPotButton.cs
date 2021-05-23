using UnityEngine;

public class CookingPotButton : MonoBehaviour
{
    [SerializeField] private Animation animation;
    [SerializeField] private CookingController cookingController;
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Hand"))
        {
            animation.Play();
            cookingController.Cook();
        }
    }
}
