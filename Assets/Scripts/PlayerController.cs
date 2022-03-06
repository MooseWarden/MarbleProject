using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public float speed = 0;
    public TextMeshProUGUI countText;
    public GameObject winTextObject;
    public GameObject infoTextObject;
    public GameObject cameraObj;
    public float jumpMultiplier = 2f;
    public int maxJumps = 1;
    public int speedMultiplier;
    public bool paused;

    private Rigidbody rb;
    private int count;
    private float movementX;
    private float movementY;
    private Vector3 start;
    private float movementZ;
    private int numPickUps;
    private int jumpCharges;
    private bool touchingGround;
    private bool canJump;
    private bool visible;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        count = 0;
        numPickUps = GameObject.FindGameObjectsWithTag("PickUp").Length;
        SetCountText();
        winTextObject.SetActive(false);

        //Debug.Log(numPickUps);
        touchingGround = false;
        //Debug.Log(touchingGround);
        canJump = true;
        jumpCharges = maxJumps;

        start = transform.position;

        paused = false;
        visible = false;
    }

    //had to change up the input schema from the input actions window to make it a vector3
    //movementZ is meant for jumping
    void OnMove(InputValue movementValue)
    {
        Vector3 movementVector = movementValue.Get<Vector3>();
        
        movementX = movementVector.x;
        movementY = movementVector.y;
        movementZ = movementVector.z;
    }

    void SetCountText()
    {
        countText.text = "Count: " + count.ToString() + "/" + numPickUps.ToString();
        if(count >= numPickUps)
        {
            winTextObject.SetActive(true);
        }
    }

    void Update()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame == true)
        {
            Jump();
        }

        if(Input.GetKeyDown(KeyCode.R) == true && paused == false)
        {
            transform.position = start;
        }

        if (Input.GetKeyDown(KeyCode.P) == true && paused == false)
        {
            Time.timeScale = 0;
            paused = true;
        }
        else if (Input.GetKeyDown(KeyCode.P) == true && paused == true)
        {
            Time.timeScale = 1;
            paused = false;
        }

        if (Input.GetKeyDown(KeyCode.I) == true && paused == true && visible == false)
        {
            infoTextObject.SetActive(!infoTextObject.activeSelf);
            visible = true;
        }
        else if (Input.GetKeyDown(KeyCode.I) == true && paused == true && visible == true)
        {
            infoTextObject.SetActive(!infoTextObject.activeSelf);
            visible = false;
        }
        else if (paused == false)
        {
            infoTextObject.SetActive(false);
            visible = false;
        }
    }

    void Jump()
    {
        if (canJump == true && jumpCharges > 0 && paused == false) //used to use the touchingGround bool
        {
            Vector3 movement = Vector3.zero;
            movement = new Vector3(0.0f, movementY, 0.0f); //x is left to right, y is up and down, z is forwad and back
            Vector3 cancelYForce = new Vector3(0.0f, -rb.velocity.y, 0.0f);
            rb.AddForce(cancelYForce, ForceMode.Impulse);
            rb.AddForce(movement * jumpMultiplier, ForceMode.Impulse);
            jumpCharges--;
            //Debug.Log("canJump: " + canJump + "|| jumpCharges: " + jumpCharges + "|| maxjumps " + maxJumps + "|| touching " + touchingGround);
        }
    }

    void FixedUpdate()
    {
        float angle = cameraObj.transform.eulerAngles.y;
        float targetAngle = Mathf.Atan2(movementX, movementZ) * Mathf.Rad2Deg + angle;
        Vector3 moveDir = Quaternion.Euler(0.0f, targetAngle, 0.0f) * Vector3.forward;
        //Vector3 movement = new Vector3(movementX, 0.0f, movementZ); //x is left to right, y is up and down, z is forwad and back
        //rb.AddForce(movement * speed);
        if(movementX == 0.0f && movementZ == 0.0f)
        {
            rb.AddForce(Vector3.zero);
        }
        else
        {
            rb.AddForce(moveDir * speed);
        }
        //Debug.Log(moveDir);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PickUp"))
        {
            other.gameObject.SetActive(false);
            count = count + 1; //count++;

            SetCountText();
        }

        //FIX, UNTANGLE FROM HERE, SAME WITH THE OTHER DEPENDANT OBJECTS, ONLY SHIT RELATING TO PLAYER SPECIFICALLY SHOULD GO HERE
        if (other.gameObject.CompareTag("SpeedPad"))
        {
            float angle = other.transform.eulerAngles.y;
            float targetAngle = Mathf.Atan2(movementX, movementZ) * Mathf.Rad2Deg + angle;
            Vector3 moveDir = Quaternion.Euler(0.0f, targetAngle, 0.0f) * Vector3.forward;
            //Vector3 go = new Vector3(-1f, 0.0f, 0.0f);
            
            Vector3 cancelMoveForce = new Vector3(-rb.velocity.x, 0.0f, -rb.velocity.z);
            rb.AddForce(cancelMoveForce, ForceMode.Impulse);
            rb.AddForce(Vector3.zero);

            rb.AddForce(moveDir * speedMultiplier, ForceMode.Impulse);
            Debug.Log(moveDir * speedMultiplier);
        }
    }
    
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Ground") && touchingGround == false)
        {
            touchingGround = true;
            jumpCharges = maxJumps;
            //Debug.Log("touching");
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("Ground") && touchingGround == true)
        {
            touchingGround = false;
            //Debug.Log("not touching");
        }
    }
}
