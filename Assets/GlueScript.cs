using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlueScript : MonoBehaviour
{
    Rigidbody rb;
    public const float VELOCITY_MODIFIER = 4f;
    float size;

    // Start is called before the first frame update
    void Start()
    {
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

    public void ShootSelf(Vector3 direction, Vector3 pos)
    {
        transform.position = pos;
        rb.velocity = direction * VELOCITY_MODIFIER;
    }
    
    public void OnCollisionEnter(Collision c) {
    	
    }

}