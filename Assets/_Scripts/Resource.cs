using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : MonoBehaviour
{
    public ResourceType ResourceType;
    public float AmountRemaining = 200;

    public float Harvest(float amount)
    {
        amount = Math.Min(amount, AmountRemaining);
        AmountRemaining -= amount;
        if (AmountRemaining <= 0){
            DestroySelf();
        }
        return amount;
    }

    private void DestroySelf()
    {
        Debug.Log("RESOURCE: " + ResourceType + " depleted, destroying self ... ");
        
        Destroy(gameObject);
    }
}