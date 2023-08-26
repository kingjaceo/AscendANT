using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Queen : MonoBehaviour
{
    private float timeToLayEgg;
    private float timeSinceLastEgg;
    private Colony colony;

    // Start is called before the first frame update
    void Start()
    {
        timeToLayEgg = 10f;
    }

    // Update is called once per frame
    void Update()
    {
        timeSinceLastEgg += Time.deltaTime;

        if (timeSinceLastEgg > timeToLayEgg) 
        {
            LayEgg();
            timeSinceLastEgg = 0;
        }
    }

    public void AssignColony(Colony colony)
    {
        this.colony = colony;
    }

    private void LayEgg()
    {
        colony.ResourceAmounts[ResourceType.Eggs]++;
    }
}
