public class CurrentVictoryCondition
{
    public static CurrentVictoryCondition Instance;

    private VictoryCondition _victoryCondition;

    public CurrentVictoryCondition()
    {
        Instance = this;
    }

    public void SetCondition(VictoryCondition victoryCondition)
    {
        // _victoryCondition?.StopListening();

        _victoryCondition = victoryCondition;
        _victoryCondition.BeginListening();
    }


}