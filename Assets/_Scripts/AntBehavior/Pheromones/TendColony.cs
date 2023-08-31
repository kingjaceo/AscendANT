using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

class TendColony : IPheromone
{
    public TendColony()
    {
        Debug.Log("TendColony pheromone created!");
    }

    public void Start()
    {

    }

    public void Update()
    {

    }

    public void Finish()
    {

    }

    public IPheromone Copy(Ant ant)
    {
        return new TendColony();
    }

    // public override Vector3 GetDirection(Ant ant)
    // {
    //     Vector3 direction = ant.Colony.GetQueenPosition() - ant.transform.position;
    //     direction.y = 0;
    //     return direction;
    // }

    // public override void UpdateStates(Ant ant, Collision collision)
    // {
    //     ant.SetAntState(AntState.TendingColony);
    //     if (collision.gameObject.name == "Queen(Clone)")
    //     {
    //         // give some attention to the nest's eggs
    //         Debug.Log("Ant" + ant.ID + " collides with the Queen, tending eggs");
    //         ant.Colony.TendEggs();
    //     }
    // }
}