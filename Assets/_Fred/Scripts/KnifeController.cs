using UnityEngine;

public class KnifeController : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        Ingredient ingredient = collision.gameObject.GetComponentInParent<Ingredient>();
        if (ingredient == null) return;

        if (!ingredient.IngredientSO.IsCuttable) return;

        if (ingredient.IngredientSO.CutPrefab == null) return;

        Instantiate(ingredient.IngredientSO.CutPrefab, collision.transform.position, new Quaternion(0, 0, 0, 0), null);
        Instantiate(ingredient.IngredientSO.CutPrefab, collision.transform.position, new Quaternion(0, 180, 0, 0), null);

        Destroy(ingredient.gameObject);
    }
}
