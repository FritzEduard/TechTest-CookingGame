using System.Collections;
using UnityEngine;

public class Ingredient : MonoBehaviour
{
    public SO_Ingredient IngredientSO;
    public Rigidbody Rigidbody;

    private void Awake()
    {
        Rigidbody = GetComponent<Rigidbody>();
    }

    [ContextMenu("COOK")]
    public void Cook()
    {
        if (Rigidbody == null) return;

        Rigidbody.AddForce(Vector3.up, ForceMode.Impulse);

        Invoke("Cook", Random.Range(0.3f,0.5f));
    }
}
