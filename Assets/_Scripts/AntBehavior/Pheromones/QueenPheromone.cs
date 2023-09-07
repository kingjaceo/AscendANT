using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

class QueenPheromone : IPheromone
{
    private Ant _ant;

    public PheromoneName PheromoneName { get; set; } = PheromoneName.Queen;

    public QueenPheromone()
    {}
    
    public QueenPheromone(Ant ant)
    {
        _ant = ant;
    }

    public void Start()
    {
        Debug.Log("Starting the Queen pheromone ... ");
        Colony colony = _ant.Colony;
        Vector3 position = colony.Transform.position;
        AntBehaviorMachine machine = _ant.AntBehaviorMachine;
        Debug.Log("Machine: " + machine.ToString());
        CircleBehavior circle = machine.Circle;
        
        circle.SetTarget(LocationType.Colony, position);
        circle.SetMinMaxRadius(2f, 3f);
        machine.TransitionTo(circle);
    }

    public void Update()
    {}

    public void Finish()
    {}

    public IPheromone Copy(Ant ant)
    {
        return new QueenPheromone(ant);
    }

    public void OnCollision(GameObject collider)
    {}

    public override string ToString()
    {
        return "Queen";
    }
}