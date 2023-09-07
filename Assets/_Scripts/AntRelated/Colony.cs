using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Colony : MonoBehaviour
{ 
    public int NumAnts = 0;

    public Memory Memory;
    
    public ColonyResources ColonyResources { get; private set; }

    public float HatchProgress = 0;

    [SerializeField] private GameObject _antPrefab;
    private List<Ant> _ants;
    public int[] AntCountByCaste;
    public List<Ant>[] AntsByCaste;
    public Caste[] Castes { get; private set;}

    [SerializeField] private GameObject _queenPrefab;
    private Queen _queen;
    public Queen Queen => _queen;

    private Transform _transform;
    public Transform Transform => _transform;

    // Start is called before the first frame update
    void Awake()
    {
        Debug.Log("New Colony starts up...");
        _transform = transform;

        InitializeColony();
        InitializeCastes();
        
    }

    // Update is called once per frame
    void Start()
    {
        InitializeQueen();
        StartCoroutine(InitializeAnts());
        StartCoroutine(HatchEggs());
    }

    // initalizes the colony according to default parameters
    private void InitializeColony()
    {
        Dictionary<ResourceType, float> resourceAmounts = new Dictionary<ResourceType, float>();
        resourceAmounts[ResourceType.Food] = 990f;
        resourceAmounts[ResourceType.Water] = 990f;
        resourceAmounts[ResourceType.Eggs] = 95f;

        Dictionary<ResourceType, float> resourceCapacities = new Dictionary<ResourceType, float>();
        resourceCapacities[ResourceType.Food] = 1100f;
        resourceCapacities[ResourceType.Water] = 1000f;
        resourceCapacities[ResourceType.Eggs] = 110f;

        ColonyResources = new ColonyResources(resourceAmounts, resourceCapacities);

        Memory = new Memory(this);        
    }

    private void InitializeCastes()
    {
        // ResourceType target = ResourceType.Food | ResourceType.Curiosity;
        Scout scout = new Scout();
        Harvest harvest = new Harvest(ResourceType.Food);
        TendColony tendColony = new TendColony();

        List<IPheromone> attendantPheromoneSequence = new List<IPheromone> { tendColony };

        List<IPheromone> scoutPheromoneSequence = new List<IPheromone> { scout };

        List<IPheromone> workerPheromoneSequence = new List<IPheromone> { harvest };
        
        Caste caste0 = new Caste("Attendant", 1f, attendantPheromoneSequence);
        Caste caste1 = new Caste("Scout", 2f, scoutPheromoneSequence);
        Caste caste2 = new Caste("Worker", 3f, workerPheromoneSequence);

        Caste[] castes = { caste0, caste1, caste2 };

        Castes = castes;
    }

    private void InitializeQueen()
    {
        QueenPheromone queenPheromone = new QueenPheromone();
        List<IPheromone> queenPheromoneSequence = new List<IPheromone> {queenPheromone};
        Caste casteQ = new Caste("Queen", 0.5f, queenPheromoneSequence);
       
        GameObject queenObject = Instantiate(_queenPrefab);
        _queen = queenObject.GetComponent<Queen>();
        _queen.Transform.parent = transform;
        _queen.AssignColony(this);
        _queen.AssignCaste(casteQ);
        queenObject.name = "Queen";

        // set the Ant's parent
        

        // set the Ant's position, colony, and caste
        _queen.Transform.position = RandomLocationAroundColony(2f, 1.5f);
        _queen.Transform.GetChild(0).GetComponent<Renderer>().material.color = PaletteManager.Colors[PaletteManager.Colors.Count - 1];
    }

    // initializes the default number and types of ants
    private IEnumerator InitializeAnts()
    {
        int[] amountOfAntsInCastes = { 1, 17, 2 };
        AntCountByCaste = new int[Castes.Length];
        AntsByCaste = new List<Ant>[Castes.Length];

        for (int i = 0; i < Castes.Length; i++) 
        {
            Debug.Log("Creating ants in caste " + i);
            AntCountByCaste[i] = 0;
            AntsByCaste[i] = new List<Ant>();
            for (int j = 0; j < amountOfAntsInCastes[i]; j++)
            {
                Debug.Log("Creating ant " + NumAnts); 
                NextAnt(i);

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

        if (ColonyResources.Amount(ResourceType.Food) > 10)
        {
            ColonyResources.DepleteResource(ResourceType.Food, tendAmount / 10);
            HatchProgress += tendAmount;
        }
    }

    IEnumerator HatchEggs()
    {
        while(true)
        {
            if (HatchProgress > 100 & ColonyResources.Amount(ResourceType.Eggs) > 0) {
                // reduce eggs and reset hatchProgress
                ColonyResources.DepleteResource(ResourceType.Food, 1);
                HatchProgress = 0;

                // produce a new ant
                int casteIndex = NextCaste();
                NextAnt(casteIndex);
            }

            yield return null;
        }
    }

    private void NextAnt(int casteIndex)
    {
        Caste caste = Castes[casteIndex];

        // create the Ant
        GameObject antObject = Instantiate(_antPrefab);
        Ant ant = antObject.GetComponent<Ant>();

        // set the Ant's parent
        ant.transform.parent = transform;

        // set the Ant's position, colony, and caste
        ant.transform.position = RandomLocationAroundColony(0f, 1f);
        ant.AssignColony(this);
        ant.AssignCaste(caste);

        // set the color and name
        Color color = PaletteManager.Colors[casteIndex * 2];
        ant.Transform.GetChild(0).GetComponent<Renderer>().material.color = color;
        antObject.name = caste.Name + "Ant" + ant.ID;
        Debug.Log(antObject.name + " gets color: " + color.ToString());

        // update colony variables
        NumAnts++;
        AntCountByCaste[casteIndex] ++;
        AntsByCaste[casteIndex].Add(ant);
    }

    private int NextCaste()
    {
        // compare Caste percentages to real percentages:
        int casteIndex = 0;
        float maxPercentageDifference = 0;
        for (int i = 0; i < Castes.Length; i++) 
        {
            float realPercentage = AntCountByCaste[i] / (float) NumAnts;
            float targetPercentage = Castes[i].Percentage;
            float percentageDifference = targetPercentage - realPercentage;
            if (percentageDifference > maxPercentageDifference)
            {
                casteIndex = i;
                maxPercentageDifference = percentageDifference;
            }
        }
        
        // Debug.Log("Next Caste chosen to be " + casteIndex + ", since real percentage is " + AntCountByCaste[casteIndex] + " / " +  NumAnts + " and caste percentage is " + Castes[casteIndex].Percentage);
        return casteIndex;
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

    public void ConnectCastesToGUI()
    {
        for (int i = 0; i < Castes.Length; i++)
        {
            Debug.Log("Connecting Caste " + i + " to ColorDropdown " + i);
            Caste caste = Castes[i];
            GUIMainPanelController.Instance.SetColony(this);
            GUIMainPanelController.Instance.CastePanels[i].SetCaste(caste);
            GUIMainPanelController.Instance.CastePanels[i].SetColony(this);
        }
    }

    public void ChangeColor(int casteIndex)
    {
        Debug.Log("Attempting to change the color of Caste " + casteIndex + " to ");
        
        Color color = GUIMainPanelController.Instance.CastePanels[casteIndex].GetColor();
        
        for (int j = 0; j < AntCountByCaste[casteIndex]; j++)
        {
            Ant ant = AntsByCaste[casteIndex][j];
            ant.Transform.GetChild(0).GetComponent<Renderer>().material.color = color;
        }  
    }
}