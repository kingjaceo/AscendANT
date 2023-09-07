using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

public class WorldManager : MonoBehaviour
{
    public static WorldManager Instance;
    [SerializeField] private GameObject _worldParent;
    [SerializeField] private GameObject _worldPrefab;
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
        // switch (state)
        // {
        //     case GameState.MainMenu:
        //         CreateNewWorld(_scale);
        //         break;
        //     case GameState.InProgress:
        //         CreateNewWorld(_scale);
        //         break;
        //     default:
        //         break;
        // }
    }

    public void ConnectToGUI()
    {
        World.MakePlayable();
    }

    public void CreateNewWorld()
    {
        if (World != null)
        {
            Debug.Log("WORLD: Destroying world ... ");
            Destroy(World.gameObject);
        }
        
        Debug.Log("WORLD: Creating new world ... ");

        World = Instantiate(_worldPrefab).GetComponent<World>();
        World.gameObject.name = "CurrentWorld";
        World.transform.parent = _worldParent.transform;
        World.Create(_scale);
    }
}