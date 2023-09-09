using System;

public abstract class VictoryCondition 
{
    public string Description;
    public string Reward;

    public VictoryCondition()
    {
        Description = "Placeholder victory condition!";
    }

    public VictoryCondition(string description)
    {
        Description = description;
    }

    public abstract void CheckConditionMet();

    public abstract void BeginListening();

    public abstract void StopListening();

    public abstract string Progress();
}