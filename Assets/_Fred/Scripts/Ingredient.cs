using System.Collections;
using UnityEngine;

public class Ingredient : MonoBehaviour
{
    public SO_Ingredient IngredientSO;
    public Rigidbody Rigidbody;

    [Header("Settings")]
    [SerializeField] private float miniJumpDelay; //Minimum delay before making the ingredient jump for his cooking "animation"
    [SerializeField] private float maxJumpDelay; //Maximum delay before making the ingredient jump for his cooking "animation"

    private void Awake()
    {
        Rigidbody = GetComponent<Rigidbody>();
    }

    public void Cook()
    {
        if (Rigidbody == null) return;

        Invoke("AddImpulse", Random.Range(0.3f,0.5f));
    }

    private void AddImpulse()
    {
        Rigidbody.AddForce(Vector3.up, ForceMode.Impulse);
    }
}
