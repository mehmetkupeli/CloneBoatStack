using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    IPlayerInput _input;
    IMouseInput _control;

    public static PlayerController Current;
    private float _scoreTimer=0;

    [Header("Player Move")]
    public float limitX;
    public float xSpeed;
    public float runningSpeed;
    private float _currentRunningSpeed;
    public GameObject ridingCubePrefab;
    public List<RidingCube> cubes;
    private bool _finished;

    [Header("Audio")]
    public AudioSource cubeAudioSource, triggerAudioSource;
    public AudioClip gatherAudioClip, dropAudioClip,coinAudioClip;
    private float _dropSoundTimer;

    [Header("BridgeSpawner")]
    public GameObject bridgePiecePrefab;
    private bool _spawningBridge;
    private BridgeSpawner _bridgeSpawner;
    private float _creatingBridgeTimer;

    [Header("Animator")]
    public Animator animator;
    private void Awake()
    {
        _input= new PcInput();
        _control = new PcInput();
    }
    private void Start()
    {
        Current = this;
       
    }
    private void Update()
    {
        if (LevelController.Current==null || !LevelController.Current.isGameActive)
        {
            return;
        }
        float newX = 0;
        float touchXDelta=0;

        if (_control.control)
        {
            touchXDelta = _input.Horizontal;
        }
        if (LevelController.Current.currentLevel==1)
        {
            _currentRunningSpeed = 8;
            xSpeed = 35;
        }

        newX = transform.position.x + xSpeed * touchXDelta * Time.deltaTime;
        newX = Mathf.Clamp(newX,-limitX,limitX);
        Vector3 newPosition = new Vector3(newX,transform.position.y,transform.position.z+_currentRunningSpeed*Time.deltaTime);
        transform.position = newPosition;

        if (_spawningBridge)
        {
            PlayDropSound();
            _creatingBridgeTimer -= Time.deltaTime;
            if (_creatingBridgeTimer<0)
            {
                _creatingBridgeTimer = 0.01f;
                IncrementCubeVolume(-0.01f);
                GameObject createdBridgePiece=Instantiate(bridgePiecePrefab,this.transform);
                createdBridgePiece.transform.SetParent(null);
                Vector3 direction = _bridgeSpawner.endReference.transform.position - _bridgeSpawner.startReference.transform.position;
                float distance = direction.magnitude;
                direction = direction.normalized;
                createdBridgePiece.transform.forward = direction;
                float characterDistance = transform.position.z - _bridgeSpawner.startReference.transform.position.z;
                characterDistance = Mathf.Clamp(characterDistance, 0, distance);
                Vector3 newPiecePosition = _bridgeSpawner.startReference.transform.position + direction * characterDistance;
                newPiecePosition.x = transform.position.x;
                createdBridgePiece.transform.position = newPiecePosition;
                if (_finished)
                {
                    _scoreTimer -= Time.deltaTime;
                    if (_scoreTimer<0)
                    {
                        _scoreTimer = 0.3f;
                        LevelController.Current.ChangeScore(1);
                    }
                }
            }
        }
    }

    public void ChangeSpeed(float value)
    {
        _currentRunningSpeed = value;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.tag);
        if (other.tag=="AddCube")
        {
            cubeAudioSource.PlayOneShot(gatherAudioClip,0.1f);
            IncrementCubeVolume(0.1f);
            Destroy(other.gameObject);
        }else if (other.tag == "SpawnBridge")
        {
            StartSpawningBridge(other.transform.parent.GetComponent<BridgeSpawner>());
        }
        else if (other.tag == "StopSpawnBridge")
        {
            StopSpawningBridge();
            if (_finished)
            {
                LevelController.Current.FinishGame();
            }
        }
        else if (other.tag == "Finish")
        {
            _finished = true;
            StartSpawningBridge(other.transform.parent.GetComponent<BridgeSpawner>());
        }else if (other.tag == "Coin")
        {
            triggerAudioSource.PlayOneShot(coinAudioClip,0.1f);
            other.tag = "Untagged";
            Destroy(other.gameObject);
        }
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (LevelController.Current.isGameActive)
        {
            if (other.tag == "Trap")
            {
                PlayDropSound();
                IncrementCubeVolume(-Time.fixedDeltaTime);
            }
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
                if (_finished)
                {
                    LevelController.Current.FinishGame();
                }
                else
                {
                    Die();
                }
            }
        }
        else
        {
            cubes[cubes.Count - 1].IncrementCubeVolume(value);//lastCube update
        }
    }
    public void Die()
    {
        animator.SetBool("isDead",true);
        gameObject.layer = 8;
        Camera.main.transform.SetParent(null);
        LevelController.Current.GameOver();
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

    public void PlayDropSound()
    {
        _dropSoundTimer -= Time.deltaTime;
        if (_dropSoundTimer<0)
        {
            _dropSoundTimer = 0.15f;
            cubeAudioSource.PlayOneShot(dropAudioClip,0.1f);
        }
    }

    
}
