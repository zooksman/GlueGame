using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipPieceScript : MonoBehaviour
{
    bool glued;
    Vector3 attatchedPosition;
    float positionDifference;
    float MAX_SNAP_DISTANCE = 7f;
    Rigidbody rb;
    const float BREAKING_MAX_VELOCITY = 15f;
    const float ANGULAR_VELOCITY_MODIFIER = 6f;
    const float SPEED_DEGREDATION = 0.997f;

    Transform savedParent;

    public bool inPlace;

    // Start is called before the first frame update
    void Start()
    {
        savedParent = transform.parent;
        inPlace = true;
        attatchedPosition = transform.position; // All pieces need to start in the first frame assembled or else their snapping positions will be messed up
        rb = GetComponent<Rigidbody>();
        
        rb.isKinematic = true;
        rb.velocity = new Vector3(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (!inPlace)
        {
            rb.velocity = new Vector3(rb.velocity.x * SPEED_DEGREDATION, rb.velocity.y * SPEED_DEGREDATION, rb.velocity.z * SPEED_DEGREDATION);
            TestAttatch();
        }
    }

    public void Detatch()
    {
        glued = false;
        rb.isKinematic = false;
        rb.velocity = new Vector3(Random.Range(-BREAKING_MAX_VELOCITY, BREAKING_MAX_VELOCITY), Random.Range(-BREAKING_MAX_VELOCITY, BREAKING_MAX_VELOCITY), Random.Range(-BREAKING_MAX_VELOCITY, BREAKING_MAX_VELOCITY));
        rb.angularVelocity = new Vector3(Random.value * ANGULAR_VELOCITY_MODIFIER, Random.value * ANGULAR_VELOCITY_MODIFIER, Random.value * ANGULAR_VELOCITY_MODIFIER);
        inPlace = false;
    }

    public void Attatch()
    {
        GetComponent<Rigidbody>().transform.parent = savedParent;
        rb.isKinematic = true;
        rb.velocity = new Vector3(0, 0, 0);
        transform.position = attatchedPosition;
        transform.localRotation = Quaternion.Euler(0,0,0);
        inPlace = true;
    }

    public void Glue()
    {
        glued = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("asteroid"))
            Detatch();        	
    }

    private void TestAttatch()
    {
        if (glued && InPosition())
        {
            Attatch();
            glued = false;
        }
    }

    private bool InPosition()
    {
        positionDifference = Mathf.Pow(Mathf.Pow(attatchedPosition.x - transform.position.x, 2f) + Mathf.Pow(attatchedPosition.y - transform.position.y, 2f) + Mathf.Pow(attatchedPosition.z - transform.position.z, 2f), 0.5f);
        print("position difference: " + positionDifference);
        if (positionDifference < MAX_SNAP_DISTANCE)
            return true;
        else
            return false;
    }

}
