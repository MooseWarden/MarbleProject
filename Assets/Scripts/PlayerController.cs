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
    public bool paused;
    public GameObject GrndDetector;
    public bool onAngledGrnd;

    private Rigidbody rb;
    private int count;
    private float movementX;
    private float movementY;
    private Vector3 start;
    private float movementZ;
    private int numPickUps;
    public int jumpCharges;
    public bool touchingGround;
    private bool canJump;
    private bool visible;
    private Vector3 tempNormal;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        count = 0;
        numPickUps = GameObject.FindGameObjectsWithTag("PickUp").Length;
        SetCountText();
        winTextObject.SetActive(false);

        touchingGround = false;
        canJump = true;
        jumpCharges = maxJumps;

        start = transform.position;

        paused = false;
        visible = false;
        onAngledGrnd = false;

        tempNormal = Vector3.zero;
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
        if (canJump == true && jumpCharges > 0 && paused == false)
        {
            Vector3 movement = Vector3.zero;

            if(onAngledGrnd == false)
            {
                movement = new Vector3(0.0f, movementY, 0.0f); //x is left to right, y is up and down, z is forwad and back
            }
            else if(onAngledGrnd == true)
            {
                movement = tempNormal;
                onAngledGrnd = false;
                Vector3 cancelXZForce = new Vector3(-rb.velocity.x, 0.0f, -rb.velocity.z);
                rb.AddForce(cancelXZForce, ForceMode.Impulse);
            }
            Vector3 cancelYForce = new Vector3(0.0f, -rb.velocity.y, 0.0f);
            rb.AddForce(cancelYForce, ForceMode.Impulse);
            rb.AddForce(movement * jumpMultiplier, ForceMode.Impulse);
            jumpCharges--;
            GrndDetector.GetComponent<ParticleSystem>().Clear();
            GrndDetector.GetComponent<ParticleSystem>().Play();
        }
    }

    void FixedUpdate()
    {
        float angle = cameraObj.transform.eulerAngles.y;
        float targetAngle = Mathf.Atan2(movementX, movementZ) * Mathf.Rad2Deg + angle;
        Vector3 moveDir = Quaternion.Euler(0.0f, targetAngle, 0.0f) * Vector3.forward;
        
        if(movementX == 0.0f && movementZ == 0.0f)
        {
            rb.AddForce(Vector3.zero);
        }
        else
        {
            rb.AddForce(moveDir * speed);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PickUp"))
        {
            other.gameObject.SetActive(false);
            count = count + 1; //count++;

            SetCountText();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (Vector3.Dot(collision.contacts[0].normal, Vector3.up) != 1)
        {
            onAngledGrnd = true;
            tempNormal = collision.contacts[0].normal;
        }
        else if(Vector3.Dot(collision.contacts[0].normal, Vector3.up) == 1)
        {
            onAngledGrnd = false;
        }
    }
}
