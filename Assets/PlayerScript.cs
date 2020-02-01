using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{


    public GameObject[] glue;
    GlueScript[] glueS;

    bool active;
    float cooldown;
    public const float BASE_COOLDOWN = 0.5f;
    bool rightClicking; // continually remains true until click is released
    
    public float horizontalSpeed = 1.0F;
    public float verticalSpeed = 1.0F;

    float glueBuildup;
    public const float GLUE_RATE_INCREASE = 0.01f;
    public const float MINIMUM_GLUE_BUILDUP = 0.2f;
    public const float MAXIMUM_GLUE_BUILDUP = 1f;

    // Start is called before the first frame update
    void Start()
    {
        active = true;
        for (int i = 0; i < glue.Length; i++)
            glueS[i] = glue[i].GetComponent<GlueScript>();
        glueBuildup = MINIMUM_GLUE_BUILDUP;
    }

    // Update is called once per frame
    void Update()
    {
    	float h = horizontalSpeed * Input.GetAxis("Mouse X");
        float v = verticalSpeed * Input.GetAxis("Mouse Y");
        transform.Rotate(v, h, 0);
        
        if (rightClicking == true)
        {
            BuildupGlue();
            glueS.SetSize(glueBuildup);
        }
        if (Input.GetMouseButtonDown(0))
        {
            rightClicking = true;
        }
        if (Input.GetMouseButtonUp(0))
        {
            ShootGlue();
            rightClicking = false;
        }
    }
    
    private void ShootGlue()
    {
        glueS.ShootSelf(transform.localRotation * Vector3.forward, transform.position + transform.forward);
        PropelBackward(transform.localRotation * Vector3.backward);
    }

    private void PropelBackward(Vector3 direction)
    {
		GetComponent<Rigidbody>().AddForce(direction);
    }

    private void BuildupGlue()
    {
        if (glueBuildup < MAXIMUM_GLUE_BUILDUP) {
            glueBuildup += GLUE_RATE_INCREASE;
        }
    }

    private void ReadyNewGlue()
    {
        
    }

}