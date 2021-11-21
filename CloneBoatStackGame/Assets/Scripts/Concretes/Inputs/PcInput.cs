using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PcInput : IPlayerInput,IMouseInput
{
    public float Horizontal=> Input.GetAxis("Mouse X");

    public bool control => Input.GetMouseButton(0);
}