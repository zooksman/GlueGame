﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidScript : MonoBehaviour
{
    Rigidbody rb;
    public const float VELOCITY_MODIFIER = 4f;
    public const float ANGULAR_VELOCITY_MODIFIER = 1f;
    Vector3 preparedVector;
    public const float CLOSEST_DISTANCE_VALUE = 50f;
    public const float FURTHEST_DISTANCE_VALUE = 90f;
    public Vector3 SHIP_COORDS = new Vector3(-4.018551f,-0.347738f,2.30722f);

    float negMultX;
    float negMultY;
    float negMultZ;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        StartingPosition();
        StartingVelocity();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Spawn()
    {
        StartingPosition();
        StartingVelocity();
    }

    private void StartingPosition() // has range of 90 to 30 OR -90 to 30
    {
    	Vector3 coords = GameObject.FindGameObjectWithTag("shippiece").GetComponent<ShipPieceScript>().GetPosition();
        if(Random.value > 0.5f)
            preparedVector = new Vector3(Random.Range(CLOSEST_DISTANCE_VALUE, FURTHEST_DISTANCE_VALUE), preparedVector.y, preparedVector.z);
        else
            preparedVector = new Vector3(-Random.Range(CLOSEST_DISTANCE_VALUE, FURTHEST_DISTANCE_VALUE), preparedVector.y, preparedVector.z);
        if (Random.value > 0.5f)
            preparedVector = new Vector3(preparedVector.x, Random.Range(CLOSEST_DISTANCE_VALUE, FURTHEST_DISTANCE_VALUE), preparedVector.z);
        else
            preparedVector = new Vector3(preparedVector.x, -Random.Range(CLOSEST_DISTANCE_VALUE, FURTHEST_DISTANCE_VALUE), preparedVector.z);
        if (Random.value > 0.5f)
            preparedVector = new Vector3(preparedVector.x, preparedVector.y, Random.Range(CLOSEST_DISTANCE_VALUE, FURTHEST_DISTANCE_VALUE));
        else
            preparedVector = new Vector3(preparedVector.x, preparedVector.y, -Random.Range(CLOSEST_DISTANCE_VALUE, FURTHEST_DISTANCE_VALUE));
        transform.position = preparedVector;
    }

    private void StartingVelocity()
    {
        if (Random.value > 0.5f)
            negMultX = 1f;
        else
            negMultX = -1f;
        if (Random.value > 0.5f)
            negMultY = 1f;
        else
            negMultY = -1f;
        if (Random.value > 0.5f)
            negMultZ = 1f;
        else
            negMultZ = -1f;
        rb.velocity = new Vector3((Random.value + 1) * VELOCITY_MODIFIER * negMultX, (Random.value + 1) * VELOCITY_MODIFIER * negMultY, (Random.value + 1) * VELOCITY_MODIFIER * negMultZ);
        rb.angularVelocity = new Vector3(Random.value * ANGULAR_VELOCITY_MODIFIER, Random.value * ANGULAR_VELOCITY_MODIFIER, Random.value * ANGULAR_VELOCITY_MODIFIER);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("boundry"))
        {
            StartingPosition();
            StartingVelocity();
        }
    }

}
