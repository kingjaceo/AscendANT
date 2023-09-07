using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleBehavior : IAntBehavior
{
    private Ant _ant;

    public IdleBehavior(Ant ant)
    {
        _ant = ant;
    }

    public void Begin()
    {
    }

    public void Update()
    {
        Vector3 direction = Quaternion.AngleAxis(3, _ant.Transform.up) * _ant.Transform.forward;
        _ant.SetDirection(direction);
    }

    public void End()
    {
    }
}