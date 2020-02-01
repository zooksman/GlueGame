using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlueScript : MonoBehaviour
{
    Rigidbody rb;
    public const float VELOCITY_MODIFIER = 30f;
    float size;
    bool beingHeld;
    public GameObject hit;

    // Start is called before the first frame update
    void Start()
    {
        beingHeld = false;
        transform.position = new Vector3(300f,300f,300f);
        rb = GetComponent<Rigidbody>();
        size = 0.2f; // same as minimum constant on PlayerScript
    }

    // Update is called once per frame
    void Update()
    {
		
    }

    public void SetSize(float s)
    {
        size = s;
        transform.localScale = new Vector3(size, size, size);
    }

    public void ShootSelf(Vector3 direction)
    {
        hit.GetComponent<Rigidbody>().isKinematic = false;
        rb.AddForce(direction, ForceMode.Impulse);
    }
    
    public void OnCollisionEnter(Collision c) {
    	hit = c.collider.gameObject;
    	if (hit.CompareTag("shippiece")) {
    		hit.GetComponent<Rigidbody>().isKinematic = true;
    		hit.GetComponent<Rigidbody>().transform.parent = GetComponent<Rigidbody>().transform;
    		hit.GetComponent<ShipPieceScript>().Glue();
    	}
    }

}