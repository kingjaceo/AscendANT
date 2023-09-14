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
    Harvest,
    TendEggs,
    Queen,
    ResupplySelf
}

[Flags]
public enum ResourceType
{
    // each represents a bit field, use bitwise logical operators | or & to conbine or intersect choices
    None        = 0b_0000_0000,
    Curiosity   = 0b_0000_0001, // 1
    Food        = 0b_0000_0010, // 2
    Water       = 0b_0000_0100, // 4
    Eggs        = 0b_0000_1000, // 8
    Aphids      = 0b_0001_0000, // 16
}

[Flags]
public enum LocationType
{
    None        = 0b_0000_0000, // 0
    Colony      = 0b_0000_0001, // 1
    Queen       = 0b_0000_0010, // 2
    Resource    = 0b_0000_0100, // 4
    Wall        = 0b_0000_1000, // 8
}