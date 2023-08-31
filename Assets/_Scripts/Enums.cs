using System;

[Serializable]
public enum AntState
{
    Searching,
    Resting,
    Reporting,
    Harvesting,
    Fighting,
    Digging,
    TendingColony,
    Idle,
    Dead,
    Collided,
}

[Serializable]
public enum PheromoneState
{
    InProgress,
    Complete
}

[Serializable]
public enum PheromoneName
{
    Scout,
    Collect,
    TendEggs,
    Queen
}

[Flags]
public enum ResourceType
{
    // each represents a bit field, use bitwise logical operators | or & to conbine or intersect choices
    Resource    = 0b_0000_0000, // 0
    Curiosity   = 0b_0000_0001, // 1
    Food        = 0b_0000_0010, // 2
    Water       = 0b_0000_0100, // 4
    Eggs        = 0b_0000_1000, // 8
    Aphids      = 0b_0001_0000, // 16
}

public enum TargetType
{
    Colony,
    Ant,
    Queen,
    Resource
}