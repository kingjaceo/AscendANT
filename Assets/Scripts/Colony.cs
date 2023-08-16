using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Colony : MonoBehaviour
{
    private int water;
    private int food;
    private int eggs;
    private int eggHatchTime;
    [SerializeField]
    private GameObject ant;
    private GameObject[][] ants;

    // Start is called before the first frame update
    void Start()
    {
        initializeColony();
    }

    // Update is called once per frame
    void Update()
    {
        // hatchEggs();
    }

    // initalizes the colony according to default parameters
    private void initializeColony()
    {
        this.water = 100;
        this.food = 100;
        this.eggs = 3;
        this.eggHatchTime = 10;
        initializeAnts();
    }

    // initializes the default number and types of ants
    private void initializeAnts()
    {
        Caste caste0 = new Caste(5);
        Caste caste1 = new Caste(10);
        Caste caste2 = new Caste(20);

        Caste[] castes = { caste0, caste1, caste2 };
        int[] amountOfAntsInCastes = { 1, 7, 2 };
        int numCastes = castes.Length;
        GameObject[][] ants = new GameObject[numCastes][];

        for (int i = 0; i < numCastes; i++)
        {
            Caste caste = castes[i];
            int amountOfAntsInCaste = amountOfAntsInCastes[i];

            GameObject[] antsInCaste = new GameObject[amountOfAntsInCaste];
            
            for (int j = 0; j < amountOfAntsInCaste; j++) {
                GameObject newAnt = Instantiate(ant);
                newAnt.GetComponent<Ant>().AssignColony(this);
                antsInCaste[j] = newAnt;
            }
        }

        this.ants = ants;
    }
}
