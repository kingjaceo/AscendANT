using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour
{
    [SerializeField] private GameObject _colonyPrefab;
    private Colony _colony;
    public Colony Colony => _colony;

    private Transform _transform;
    private Vector3 _scale;
    private GameObject _ground;
    [SerializeField] private Material _groundMaterial;
    private GameObject _posXWall;
    private GameObject _negXWall;
    private GameObject _posZWall;
    private GameObject _negZWall;

    [SerializeField] private GameObject _curioisityPrefab;
    [SerializeField] private GameObject _foodPrefab;

    void Awake()
    {

    }

    public void Create(Vector3 scale)
    {
        _transform = transform;
        _transform.position = new Vector3(0, 0, 0);

        _scale = scale;

        CreateGround();
        
        CreateWalls();

        CreateColony();

        CreateLocations();
    }
    
    private void CreateGround()
    {
        _ground = GameObject.CreatePrimitive(PrimitiveType.Cube);
        _ground.transform.parent = transform;
        _ground.GetComponent<Renderer>().material = _groundMaterial;
        _ground.transform.position = new Vector3(0, 0, 0);
        _ground.transform.localScale = new Vector3(_scale.x, 1, _scale.z);

        _ground.name = "Ground";
    }
    
    private void CreateWalls()
    {
        var xShift = _scale.x / 2;
        var yShift = _scale.y / 2; 
        var zShift = _scale.z / 2;

        var xWallScale = new Vector3(1, _scale.y, _scale.z);
        var zWallScale = new Vector3(_scale.x + 1, _scale.y, 1);

        _posXWall = GameObject.CreatePrimitive(PrimitiveType.Cube);
        _posXWall.transform.parent = transform;
        _posXWall.transform.localScale = xWallScale;
        _posXWall.transform.position = new Vector3(_transform.position.x + xShift, _transform.position.y + yShift, _transform.position.z);
        _posXWall.name = "Boundary";

        _negXWall = GameObject.CreatePrimitive(PrimitiveType.Cube);
        _negXWall.transform.parent = transform;
        _negXWall.transform.localScale = xWallScale;
        _negXWall.transform.position = new Vector3(_transform.position.x - xShift, _transform.position.y + yShift, _transform.position.z);
        _negXWall.name = "Boundary";

        _posZWall = GameObject.CreatePrimitive(PrimitiveType.Cube);
        _posZWall.transform.parent = transform;
        _posZWall.transform.localScale = zWallScale;
        _posZWall.transform.position = new Vector3(_transform.position.x, _transform.position.y + yShift, _transform.position.z + zShift);
        _posZWall.name = "Boundary";

        _negZWall = GameObject.CreatePrimitive(PrimitiveType.Cube);
        _negZWall.transform.parent = transform;
        _negZWall.transform.localScale = zWallScale;
        _negZWall.transform.position = new Vector3(_transform.position.x, _transform.position.y + yShift, _transform.position.z - zShift);
        _negZWall.name = "Boundary";
    }

    private void CreateColony()
    {
        _colony = Instantiate(_colonyPrefab).GetComponent<Colony>();
        _colony.Initialize(RandomLocationInWorld());
        _colony.Transform.parent = transform;
        _colony.name = "Colony";

        CameraController.Instance.transform.position = _colony.transform.position + new Vector3(0, 10, 0);
    }

    private void CreateLocations()
    {
        GameObject curiosity = Instantiate(_curioisityPrefab);
        curiosity.transform.position = _colony.RandomLocationAroundColony(5f, 10f);
        curiosity.transform.parent = transform;
        curiosity.name = "Curiosity";
        GameObject food = Instantiate(_foodPrefab);
        food.transform.position = _colony.RandomLocationAroundColony(3f, 5f);
        food.transform.parent = transform;
        food.name = "Food";
    }

    private Vector3 RandomLocationInWorld()
    {
        var buffer = 10f;
        var x = _transform.position.x + Random.Range(-_scale.x / 2 + buffer, _scale.x / 2 - buffer);
        var y = _transform.position.y + _scale.y / 4;
        var z = _transform.position.z + Random.Range(-_scale.z / 2 + buffer, _scale.z / 2 - buffer);

        return new Vector3(x, y, z);
    }

    public void MakePlayable()
    {
        _colony.ConnectCastesToGUI();
    }

    public void OnDestroy()
    {
        Debug.Log("WORLD: World Destroyed!");
    }

    public void SpawnFood()
    {
        Vector3 location = RandomLocationInWorld();

        GameObject food = Instantiate(_foodPrefab);
        food.transform.position = location;
        food.transform.parent = transform;
        food.name = "Food";

        Debug.Log("WORLD: Created new food at " + location);
    }
}
