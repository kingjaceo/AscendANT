using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WanderBehavior : IAntBehavior
{
    private Ant _ant;

    public WanderBehavior(Ant ant)
    {
        _ant = ant;
    }

    public void Begin()
    {
    }

    public void Update()
    {
        Vector3 direction = Quaternion.AngleAxis(UnityEngine.Random.Range(-5f, 5f), _ant.Transform.up) * _ant.Transform.forward;
        _ant.SetDirection(direction);

        // ask the PheromoneMachine if the ant needs to change behaviors
    }

    public void End()
    {
    }
}