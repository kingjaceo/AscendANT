using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Colony : MonoBehaviour
{ 
    public int NumAnts = 0;

    public Memory Memory;
    public Dictionary<ResourceType, float> ResourceAmounts = new Dictionary<ResourceType, float>();
    public float HatchProgress = 0;

    [SerializeField] private GameObject _antPrefab;
    private List<Ant> _ants;
    public Dictionary<string, float> AntsByCaste = new Dictionary<string, float>();
    public Caste[] Castes { get; private set;}

    [SerializeField] private GameObject _queenPrefab;
    private Queen _queen;

    private Transform _transform;
    
    // Start is called before the first frame update
    void Awake()
    {
        Debug.Log("New Colony starts up...");
        _transform = transform;

        InitializeColony();
        InitializeCastes();
        InitializeQueen();
        StartCoroutine(InitializeAnts());
        StartCoroutine(HatchEggs());

    }

    // Update is called once per frame
    void Start()
    {
        MainGUIPanel.Instance.GiveColony(this);
    }

    // initalizes the colony according to default parameters
    private void InitializeColony()
    {
        ResourceAmounts[ResourceType.Food] = 100f;
        ResourceAmounts[ResourceType.Water] = 100f;
        ResourceAmounts[ResourceType.Eggs] = 3f;
        Memory = new Memory(this);        
    }

    private void InitializeCastes()
    {
        ResourceType target = ResourceType.Food | ResourceType.Curiosity;
        Scout scout = new Scout(target);
        Harvest harvest = new Harvest(ResourceType.Food);
        TendColony tendColony = new TendColony();

        List<IPheromone> attendantPheromoneSequence = new List<IPheromone> { tendColony };

        List<IPheromone> scoutPheromoneSequence = new List<IPheromone> { scout };

        List<IPheromone> workerPheromoneSequence = new List<IPheromone> { harvest };
        
        Caste caste0 = new Caste("Attendant", 0.1f, 1f, attendantPheromoneSequence);
        Caste caste1 = new Caste("Scout", 0.4f, 2f, scoutPheromoneSequence);
        Caste caste2 = new Caste("Worker", 0.5f, 3f, workerPheromoneSequence);

        Caste[] castes = { caste0, caste1, caste2 };
        Castes = castes;
    }

    private void InitializeQueen()
    {
        CircleColony circle = new CircleColony();
        List<IPheromone> queenPheromoneSequence = new List<IPheromone> { circle };
        Caste casteQ = new Caste("Queen", 0f, 0.25f, queenPheromoneSequence);
       
        _queen = Instantiate(_queenPrefab).GetComponent<Queen>();
        _queen.AssignColony(this);

        // set the Ant's parent
        _queen.transform.parent = transform;

        // set the Ant's position, colony, and caste
        _queen.transform.position = RandomLocationAroundColony(0.5f, 1.5f);
        _queen.AssignColony(this);
        _queen.AssignCaste(casteQ);
    }

    // initializes the default number and types of ants
    private IEnumerator InitializeAnts()
    {
        int[] amountOfAntsInCastes = { 1, 17, 2 };

        for (int i = 0; i < Castes.Length; i++) 
        {
            AntsByCaste[Castes[i].Name] = 0;
            for (int j = 0; j < amountOfAntsInCastes[i]; j++)
            {
                NextAnt(Castes[i]);

                yield return new WaitForSeconds(0.01f);
            }

            
        }
    }

    public Vector3 RandomLocationAroundColony(float minRadius, float maxRadius)
    {
        var x = _transform.position.x + (UnityEngine.Random.Range(0, 2) * 2 - 1) * UnityEngine.Random.Range(minRadius, maxRadius);
        var y = 1f;
        var z = _transform.position.z + (UnityEngine.Random.Range(0, 2) * 2 - 1) * UnityEngine.Random.Range(minRadius, maxRadius);

        return new Vector3(x, y, z);
    }

    public void TendEggs()
    {
        float tendAmount = 0.1f;

        if (ResourceAmounts[ResourceType.Food] > 10)
        {
            ResourceAmounts[ResourceType.Food] -= tendAmount / 10;
            HatchProgress += tendAmount;
        }
    }

    IEnumerator HatchEggs()
    {
        while(true)
        {
            if (HatchProgress > 100 & ResourceAmounts[ResourceType.Eggs] > 0) {
                // reduce eggs and reset hatchProgress
                ResourceAmounts[ResourceType.Eggs]--;
                HatchProgress = 0;

                // produce a new ant
                Caste caste = NextCaste();
                NextAnt(caste);
            }

            yield return null;
        }
    }

    private void NextAnt(Caste caste)
    {
        // create the Ant
        Ant ant = Instantiate(_antPrefab).GetComponent<Ant>();

        // set the Ant's parent
        ant.transform.parent = transform;

        // set the Ant's position, colony, and caste
        ant.transform.position = RandomLocationAroundColony(0f, 1f);
        ant.AssignColony(this);
        ant.AssignCaste(caste);

        // update colony variables
        NumAnts++;
        // _ants.Add(ant);
        AntsByCaste[caste.Name] ++;
    }

    private Caste NextCaste()
    {
        // compare Caste percentages to real percentages:
        int casteIndex = 0;
        float maxPercentageDifference = 0;
        for (int i = 0; i < Castes.Length; i++) 
        {
            float realPercentage = AntsByCaste[Castes[i].Name] / NumAnts;
            float targetPercentage = Castes[i].Percentage;
            float percentageDifference = targetPercentage - realPercentage;
            if (percentageDifference > maxPercentageDifference)
            {
                casteIndex = i;
                maxPercentageDifference = percentageDifference;
            }
        }

        return Castes[casteIndex];
    }

    public void UpdateCastePercentages(float[] newCastePercentages)
    {
        for (int i = 0; i < Castes.Length; i++)
        {
            Castes[i].SetPercentage(newCastePercentages[i]);
        }
    }

    public Vector3 GetQueenPosition()
    {
        return _queen.transform.position;
    }
}
