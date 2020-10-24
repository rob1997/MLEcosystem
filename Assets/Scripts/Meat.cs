using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Meat : Food
{
    protected override void OnEmpty()
    {
        ResetFood();
    }
    
    public override void ResetFood()
    {
        base.ResetFood();

        Animal animal = GetComponent<Animal>();
        
        transform.Spawn(animal.foodArea.radius, Random.Range(1.5f, 2.5f), animal.groundMask);
    }
}