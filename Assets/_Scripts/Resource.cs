using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : MonoBehaviour
{
    public ResourceType ResourceType;
    public float AmountRemaining = 1000;

    public float Harvest(float amount)
    {
        amount = Math.Min(amount, AmountRemaining);
        AmountRemaining -= amount;
        return amount;
    }
}