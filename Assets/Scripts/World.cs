using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour
{
    public Colony mainColony;
    public float sideLength = 10;
    public GameObject invisibleWall;
    public GameObject posXWall;
    public GameObject negXWall;
    public GameObject posZWall;
    public GameObject negZWall;

    // Start is called before the first frame update
    void Start()
    {
        posXWall = Instantiate(invisibleWall);
        posXWall.transform.position = new Vector3(this.sideLength / 2, 0, this.transform.position[2]);
        posXWall.transform.Rotate(0, 90, 0);

        negXWall = Instantiate(invisibleWall);
        negXWall.transform.position = new Vector3(-this.sideLength / 2, 0, this.transform.position[2]);
        negXWall.transform.Rotate(0, 90, 0);

        posZWall = Instantiate(invisibleWall);
        posZWall.transform.position = new Vector3(this.transform.position[0], 0, this.sideLength / 2);

        negZWall = Instantiate(invisibleWall);
        negZWall.transform.position = new Vector3(this.transform.position[0], 0, -this.sideLength / 2);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
