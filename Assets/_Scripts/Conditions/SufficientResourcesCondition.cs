using System;
using UnityEngine;

public class SufficientResourcesCondition : VictoryCondition 
{
    private ResourceType[] _requiredResourceTypes;
    private float[] _requiredResourceAmounts;

    public SufficientResourcesCondition()
    {
        Description = "Get enough of any resource!";
    }

    public SufficientResourcesCondition(ResourceType[] requiredResourceTypes, float[] requiredResourceAmounts, string reward) 
    {
        Description = "Get ";
        for (int i = 0; i < requiredResourceTypes.Length; i++)
        {
            Description += requiredResourceAmounts[i] + " " + requiredResourceTypes[i];
            if (i < requiredResourceTypes.Length - 1)
            {
                Description += ", ";
            }

            if (i > 0 && i == requiredResourceAmounts.Length - 1)
            {
                Description += "and ";
            }
        }
        Description += ".";
        _requiredResourceTypes = requiredResourceTypes;
        _requiredResourceAmounts = requiredResourceAmounts;

        Reward = reward;

    }

    public override void CheckConditionMet()
    {
        Debug.Log("VICTORY: Checking SufficientResourcesCondition ... ");
        // on update of any colony-level resource, recalculate success
        for (int i = 0; i < _requiredResourceTypes.Length; i++)
        {
            
            ResourceType requiredResource = _requiredResourceTypes[i];
            float requiredAmount = _requiredResourceAmounts[i];
            float currentAmount = WorldManager.Instance.World.Colony.ColonyResources.Amount(requiredResource);
            Debug.Log("VICTORY: SufficientResourcesCondition has status " + requiredResource + ", " + requiredAmount + ", " + currentAmount);
            if (currentAmount < requiredAmount)
            {
                return;
            }
        }

        GameManager.Instance.ChangeState(GameState.VictoryScreen);
    }

    public override void BeginListening()
    {
        Debug.Log("VICTORY: Adding a listener for SufficientResourcesCondition");
        WorldManager.Instance.World.Colony.ColonyResources.ResourceIncreased.AddListener(CheckConditionMet);
    }
}