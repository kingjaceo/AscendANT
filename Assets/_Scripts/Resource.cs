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
        if (AmountRemaining <= 0){
            StartCoroutine(DestroySelf());
        }
        return amount;
    }

    private IEnumerator DestroySelf()
    {
        yield return new WaitForSeconds(0.2f);

        Destroy(gameObject);
    }
}