using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApproachBehavior : IAntBehavior
{
    private Ant _ant;

    private LocationType _targetLocation;
    public LocationType TargetLocation => _targetLocation;

    private ResourceType _targetResource;
    public ResourceType TargetResource => _targetResource;

    private Vector3 _targetPosition;
    public Vector3 TargetPosition => _targetPosition;

    public ApproachBehavior(Ant ant)
    {
        _ant = ant;
    }

    public void Begin()
    {
        // Debug.Log("BEHAVIOR: " + _ant + " begins approaching " + TargetLocation);
    }

    public void Update()
    {
        Vector3 direction = TargetPosition - _ant.transform.position;
        _ant.SetDirection(direction);
    }

    public void End()
    {
        // Debug.Log("BEHAVIOR: " + _ant + " finishes approaching " + TargetLocation);
    }

    public void SetTarget(LocationType target)
    {
        _targetLocation = target;
    }

    public void SetTargetPosition(Vector3 targetPosition)
    {
        _targetPosition = targetPosition;
    }
}