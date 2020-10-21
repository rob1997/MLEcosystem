using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Meat : Food
{
    protected override void OnEmpty()
    {
        ResetFood();
    }
}