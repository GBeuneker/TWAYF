using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldBehaviour : MonoBehaviour
{
    public float speed = 1.0f;
    private float zRotation;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        TurnClockwise(speed);
    }

    void TurnClockwise(float angle)
    {
        Vector3 eulerAngles = transform.eulerAngles;
        zRotation -= angle;
        eulerAngles.z = zRotation;

        transform.eulerAngles = eulerAngles;
    }
}
