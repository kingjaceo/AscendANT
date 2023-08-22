using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class Report : Pheromone
{
    public override PheromoneName pheromoneName { get; set; }

    public Report()
    {
        pheromoneName = PheromoneName.Report;
    }

    public override Vector3 GetDirection(Ant ant)
    {
        Vector3 direction = ant.colony.transform.position - ant.transform.position;
        ant.antState = AntState.Reporting;
        return direction;
    }

    public override float GetDelay(Ant ant)
    {
        return 1f;
    }

    public override void UpdateStates(Ant ant, Collision collision)
    {
        if (collision.gameObject.name == "Colony")
        {
            ant.pheromoneState = PheromoneState.Complete;
            ant.antState = AntState.Idle;
        }
    }
}