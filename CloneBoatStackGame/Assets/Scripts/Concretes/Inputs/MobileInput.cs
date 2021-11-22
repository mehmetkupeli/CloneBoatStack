
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobileInput : IPlayerInput,ITouchInput
{
    public bool control => Input.touchCount > 0;
    public float lasTouchedX;
    public float touchXDelta;
    public float Horizontal
    {
        get
        {
            if (control)
            {
                if (Input.GetTouch(0).phase == TouchPhase.Began)
                {
                    lasTouchedX = Input.GetTouch(0).position.x;
                }
                else if (Input.GetTouch(0).phase == TouchPhase.Moved)
                {
                    touchXDelta = 5 * (Input.GetTouch(0).position.x - lasTouchedX) / Screen.width;
                    lasTouchedX = Input.GetTouch(0).position.x;
                }
            }
            return lasTouchedX;
        }



    }

}