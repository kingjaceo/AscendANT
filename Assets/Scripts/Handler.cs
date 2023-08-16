using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Handler : MonoBehaviour
{
    public GameObject world;
    public GameObject colony; 
    public GameObject curiosity;

    // Start is called before the first frame update
    void Awake()
    {
        world = Instantiate(world);
        world.transform.position = new Vector3(0, 0, 0);

        GameObject mainColony = Instantiate(colony);
        mainColony.transform.position = new Vector3(-3, 0, 3);

        GameObject initialCuriosity = Instantiate(curiosity);
        initialCuriosity.transform.position = new Vector3(3, 0, 3);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
