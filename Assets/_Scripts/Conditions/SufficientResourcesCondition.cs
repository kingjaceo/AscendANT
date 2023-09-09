using System;
using UnityEngine;

public class SufficientResourcesCondition : VictoryCondition 
{
    private ResourceType[] _requiredResourceTypes;
    private float[] _requiredResourceAmounts;
    private int _numResources;

    public SufficientResourcesCondition()
    {
        Description = "Get enough of any resource!";
    }

    public SufficientResourcesCondition(ResourceType[] requiredResourceTypes, float[] requiredResourceAmounts, string reward) 
    {
        _numResources = requiredResourceAmounts.Length;
        
        Description = "Get ";
        for (int i = 0; i < _numResources; i++)
        {
            if (i > 0 && i == _numResources - 1)
            {
                Description += " and ";
            }
            
            Description += requiredResourceAmounts[i] + " " + requiredResourceTypes[i];

            if (_numResources > 2 && i < _numResources - 1)
            {
                Description += ", ";
            }

            
        }

        Description += ".";
        _requiredResourceTypes = requiredResourceTypes;
        _requiredResourceAmounts = requiredResourceAmounts;

        Reward = reward;

    }

    public override void CheckConditionMet()
    {
        // Debug.Log("VICTORY: Checking SufficientResourcesCondition ... ");
        // on update of any colony-level resource, recalculate success
        for (int i = 0; i < _requiredResourceTypes.Length; i++)
        {
            
            ResourceType requiredResource = _requiredResourceTypes[i];
            float requiredAmount = _requiredResourceAmounts[i];
            float currentAmount = WorldManager.Instance.World.Colony.ColonyResources.Amount(requiredResource);
            // Debug.Log("VICTORY: SufficientResourcesCondition has status " + requiredResource + ", " + requiredAmount + ", " + currentAmount);
            if (currentAmount < requiredAmount)
            {
                return;
            }
        }

        GameManager.Instance.ChangeState(GameState.VictoryScreen);
    }

    public override string Progress()
    {
        string progressReport = "";

        for (int i = 0; i < _requiredResourceTypes.Length; i++)
        {
            ResourceType requiredResource = _requiredResourceTypes[i];
            float requiredAmount = _requiredResourceAmounts[i];
            float currentAmount = WorldManager.Instance.World.Colony.ColonyResources.Amount(requiredResource);
            
            progressReport += Mathf.Round(currentAmount) + " / " + Mathf.Round(requiredAmount) + " " + requiredResource;
            progressReport += "    ";
        }

        return progressReport;
    }

    public override void BeginListening()
    {
        Debug.Log("VICTORY: Adding a listener for SufficientResourcesCondition");
        WorldManager.Instance.World.Colony.ColonyResources.ResourceIncreased.AddListener(CheckConditionMet);
    }

    public override void StopListening()
    {
        Debug.Log("VICTORY: Removing a listener for SufficientResourcesCondition");
        WorldManager.Instance.World.Colony.ColonyResources.ResourceIncreased.RemoveListener(CheckConditionMet);
    }
}