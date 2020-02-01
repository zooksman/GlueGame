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
    Animator anim;

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

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckMouse();
        FaceDirection();
    }

    private void CheckMouse()
    {
        if (leftClicking == true)
        {
            BuildupGlue();
            glue[currentGlue].transform.position = GetComponent<Rigidbody>().transform.position + GetComponent<Rigidbody>().transform.forward;  
        }
        if (Input.GetMouseButtonDown(0))
        {
            leftClicking = true;
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
        glueS[currentGlue].ShootSelf(transform.localRotation * Vector3.forward);
        PropelBackward(transform.localRotation * (-Vector3.forward * VELOCITY_MODIFIER));
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
        if (glue[currentGlue] != null)
            currentGlue++;
        else
            currentGlue = 0;
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
        anim.SetTrigger("GameOver");
    }

}