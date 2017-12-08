using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Starfighter : MonoBehaviour
{
    [SerializeField] float mainThrust = 100f;

    Rigidbody rigidBody;
    bool rotateOnLeft = true;
    bool rotateOnRight = false;

    // Use this for initialization
    void Start()
    {
        rigidBody = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        ChangePosition();
    }

    private void ChangePosition()
    {
        if (transform.position.x < -15f)
        {
            if (rotateOnLeft)
            {
                Rotate();
            }
        }
        else if (transform.position.x > 15f)
        {
            if (rotateOnRight)
            {
                Rotate();
            }
        }
        float thrustThisFrame = mainThrust * Time.deltaTime;
        rigidBody.AddRelativeForce(Vector3.left * thrustThisFrame);
    }

    private void Rotate()
    {
        rigidBody.freezeRotation = true;                // take manual control of rotation
        transform.Rotate(Vector3.up, 180f);
        rigidBody.freezeRotation = false;               // resume physics control of rotation
        rotateOnLeft = !rotateOnLeft;
        rotateOnRight = !rotateOnRight;
    }
}
