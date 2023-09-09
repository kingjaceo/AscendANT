using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleBehavior : IAntBehavior
{
    private Ant _ant;

    private LocationType _targetLocation;
    public LocationType TargetLocation => _targetLocation;

    private Vector3 _targetPosition;
    public Vector3 TargetPosition => _targetPosition;

    private float _minRadiusFromTarget = 0.5f;
    private float _maxRadiusFromTarget = 1f;

    public CircleBehavior(Ant ant)
    {
        _ant = ant;
    }

    public void Begin()
    {
        // Debug.Log("CIRCLE: " + _ant + " begins circling " + _targetPosition);
    }

    public void Update()
    {
        Vector3 radius = _targetPosition - _ant.Transform.position;

        // point toward the target
        Vector3 direction = radius;
        
        float radiusLength = radius.magnitude;
        if (radiusLength > _minRadiusFromTarget & radiusLength < _maxRadiusFromTarget)
        {
            // begin travelling perpindicular
            Vector2 radius2D = new Vector2(radius.x, radius.z);
            Vector2 newDirection2D = Vector2.Perpendicular(radius2D);
            direction = new Vector3(newDirection2D.x, _ant.Transform.forward.y, newDirection2D.y);
            // direction = Quaternion.AngleAxis(Random.Range(-5, 5), _ant.Transform.up) * direction;
        }
        else if (radiusLength < _minRadiusFromTarget)
        {
            // begin moving away from the target
            direction = -direction;
        }
        
        // Debug.Log("CIRCLE: Setting " + _ant + " direction to " + direction);
        _ant.SetDirection(direction);
    }

    public void End()
    {
    }

    public void SetTarget(LocationType location, Vector3 position)
    {
        _targetLocation = location;
        _targetPosition = position;
    }

    public void SetMinMaxRadius(float minRadius, float maxRadius)
    {
        _minRadiusFromTarget = minRadius;
        _maxRadiusFromTarget = maxRadius;
    }
}