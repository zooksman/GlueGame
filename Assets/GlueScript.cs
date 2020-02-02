using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlueScript : MonoBehaviour
{
    Rigidbody rb;
    const float VELOCITY_MODIFIER = 10000f;
    float size;
    bool beingHeld;
    public GameObject hit;
    bool glued;
    const float SPEED_DEGREDATION = 0.999f;
    Vector3 savedForce;

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
        transform.localRotation = Quaternion.Euler(0, 0, 0);
        //if (glued)
        //rb.velocity = new Vector3(rb.velocity.x * SPEED_DEGREDATION, rb.velocity.y * SPEED_DEGREDATION, rb.velocity.z * SPEED_DEGREDATION);
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
        savedForce = direction * VELOCITY_MODIFIER;
    }

    public void OnTriggerEnter(Collider c) {
    	hit = c.gameObject;
        if (hit.CompareTag("shippiece") && hit.GetComponent<ShipPieceScript>().inPlace == false)
        {
            print("before: " + rb.velocity.x + "    " + rb.velocity.y + "    " + rb.velocity.z);
            glued = true;
            rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, rb.velocity.z);
            hit.GetComponent<Rigidbody>().isKinematic = true;
            hit.GetComponent<Rigidbody>().transform.parent = GetComponent<Rigidbody>().transform;
            hit.GetComponent<ShipPieceScript>().Glue();
            print("on stick: " + rb.velocity.x + "    " + rb.velocity.y + "    " + rb.velocity.z);
            StartCoroutine("ResetVelocity");
        }
    }

    IEnumerator ResetVelocity()
    {
        print("RESETTING!!!");
        yield return new WaitForSeconds(0.1f);
        rb.velocity = new Vector3(0,0,0);
        rb.AddForce(savedForce);
        print("on reset: " + rb.velocity.x + "    " + rb.velocity.y + "    " + rb.velocity.z);
    }

    IEnumerator TestingVelocity()
    {
        yield return new WaitForSeconds(0.01f);
        print("afterwards: " + rb.velocity.x + "    " + rb.velocity.y + "    " + rb.velocity.z);
    }

}