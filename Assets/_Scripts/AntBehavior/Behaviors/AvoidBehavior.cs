using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvoidBehavior : IAntBehavior
{
    private Ant _ant;
    private Vector3 _targetDirection;

    public AvoidBehavior(Ant ant)
    {
        _ant = ant;
    }

    public void Begin()
    {
        _targetDirection = Quaternion.AngleAxis(UnityEngine.Random.Range(90f, 270f), _ant.Transform.up) * _ant.Transform.forward;
        _ant.SetDirection(_targetDirection);
    }

    public void Update()
    {
    }

    public void End()
    {
    }
}