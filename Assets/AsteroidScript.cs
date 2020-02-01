using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidScript : MonoBehaviour
{
    Rigidbody rb;
    public const float VELOCITY_MODIFIER = 1f;
    public const float ANGULAR_VELOCITY_MODIFIER = 1f;
    Vector3 preparedVector;
    public const float CLOSEST_DISTANCE_VALUE = 50f;
    public const float FURTHEST_DISTANCE_VALUE = 90f;

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
        rb.velocity = new Vector3((Random.value + 1) * VELOCITY_MODIFIER, (Random.value + 1) * VELOCITY_MODIFIER, (Random.value + 1) * VELOCITY_MODIFIER);
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
