using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RidingCube : MonoBehaviour
{
    private bool _filled;
    private float _value;

    public void IncrementCubeVolume(float value)
    {
        _value += value;
        if (_value>1)
        {
            float leftValue = _value - 1;
            int cubeCount = PlayerController.Current.cubes.Count;
            transform.localPosition = new Vector3(transform.localPosition.x, -0.5f*(cubeCount-1) -0.25f,transform.localPosition.z);
            transform.localScale = new Vector3(0.5f,transform.localScale.y, 0.5f);
            PlayerController.Current.CreateCube(leftValue);
        }
        else if (_value < 0)
        {
            PlayerController.Current.DestroyCube(this);
        }
        else
        {
            int cubeCount = PlayerController.Current.cubes.Count;
            transform.localPosition = new Vector3(transform.localPosition.x, -0.5f * (cubeCount - 1) - 0.25f * _value, transform.localPosition.z);
            transform.localScale = new Vector3(0.5f * _value, transform.localScale.y, 0.5f*_value);
        }
    }
}
