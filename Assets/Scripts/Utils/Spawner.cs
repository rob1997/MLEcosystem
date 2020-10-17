using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Spawner
{
    public static void Spawn(this Transform transform, float bounds, float radius, LayerMask groundMask)
    {
        bool safePositionFound = false;

        int attemptsRemaining = 100;

        Vector3 potentialPosition = Vector3.zero;
        Quaternion potentialRotation = new Quaternion();

        while (!safePositionFound && attemptsRemaining > 0)
        {
            attemptsRemaining--;

            float groundLevel = 1.5f;

            float randomX = Random.Range(- bounds + radius, bounds - radius);

            float safeZ = bounds - Mathf.Abs(randomX);
            
            float randomZ = Random.Range(- safeZ + radius, safeZ - radius);

            float yaw = Random.Range(-180f, 180f);
                
            //local position
            potentialPosition = new Vector3( randomX, groundLevel, randomZ);

            potentialRotation = Quaternion.Euler(0f, yaw, 0f);
                
            Collider[] results = Physics.OverlapSphere(potentialPosition, radius, groundMask);

            safePositionFound = results.Length == 0;
        }
            
        Debug.Assert(safePositionFound, "Could not find a safe position");

        transform.localPosition = potentialPosition;
        transform.rotation = potentialRotation;
    }
}
