using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Grass : Food
{
    #region Visualization

    public Material fullMaterial;
        
    public Material emptyMaterial;

    #endregion

    private FoodArea _foodArea;
    
    public bool HasFood => Amount > 0f;
    
    public Vector3 FoodCenter => _foodCollider.transform.position;
    
    private Collider _foodCollider;
    
    private Renderer _foodRenderer;
    
    [SerializeField] private LayerMask groundMask;

    protected override void Awake()
    {
        base.Awake();
        
        _foodCollider = GetComponent<Collider>();
        _foodRenderer = GetComponent<Renderer>();
    }

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

//    protected override void OnEmpty()
//    {
//        //Disable collider
//        _foodCollider.enabled = false;
//            
//        //Change to empty color
//        _foodRenderer.material = emptyMaterial;
//    }
    
    protected override void OnEmpty()
    {
        ResetFood();
    }

    /// <summary>
    /// Reset source
    /// </summary>
//    public override void ResetFood()
//    {
//        base.ResetFood();
//        
//        _foodCollider.enabled = true;
//        
//        _foodRenderer.material = fullMaterial;
//        
//        transform.Spawn(_foodArea.radius, Random.Range(1.5f, 2.5f), groundMask);
//    }
    
    public override void ResetFood()
    {
        base.ResetFood();
        
        transform.Spawn(_foodArea.radius, Random.Range(1.5f, 2.5f), groundMask);
    }
}