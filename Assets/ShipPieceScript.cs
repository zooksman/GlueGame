using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipPieceScript : MonoBehaviour
{
    bool glued;
    Vector3 attatchedPosition;
    float positionDifference;
    public float MAX_SNAP_DISTANCE = 50f;
    Rigidbody rb;
    const float BREAKING_MAX_VELOCITY = 4f;
    const float ANGULAR_VELOCITY_MODIFIER = 4f;
    const float SPEED_DEGREDATION = 0.997f;

    public bool inPlace;

    // Start is called before the first frame update
    void Start()
    {
        inPlace = true;
        attatchedPosition = transform.position; // All pieces need to start in the first frame assembled or else their snapping positions will be messed up
        rb = GetComponent<Rigidbody>();
        
        //MAKES COLLISIONS NOT WORK
        //rb.isKinematic = true;
        rb.velocity = new Vector3(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (!inPlace)
        {
            rb.velocity = new Vector3(rb.velocity.x * SPEED_DEGREDATION, rb.velocity.y * SPEED_DEGREDATION, rb.velocity.z * SPEED_DEGREDATION);
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
        print("attatching");
        rb.isKinematic = true;
        rb.velocity = new Vector3(0, 0, 0);
        transform.position = attatchedPosition;
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
        else if (collision.gameObject.CompareTag("shippiece"))
            TestAttatch();
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
        if (positionDifference < MAX_SNAP_DISTANCE)
            return true;
        else
            return false;
    }

}
