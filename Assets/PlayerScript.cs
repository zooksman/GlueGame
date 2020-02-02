using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{


    public GameObject[] glue;
    public GlueScript[] glueS;
    int currentGlue;

    public int startingHealth = 100;
    public int currentHealth;
    public Text changeHealth;

    Animator anim;

    bool active;
    float cooldown;
    const float BASE_COOLDOWN = 0.5f;
    bool leftClicking; // continually remains true until click is released

    Rigidbody rb;
    const float VELOCITY_MODIFIER = 500f;
    const float HORIZONTAL_SPEED = 1.8f;
    const float VERTICAL_SPEED = 1.8f;
    float directionX;
    float directionY;
    const float BREAK_SPEED = 0.96f;
    bool spacePressed;

    float glueBuildup;
    public const float GLUE_RATE_INCREASE = 0.01f;
    public const float MINIMUM_GLUE_BUILDUP = 0.2f;
    public const float MAXIMUM_GLUE_BUILDUP = 1f;
    
    public const float GRABBER_RANGE = 300f;
    
    private RaycastHit hit;
    private float dist;
    private bool isHolding = false;
    private float holdDistance = 5.0f;
    private Vector3 newPos;
    private GameObject heldObject;
    private Camera camera;

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
        camera = (Camera)GameObject.FindObjectOfType(typeof(Camera));
    }

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckBreak();
        CheckMouse();
        FaceDirection();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void CheckBreak()
    {
        if (Input.GetKeyDown("space"))
            spacePressed = true;
        if (Input.GetKeyUp("space"))
            spacePressed = false;
        if(spacePressed)
           rb.velocity = new Vector3(rb.velocity.x * BREAK_SPEED, rb.velocity.y * BREAK_SPEED, rb.velocity.z * BREAK_SPEED);
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
                
         if (!isHolding)
            {
            if (Input.GetMouseButtonDown(1))
            {   

                if (Physics.Raycast(transform.position, transform.localRotation * Vector3.forward, out hit, GRABBER_RANGE))
                {
                    if (hit.collider.gameObject.CompareTag("shippiece") && !hit.collider.gameObject.GetComponent<ShipPieceScript>().inPlace && !hit.collider.gameObject.GetComponent<ShipPieceScript>().glued)
                    {
                    	Debug.Log("Grab hit");
                        hit.collider.transform.parent = transform.parent;
                        hit.collider.GetComponent<Rigidbody>().isKinematic = true;
                        Vector3 newPosition = transform.position;
                        newPosition += transform.forward * holdDistance;
                        heldObject = hit.collider.gameObject;
                        hit.collider.transform.position = newPosition;
                        isHolding = true;
                    }
                }
            }
            }
            else
            {
                if (heldObject != null)
            {
                Vector3 newPosition = transform.position;
                newPosition += transform.forward * holdDistance;
                heldObject.transform.position = newPosition;

                if (Input.GetMouseButtonUp(1))
                {
                    heldObject.GetComponent<Rigidbody>().isKinematic = false;
                    heldObject.transform.parent = null;
                    heldObject = null;
                    isHolding = false;
                }
            } else
            {
                isHolding = false;
            }
               
        }
    }
    
    private void FaceDirection()
    {
        directionY += HORIZONTAL_SPEED * Input.GetAxis("Mouse X");
        directionX += VERTICAL_SPEED * Input.GetAxis("Mouse Y");
        transform.rotation = Quaternion.Euler(-directionX, directionY, 0);
    }

    private void ShootGlue()
    {
        glueS[currentGlue].ShootSelf(transform.localRotation * Vector3.forward);
        PropelBackward(transform.localRotation * (-Vector3.forward * VELOCITY_MODIFIER * glueBuildup));
        ReadyNewGlue();
    }

    private void PropelBackward(Vector3 direction)
    {
        rb.velocity = new Vector3(0,0,0);
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
            currentHealth = currentHealth - 100;
            StartCoroutine("WaitandCheck");
        }
    }
    
    private void Grabber() 
    {
    	RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(transform.position, transform.localRotation * Vector3.forward, out hit, GRABBER_RANGE))
        {
            Debug.DrawRay(transform.position, transform.localRotation * Vector3.forward * hit.distance, Color.yellow, 30.0f);
            Debug.Log("Grab hit");
        }
        else
        {
            Debug.DrawRay(transform.position, transform.localRotation * Vector3.forward * 1000, Color.white, 30.0f);
            Debug.Log("Grab not Hit");
        }
    }

    IEnumerator WaitAndCheck()
    {
        yield return new WaitForSeconds(1.0f);
        if (currentHealth <= 0)
        {
            GameOver();
        }
    }

    void GameOver()
    {
        anim.SetTrigger("GameOver");
    }

}