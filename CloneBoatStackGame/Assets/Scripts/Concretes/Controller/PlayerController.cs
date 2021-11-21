using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    IPlayerInput _input;
    IMouseInput _control;
    public float limitX;
    public float runningSpeed;
    public float xSpeed;
    private float _currentRunningSpeed;
    private void Awake()
    {
        _input= new PcInput();
        _control = new PcInput();

    }
    private void Start()
    {
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

}
