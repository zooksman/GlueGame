using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{


    public GameObject[] glue;
    public GlueScript[] glueS;
    int currentGlue;

    public int startingHealth = 100;
    public int currentHealth;

    bool active;
    float cooldown;
    public const float BASE_COOLDOWN = 0.5f;
    bool leftClicking; // continually remains true until click is released

    Rigidbody rb;
    const float VELOCITY_MODIFIER = 50f;
    public float horizontalSpeed = 1.8f;
    public float verticalSpeed = 1.8f;
    float directionX;
    float directionY;

    float glueBuildup;
    public const float GLUE_RATE_INCREASE = 0.01f;
    public const float MINIMUM_GLUE_BUILDUP = 0.2f;
    public const float MAXIMUM_GLUE_BUILDUP = 1f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        directionX = 0;
        directionY = 0;
        active = true;
        for (int i = 0; i < glue.Length; i++)
            glueS[i] = glue[i].GetComponent<GlueScript>();
        glueBuildup = MINIMUM_GLUE_BUILDUP;
        currentGlue = 0;
    }

    // Update is called once per frame
    void Update()
    {
        CheckMouse();
        FaceDirection();
    }

    private void CheckMouse()
    {	
    	if (Input.GetMouseButtonDown(0)) 
    	{
    		leftClicking = true;
    	}
    	
        if (leftClicking)
        {
            BuildupGlue();
            glue[currentGlue].transform.position = GetComponent<Rigidbody>().transform.position + GetComponent<Rigidbody>().transform.forward;  
        }
        
        if (Input.GetMouseButtonUp(0))
        {
        	leftClicking = false;
        	glue[currentGlue].transform.rotation = GetComponent<Rigidbody>().transform.rotation;
            ShootGlue();
        }
    }
    
    private void FaceDirection()
    {
        directionY += horizontalSpeed * Input.GetAxis("Mouse X");
        directionX += verticalSpeed * Input.GetAxis("Mouse Y");
        transform.rotation = Quaternion.Euler(-directionX, directionY, 0);
    }

    private void ShootGlue()
    {
        glueS[currentGlue].ShootSelf(transform.forward);
        PropelBackward(-transform.forward * VELOCITY_MODIFIER);
        ReadyNewGlue();
    }

    private void PropelBackward(Vector3 direction)
    {
		rb.AddForce(direction);
    }

    private void BuildupGlue()
    {
        if (glueBuildup < MAXIMUM_GLUE_BUILDUP) {
            glueBuildup += GLUE_RATE_INCREASE;
        }
        glueS[currentGlue].SetSize(glueBuildup);
    }

    private void ReadyNewGlue()
    {
        if (currentGlue < glue.Length - 1)
            currentGlue++;
        else
            currentGlue = 0;
        glueBuildup = MINIMUM_GLUE_BUILDUP;
    }

    private void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.CompareTag("asteroid"))
        {
            currentHealth = currentHealth - 10;
            StartCoroutine("WaitandCheck");
        }
    }

    IEnumerator WaitAndCheck()
    {
        yield return new WaitForSeconds(1.0f);
        if (currentHealth == 0)
        {
            GameOver();
        }
    }

    void GameOver()
    {
    }

}