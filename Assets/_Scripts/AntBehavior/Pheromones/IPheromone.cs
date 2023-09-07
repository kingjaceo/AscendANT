using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPheromone
{    
    public PheromoneName PheromoneName { get; set; }

    public void Start();

    public void Update();

    public void Finish();

    public IPheromone Copy(Ant ant);

    public void OnCollision(GameObject collider);

    public string ToString();
}