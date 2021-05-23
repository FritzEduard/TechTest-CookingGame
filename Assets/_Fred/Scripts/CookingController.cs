using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CookingController : MonoBehaviour
{

    [SerializeField] private SO_Ingredient defaultResult;
    [SerializeField] private float defaultRecipeCookingDuration;
    [SerializeField] private List<SO_RecipeDefinition> recipes;
    [SerializeField] private List<SO_Ingredient> currentIngredients = new List<SO_Ingredient>();
    [SerializeField] private Image progressBar;
    [SerializeField] private List<FoodPosition> foodPositions;
    private SO_RecipeDefinition currentRecipe;
    private SO_Ingredient currentResult;
    private bool isValidRecipe = false;



    [SerializeField] private List<Transform> ingredientsTransform = new List<Transform>();

    private void AddIngredient(SO_Ingredient ingredient)
    {
        currentIngredients.Add(ingredient);
        CheckRecipeValidity();
    }
    private void RemoveIngredient (SO_Ingredient ingredient)
    {
        currentIngredients.Remove(ingredient); 
        CheckRecipeValidity();
    }

    private void CheckRecipeValidity()
    {
        currentIngredients.Sort();
        currentRecipe = null;

        foreach (var recipe in recipes)
        {
            recipe.Ingredients.Sort();

            if (currentIngredients.Count != recipe.Ingredients.Count) continue;

            for (int i = 0; i < currentIngredients.Count; i++)
            {
                if (currentIngredients[i].Name != recipe.Ingredients[i].Name)
                {
                    isValidRecipe = false;
                    break; //This is not the correct recipe . We stop checking for this recipe and go to the next one
                }
                
                Debug.Log("Ingredient Match!");
                isValidRecipe = true;
            }

            if(isValidRecipe)
            {
                Debug.Log("Valid Recipe!");
                currentRecipe = recipe;
                break;
            }
        }
    }
    [ContextMenu("COOK NOW!")]
    public void Cook()
    {
        StartCoroutine(_CookingCoroutine());
    }
    private IEnumerator _CookingCoroutine ()
    {
        float duration = 0.0f;
        float curentTime = 0.0f;
        if (currentRecipe == null)
        {
            duration = defaultRecipeCookingDuration;
            currentResult = defaultResult;
        }
        else
        {
            duration = currentRecipe.CookingTime;
            currentResult = currentRecipe.Result;
        }

        foreach (Transform ingredient in ingredientsTransform)
        {
            ingredient.GetComponent<Ingredient>().Cook();
        }

        while(curentTime < duration)
        {
            curentTime += Time.deltaTime;
            progressBar.fillAmount = curentTime / duration;
            yield return new WaitForEndOfFrame();
        }
        
        CookingDone();
    }

    private void CookingDone()
    {
        foreach (Transform item in ingredientsTransform)
        {
            Destroy(item.gameObject);
        }
        ingredientsTransform.Clear();
        currentIngredients.Clear();

        Instantiate(currentResult.Prefab , transform.position, Quaternion.identity , null);
    }

    //---------------UNITY EVENTS--------------//
    private void OnTriggerEnter(Collider other)
    {
        Ingredient ingredient = other.gameObject.GetComponentInParent<Ingredient>();
        if (ingredient == null) return;
        
        if(!ingredientsTransform.Contains(ingredient.transform))
        {
            ingredientsTransform.Add(ingredient.transform);
            AddIngredient(ingredient.IngredientSO);
            ingredient.transform.SetParent(transform);
            ingredient.transform.localScale = Vector3.one / 2;
            ingredient.transform.position = GetFoodPosition().position;
            ingredient.Rigidbody.constraints = RigidbodyConstraints.FreezePositionX;
            ingredient.Rigidbody.constraints = RigidbodyConstraints.FreezePositionZ;
        }
        
        
    }
    private void OnTriggerExit(Collider other)
    {
        Ingredient ingredient = other.gameObject.GetComponentInParent<Ingredient>();
        if (ingredient == null) return;

        ingredientsTransform.Remove(ingredient.transform);
        ingredient.transform.SetParent(null);
        ingredient.transform.localScale = Vector3.one;
        RemoveIngredient(ingredient.IngredientSO);
    }

    private Transform GetFoodPosition()
    {
        for (int i = 0; i < foodPositions.Count; i++)
        {
            if(!foodPositions[i].IsUsed)
            {
                return foodPositions[i].Position;
            }
        }
        return foodPositions[0].Position;
    }
}

[Serializable]
public class FoodPosition
{
    public Transform Position;
    public bool IsUsed;
}