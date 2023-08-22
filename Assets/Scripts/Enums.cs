using System;

[Serializable]
public enum AntState
{
    Searching,
    Resting,
    Reporting,
    Fighting,
    Digging,
    Tending,
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
    Pheromone,
    Scout,
    Report,
    Retrieve,
    TendColony,
    Attack
}