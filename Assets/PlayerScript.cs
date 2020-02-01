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
    bool rightClicking; // continually remains true until click is released
    
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
        directionX = 0;
        directionY = 0;
        active = true;
        for (int i = 0; i < glue.Length; i++)
            glueS[i] = glue[i].GetComponent<GlueScript>();
        glueBuildup = MINIMUM_GLUE_BUILDUP;
    }

    // Update is called once per frame
    void Update()
    {
        CheckMouse();
        FaceDirection();
    }

    private void CheckMouse()
    {
        if (rightClicking == true)
        {
            BuildupGlue();
            glueS[currentGlue].SetSize(glueBuildup);
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
    
    private void FaceDirection()
    {
        directionY += horizontalSpeed * Input.GetAxis("Mouse X");
        directionX += verticalSpeed * Input.GetAxis("Mouse Y");
        transform.rotation = Quaternion.Euler(-directionX, directionY, 0);
    }

    private void ShootGlue()
    {
        glueS[currentGlue].ShootSelf(transform.localRotation * Vector3.forward, transform.position + transform.forward);
        PropelBackward(transform.localRotation * -Vector3.forward);
        ReadyNewGlue();
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
    }

}