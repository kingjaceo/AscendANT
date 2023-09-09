public class CurrentVictoryCondition
{
    public static CurrentVictoryCondition Instance;

    public string Reward;

    private VictoryCondition _victoryCondition;

    public CurrentVictoryCondition()
    {
        Instance = this;
    }

    public void SetCondition(VictoryCondition victoryCondition)
    {
        // _victoryCondition?.StopListening();

        _victoryCondition = victoryCondition;
        Reward = victoryCondition.Reward;
        _victoryCondition.BeginListening();
    }

    public string Progress()
    {
        if (_victoryCondition != null)
        {
            return _victoryCondition.Progress();
        }

        return "Current Victory Condition is Null";
    }
}