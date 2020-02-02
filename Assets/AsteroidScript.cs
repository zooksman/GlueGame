using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidScript : MonoBehaviour
{
    Rigidbody rb;
    public const float VELOCITY_MODIFIER = 15f;
    public const float ANGULAR_VELOCITY_MODIFIER = 1f;
    Vector3 preparedVector;
    public const float CLOSEST_DISTANCE_VALUE = 30f;
    public const float FURTHEST_DISTANCE_VALUE = 60f;

    float negMultX;
    float negMultY;
    float negMultZ;
    
    GameObject target = null;
    GameObject[] pieces;

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
    	pieces = GameObject.FindGameObjectsWithTag("shippiece");
    	int i;
    	for (i = Random.Range(0,pieces.Length); i < pieces.Length; i++ ) {
    		if (pieces[i].GetComponent<ShipPieceScript>().GetInPlace()) {
    			target = pieces[i];
    		}
    	}
    	if (target == null) {
    		target = GameObject.FindWithTag("shipbase");
    	}

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
    	Vector3 randomVelocity;
    	randomVelocity = new Vector3(Random.value, Random.value, Random.value);
        rb.velocity = ((target.transform.position - this.transform.position).normalized + randomVelocity) * VELOCITY_MODIFIER * 2*Random.value;
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
