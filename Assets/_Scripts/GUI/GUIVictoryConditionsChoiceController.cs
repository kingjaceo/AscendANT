using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GUIVictoryConditionsChoiceController : MonoBehaviour
{
    [SerializeField] private GameObject _victoryPanelTemplate;
    private List<GameObject> _victoryPanels = new List<GameObject>();
    private List<Button> _victorySelectButton = new List<Button>();
    private List<VictoryCondition> _victoryConditions = new List<VictoryCondition>();
    [SerializeField] private Button _backButton;

    public CurrentVictoryCondition CurrentVictoryCondition;

    private int _numConditions;

    void Awake()
    {
        CurrentVictoryCondition = new CurrentVictoryCondition();
        
        _backButton.onClick.AddListener(() => { GameManager.Instance.ChangeState(GameState.MainMenu); });

        CreateVictoryConditions();
        // List<VictoryCondition> conditionsForChoice = ChooseRandomVictoryConditions(_numConditions);
        List<VictoryCondition> conditionsForChoice = _victoryConditions;

        for (int i = 0; i < _numConditions; i++)
        {
            GameObject victoryPanel = Instantiate(_victoryPanelTemplate);
            victoryPanel.transform.position = transform.position;
            victoryPanel.transform.SetParent(transform);
            victoryPanel.SetActive(true);
            victoryPanel.GetComponentInChildren<TMP_Text>().text = conditionsForChoice[i].Description;
            victoryPanel.transform.position += new Vector3(i*300, 0, -600);
            Button victorySelectButton = victoryPanel.GetComponentInChildren<Button>();
            int k = i;
            victorySelectButton.onClick.AddListener(() => { VictoryButtonClicked(k); });

            _victoryPanels.Add(victoryPanel);
            _victorySelectButton.Add(victorySelectButton);
        }
    }

    private void CreateVictoryConditions()
    {
        ResourceType[] requiredResources = new ResourceType[] { ResourceType.Food };
        float[] requiredAmounts = new float[] { 1000 };
        string reward = "1 Adaptation";
        _victoryConditions.Add(new SufficientResourcesCondition(requiredResources, requiredAmounts, reward));

        // requiredResources = new ResourceType[] { ResourceType.Eggs };
        // requiredAmounts = new float[] { 100 };
        // reward = "1 Insight";
        // _victoryConditions.Add(new SufficientResourcesCondition(requiredResources, requiredAmounts, reward));

        // requiredResources = new ResourceType[] { ResourceType.Food, ResourceType.Eggs };
        // requiredAmounts = new float[] { 800, 80 };
        // reward = "1 Adaptation, 1 Insight";
        // _victoryConditions.Add(new SufficientResourcesCondition(requiredResources, requiredAmounts, reward));

        _numConditions = _victoryConditions.Count;
    }

    private void VictoryButtonClicked(int index)
    {
        GameManager.Instance.ChangeState(GameState.StartRound);
        CurrentVictoryCondition.Instance.SetCondition(_victoryConditions[index]);
    }

    // private List<VictoryCondition> ChooseRandomVictoryConditions(int amount)
    // {
    //     // List<VictoryCondition> victoryChoices = new List<VictoryCondition>();

    //     // for (int i = 0; i < amount; i++)
    //     // {

    //     // }
    // }
}