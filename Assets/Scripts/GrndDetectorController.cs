using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrndDetectorController : MonoBehaviour
{
    public GameObject player;

    private Quaternion InitRot;
    private Vector3 distanceOffset;

    // Start is called before the first frame update
    void Start()
    {
        distanceOffset = transform.position - player.transform.position;

        InitRot = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LateUpdate()
    {
        transform.position = player.transform.position + distanceOffset;

        transform.rotation = InitRot;
    }

    /*
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Ground") && player.GetComponent<PlayerController>().touchingGround == false)
        {
            player.GetComponent<PlayerController>().touchingGround = true;
            player.GetComponent<PlayerController>().jumpCharges = player.GetComponent<PlayerController>().maxJumps;
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("Ground") && player.GetComponent<PlayerController>().touchingGround == true)
        {
            player.GetComponent<PlayerController>().touchingGround = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        
    }
    */
}
