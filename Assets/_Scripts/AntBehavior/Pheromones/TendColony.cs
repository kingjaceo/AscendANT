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
        // After a few seconds, circle the Queen
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

    public void OnCollision(GameObject collider)
    {
        
    }

    public override string ToString()
    {
        return "TendColony";
    }
}