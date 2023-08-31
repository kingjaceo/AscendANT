using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApproachBehavior : IAntBehavior
{
    private Ant _ant;
    public TargetType Target;

    public ApproachBehavior(Ant ant)
    {
        _ant = ant;
    }

    public void Begin()
    {
        Debug.Log("Ant" + _ant.ID + " begins approaching " + _ant.LocationTarget);
    }

    public void Update()
    {
        Vector3 direction = _ant.Colony.transform.position - _ant.transform.position;
        _ant.SetDirection(direction);
    }

    public void End()
    {
        Debug.Log("Ant" + _ant.ID + " finishes approaching " + _ant.LocationTarget);
    }

    public void SetTarget(TargetType target)
    {
        Target = target;
    }
}