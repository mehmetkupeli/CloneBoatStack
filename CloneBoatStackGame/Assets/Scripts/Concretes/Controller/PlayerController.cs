using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    IPlayerInput _input;
    IMouseInput _control;

    public static PlayerController Current;


    public float limitX;
    public float runningSpeed;
    public float xSpeed;
    private float _currentRunningSpeed;
    public GameObject ridingCubePrefab;
    public List<RidingCube> cubes;
    private void Awake()
    {
        _input= new PcInput();
        _control = new PcInput();

    }
    private void Start()
    {
        Current = this;
        _currentRunningSpeed = runningSpeed;
    }
    private void Update()
    {
        float newX = 0;
        float touchXDelta=0;

        if (_control.control)
        {
            touchXDelta = _input.Horizontal;
        }

        newX = transform.position.x + xSpeed * touchXDelta * Time.deltaTime;
        newX = Mathf.Clamp(newX,-limitX,limitX);
        Vector3 newPosition = new Vector3(newX,transform.position.y,transform.position.z+_currentRunningSpeed*Time.deltaTime);
        transform.position = newPosition;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag=="AddCube")
        {
            IncrementCubeVolume(0.1f);
            Destroy(other.gameObject);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag=="Trap")
        {
            IncrementCubeVolume(-Time.fixedDeltaTime);
        }
    }

    public void IncrementCubeVolume(float value)
    {
        if (cubes.Count==0)
        {
            if (value>0)
            {
                CreateCube(value);
            }
            else
            {
                //GameOver
            }
        }
        else
        {
            cubes[cubes.Count - 1].IncrementCubeVolume(value);//lastCube update
        }
    }

    public void CreateCube(float value)
    {
        RidingCube createdCube = Instantiate(ridingCubePrefab,transform).GetComponent<RidingCube>();
        cubes.Add(createdCube);
        createdCube.IncrementCubeVolume(value);
    }

    public void DestroyCube(RidingCube cube)
    {
        cubes.Remove(cube);
        Destroy(cube.gameObject);
    }
}
