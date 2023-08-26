using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

class TendColony : Pheromone
{
    public ResourceType target;

    public override PheromoneName pheromoneName { get; set; }

    public TendColony()
    {
        pheromoneName = PheromoneName.TendColony;

        Debug.Log("TendColony pheromone created!");
    }

    public override Vector3 GetDirection(Ant ant)
    {
        Vector3 direction = ant.colony.transform.position - ant.transform.position;

        return direction;
    }

    public override void UpdateStates(Ant ant, Collision collision)
    {
        ant.antState = AntState.TendingColony;
        if (collision.gameObject.name == "Colony")
        {
            // give some attention to the nest's eggs
            ant.colony.TendEggs();
        }
    }
}