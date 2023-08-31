// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// class Report : IPheromone
// {
//     public override PheromoneName pheromoneName { get; set; }

//     public Report()
//     {
//         pheromoneName = PheromoneName.Report;
//     }

//     public override Vector3 GetDirection(Ant ant)
//     {
//         Vector3 direction = ant.Colony.transform.position - ant.transform.position;
//         ant.SetAntState(AntState.Reporting);
//         return direction;
//     }

//     public override float GetDelay(Ant ant)
//     {
//         return 1f;
//     }

//     public override void UpdateStates(Ant ant, Collision collision)
//     {
//         if (collision.gameObject.name == "Colony")
//         {
//             ant.SetPheromoneState(PheromoneState.Complete);
//             ant.SetAntState(AntState.Idle);
//         }
//     }
// }