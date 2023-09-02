using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPheromone
{
    public void Start();

    public void Update();

    public void Finish();

    public IPheromone Copy(Ant ant);

    // public void CollidedWithTarget(GameObject target);

    public void OnCollision(GameObject collider);
}