using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleBehavior : IAntBehavior
{
    private Ant _ant;
    private Vector3 _target;

    public CircleBehavior(Ant ant)
    {
        _ant = ant;
    }

    public void Begin()
    {
        Debug.Log("Ant" + _ant.ID + " begins circling " + _ant.LocationTarget);
    }

    public void Update()
    {
        Vector3 radius = _target - _ant.Transform.position;
        Vector3 direction = radius;
        
        float radiusLength = radius.magnitude;
        if (radiusLength > 0.5f & radiusLength < 1f)
        {
            // begin travelling perpindicular
            Vector2 radius2D = new Vector2(radius.x, radius.z);
            Vector2 newDirection2D = Vector2.Perpendicular(radius2D);
            direction = new Vector3(newDirection2D.x, _ant.Transform.forward.y, newDirection2D.y);
            // direction = Quaternion.AngleAxis(Random.Range(-5, 5), _ant.Transform.up) * direction;
        }
        else if (radiusLength < 0.5f)
        {
            // begin moving away from the target
            direction = -direction;
        }
        
        _ant.SetDirection(direction);
    }

    public void End()
    {
        Debug.Log("Ant" + _ant.ID + " finishes circling " + _ant.LocationTarget);
    }

    public void SetTarget(Vector3 target)
    {
        _target = target;
    }
}