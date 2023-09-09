using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SocialPlatforms.GameCenter;
using UnityEngine.UIElements;

public class WorldManager : MonoBehaviour
{
    public static WorldManager Instance;
    [SerializeField] private GameObject _worldParent;
    [SerializeField] private GameObject _worldPrefab;
    public World World;
    private Vector3 _scale;
    private WorldState _state;

    void Awake()
    {
        Instance = this;
        AllPheromones Pheromones = new AllPheromones();
        _scale = new Vector3(50f, 2f, 50f);
        _state = WorldState.None;
    }
    
    private void ChangeState(WorldState state)
    {
        _state = state;

        switch (state)
        {
            case WorldState.Creating:
                break;
            case WorldState.PlayableWorld:
                StartCoroutine(WorldAlive());
                break;
            case WorldState.DestroyingWorld:
                StopCoroutine(WorldAlive());
                break;
            default:
                break;
        }
    }

    public void ConnectToGUI()
    {
        World.MakePlayable();
    }

    public void CreateDemoWorld()
    {
        ChangeState(WorldState.Creating);

        CreateWorld();
        CenterCameraOnColony();

        ChangeState(WorldState.DemoWorld);
    }

    public void CreatePlayableWorld()
    {
        ChangeState(WorldState.Creating);
        
        CreateWorld();
        CenterCameraOnColony();

        ChangeState(WorldState.PlayableWorld);
    }

    public void CreateWorld()
    {
        DestroyWorld();
        
        Debug.Log("WORLD: Creating new world ... ");

        World = Instantiate(_worldPrefab).GetComponent<World>();
        World.gameObject.name = "CurrentWorld";
        World.transform.parent = _worldParent.transform;
        World.Create(_scale);
    }

    public void DestroyWorld()
    {
        ChangeState(WorldState.DestroyingWorld);

        if (World != null)
        {
            Debug.Log("WORLD: Destroying the old world ... ");
            Destroy(World.gameObject);
        }
        
        ChangeState(WorldState.None);
    }

    private IEnumerator WorldAlive()
    {
        while (true)
        {
            // every 1 minute a new food resource will appear
            yield return new WaitForSeconds(60);

            World.SpawnFood();
        }
    }

    private void CenterCameraOnColony()
    {
        CameraController.Instance.transform.position = World.Colony.transform.position + new Vector3(0, CameraController.Instance.transform.position.y, 0);
    }

    private enum WorldState
    {
        None,
        Creating,
        DemoWorld,
        PlayableWorld,
        DestroyingWorld,
    }
}