
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobileInput : IPlayerInput,ITouchInput
{
    public float Horizontal=> Input.GetTouch(0).position.x / Screen.width;

    public bool control => Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved;
}