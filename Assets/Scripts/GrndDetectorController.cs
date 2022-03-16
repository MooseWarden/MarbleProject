using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrndDetectorController : MonoBehaviour
{
    public GameObject playerObj;

    private Quaternion InitRot;
    private Vector3 distanceOffset;
    private PlayerController playerScript;

    // Start is called before the first frame update
    void Start()
    {
        distanceOffset = transform.position - playerObj.transform.position;

        InitRot = transform.rotation;

        playerScript = playerObj.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LateUpdate()
    {
        transform.position = playerObj.transform.position + distanceOffset;

        transform.rotation = InitRot;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if ((other.gameObject.CompareTag("Ground") || other.gameObject.CompareTag("Wall")) && playerScript.touchingGround == false)
        {
            playerScript.touchingGround = true;
            playerScript.jumpCharges = playerScript.maxJumps;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if ((other.gameObject.CompareTag("Ground") || other.gameObject.CompareTag("Wall")) && playerScript.touchingGround == true)
        {
            playerScript.touchingGround = false;
            playerScript.onAngledGrnd = false;
        }
    }
}
