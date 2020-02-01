using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipPieceScript : MonoBehaviour
{
    bool glued;
    Vector3 attatchedPosition;
    float positionDifference;
    public float MAX_SNAP_DISTANCE = 2f;
    Rigidbody rb;
    public const float BREAKING_MAX_VELOCITY = 4f;
    public const float ANGULAR_VELOCITY_MODIFIER = 4f;

    // Start is called before the first frame update
    void Start()
    {
        attatchedPosition = transform.position; // All pieces need to start in the first frame assembled or else their snapping positions will be messed up
        rb = GetComponent<Rigidbody>();
        
        rb.isKinematic = true;
        rb.velocity = new Vector3(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Detatch()
    {
        glued = false;
        rb.isKinematic = false;
        rb.velocity = new Vector3(Random.Range(-BREAKING_MAX_VELOCITY, BREAKING_MAX_VELOCITY), Random.Range(-BREAKING_MAX_VELOCITY, BREAKING_MAX_VELOCITY), Random.Range(-BREAKING_MAX_VELOCITY, BREAKING_MAX_VELOCITY));
        rb.angularVelocity = new Vector3(Random.value * ANGULAR_VELOCITY_MODIFIER, Random.value * ANGULAR_VELOCITY_MODIFIER, Random.value * ANGULAR_VELOCITY_MODIFIER);
    }

    public void Attatch()
    {
        rb.isKinematic = true;
        rb.velocity = new Vector3(0, 0, 0);
        transform.position = attatchedPosition;
        
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
            Glue();
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
