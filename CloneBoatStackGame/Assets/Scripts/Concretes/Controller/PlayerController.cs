using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    IPlayerInput _input;
    IMouseInput _control;

    public static PlayerController Current;


    public float limitX;
    public float xSpeed;
    public float runningSpeed;
    private float _currentRunningSpeed;
    public GameObject ridingCubePrefab;
    public List<RidingCube> cubes;

    //BridgeSpawner
    private bool _spawningBridge;
    public GameObject bridgePiecePrefab;
    private BridgeSpawner _bridgeSpawner;
    private float _creatingBridgeTimer;
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

        if (_spawningBridge)
        {
            _creatingBridgeTimer -= Time.deltaTime;
            if (_creatingBridgeTimer<0)
            {
                _creatingBridgeTimer = 0.01f;
                IncrementCubeVolume(-0.01f);
                GameObject createdBridgePiece=Instantiate(bridgePiecePrefab);
                Vector3 direction = _bridgeSpawner.endReference.transform.position - _bridgeSpawner.startReference.transform.position;
                float distance = direction.magnitude;
                direction = direction.normalized;
                createdBridgePiece.transform.forward = direction;
                float characterDistance = transform.position.z - _bridgeSpawner.startReference.transform.position.z;
                characterDistance = Mathf.Clamp(characterDistance, 0, distance);
                Vector3 newPiecePosition = _bridgeSpawner.startReference.transform.position + direction * characterDistance;
                newPiecePosition.x = transform.position.x;
                createdBridgePiece.transform.position = newPiecePosition;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag=="AddCube")
        {
            IncrementCubeVolume(0.1f);
            Destroy(other.gameObject);
        }else if (other.tag == "SpawnBridge")
        {
            StartSpawningBridge(other.transform.parent.GetComponent<BridgeSpawner>());
        }
        else if (other.tag == "StopSpawnBridge")
        {
            StopSpawningBridge();
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

    //Bridge Spawner Trap Functions
    public void StartSpawningBridge(BridgeSpawner spawner)
    {
        _bridgeSpawner = spawner;
        _spawningBridge = true;
    }
    public void StopSpawningBridge()
    {
        _spawningBridge = false;
    }
}
