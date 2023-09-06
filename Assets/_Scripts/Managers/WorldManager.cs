using System;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

public class WorldManager : MonoBehaviour
{
    public static WorldManager Instance;
    public World World;
    private Vector3 _scale;

    void Awake()
    {
        Instance = this;
        GameManager.OnAfterStateChanged += GameManager_OnAfterStateChanged;
        AllPheromones Pheromones = new AllPheromones();
        _scale = new Vector3(50f, 2f, 50f);
    }
    
    private void GameManager_OnAfterStateChanged(GameState state)
    {
        switch (state)
        {
            case GameState.WorldGeneration:
                World.Create(_scale);
                break;
            default:
                break;
        }
    }
}