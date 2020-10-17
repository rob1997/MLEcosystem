using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class FoodArea : MonoBehaviour
{
    public float radius;

    public Transform world;
    
    public List<Food> FoodSources { get; } = new List<Food>();

    public void ResetSources()
    {
        Quaternion localRotation = world.localRotation;
        
        float rotationX = localRotation.eulerAngles.x;
        float rotationY = Random.Range(-180f, 180f);
        float rotationZ = localRotation.eulerAngles.z;
        
        localRotation = Quaternion.Euler(rotationX, rotationY, rotationZ);
        
        world.localRotation = localRotation;
        
        FoodSources.ForEach(f => f.ResetFood());
    }
}