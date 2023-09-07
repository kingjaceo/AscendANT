using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

class TendColony : IPheromone
{
    private float _timeElapsed = 0;
    private Ant _ant;

    public PheromoneName PheromoneName { get; set; } = PheromoneName.TendEggs;

    public TendColony()
    {
    }

    public TendColony(Ant ant)
    {
        _ant = ant;
    }

    public void Start()
    {

    }

    public void Update()
    {
        // Circle the Queen
        if (_timeElapsed > 3)
        {
            _ant.AntBehaviorMachine.Circle.SetTarget(LocationType.Queen, _ant.Colony.Queen.Transform.position);
            _ant.AntBehaviorMachine.TransitionTo(_ant.AntBehaviorMachine.Circle);
        }

        // if close enough to queen, increment egg progress
        if ((_ant.Transform.position - _ant.Colony.Queen.Transform.position).sqrMagnitude < 2)
        {
            _ant.Colony.TendEggs();
        }

        _timeElapsed += Time.deltaTime;
    }

    public void Finish()
    {

    }

    public IPheromone Copy(Ant ant)
    {
        return new TendColony(ant);
    }

    // public void CollidedWithTarget(GameObject target)
    // {
        
    // }

    public void OnCollision(GameObject collider)
    {
        
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

    public override string ToString()
    {
        return "TendColony";
    }
}