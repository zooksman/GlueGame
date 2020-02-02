using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlueScript : MonoBehaviour
{
    Rigidbody rb;
    public const float VELOCITY_MODIFIER = 1000f;
    float size;
    bool beingHeld;
    public GameObject hit;
    bool glued;
    const float SPEED_DEGREDATION = 0.999f;
    Vector3 savedVelocity;

    // Start is called before the first frame update
    void Start()
    {
        glued = false;
        beingHeld = false;
        transform.position = new Vector3(300f,300f,300f);
        rb = GetComponent<Rigidbody>();
        size = 0.2f; // same as minimum constant on PlayerScript
    }

    // Update is called once per frame
    void Update()
    {
        if (glued)
            rb.velocity = new Vector3(rb.velocity.x * SPEED_DEGREDATION, rb.velocity.y * SPEED_DEGREDATION, rb.velocity.z * SPEED_DEGREDATION);
    }

    public void SetSize(float s)
    {
        size = s;
        transform.localScale = new Vector3(size, size, size);
    }

    public void ShootSelf(Vector3 direction)
    {
        glued = false;
        rb.velocity = new Vector3(0,0,0);
        rb.AddForce(direction * VELOCITY_MODIFIER);
        savedVelocity = rb.velocity;
    }

    public void OnTriggerEnter(Collider c) {
    	hit = c.gameObject;
        if (hit.CompareTag("shippiece") && hit.GetComponent<ShipPieceScript>().inPlace == false)
        {
            glued = true;
            rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, rb.velocity.z);
            hit.GetComponent<Rigidbody>().isKinematic = true;
            hit.GetComponent<Rigidbody>().transform.parent = GetComponent<Rigidbody>().transform;
            hit.GetComponent<ShipPieceScript>().Glue();
        }
        StartCoroutine("ResetVelocity");
    }

    IEnumerator ResetVelocity()
    {
        yield return new WaitForSeconds(0.1f);
        rb.velocity = savedVelocity;
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("shippiece"))
            savedVelocity = rb.velocity;
    }
}