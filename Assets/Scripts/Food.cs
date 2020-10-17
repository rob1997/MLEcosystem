using UnityEngine;

public class Food : MonoBehaviour
{
    #region Visualization

    public Material fullMaterial;
        
    public Material emptyMaterial;

    #endregion

    private FoodArea _foodArea;
    
    private float Amount { get; set; }

    public bool HasFood => Amount > 0f;
    
    public Vector3 FoodCenter => foodCollider.transform.position;
    
    public Collider foodCollider;
    
    [SerializeField] private Renderer foodRenderer;
    
    [SerializeField] private LayerMask groundMask;

    private void Start()
    {
        FindFoodArea(transform.GetComponentInParent<Transform>());
        
        _foodArea.FoodSources.Add(this);
    }

    void FindFoodArea(Transform parent)
    {
        _foodArea = parent.GetComponentInParent<FoodArea>();

        if (_foodArea == null)
        {
            FindFoodArea(_foodArea.GetComponentInParent<Transform>());   
        }
    }
    
    public float Feed(float amount)
    {
        float foodTaken = Mathf.Clamp(amount, 0f, Amount);

        Amount -= amount;

        if (Amount <= 0)
        {
            Amount = 0f;
                
            //Disable collider
            foodCollider.enabled = false;
            
            //Change to empty color
            foodRenderer.material = emptyMaterial;
        }

        return foodTaken;
    }

    /// <summary>
    /// Reset source
    /// </summary>
    public void ResetFood()
    {
        Amount = 1f;
        
        foodCollider.enabled = true;
        
        foodRenderer.material = fullMaterial;
        
        transform.Spawn(_foodArea.radius, Random.Range(1.5f, 2.5f), groundMask);
    }
}