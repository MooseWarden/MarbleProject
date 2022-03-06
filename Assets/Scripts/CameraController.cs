using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    public GameObject player;
    //public int moveOffset = 1;
    public int camSpeed = 1;

    private Vector3 distanceOffset;
    //private Quaternion initRotate;
    private Vector3 rotateX;
    //private Vector3 rotateY;
    private bool isPaused;

    // Start is called before the first frame update
    void Start()
    {
        distanceOffset = transform.position - player.transform.position;
        //initRotate = transform.rotation;
        rotateX = distanceOffset;
        //rotateY = distanceOffset;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = player.transform.position + distanceOffset;
        transform.position = player.transform.position + rotateX;
        //transform.position = player.transform.position + rotateY;

        transform.LookAt(player.transform.position);
        isPaused = player.GetComponent<PlayerController>().paused;

        if (Input.GetMouseButton(1) && isPaused != true)
        {
            /*
            float moveX = Input.GetAxis("Mouse X");
            transform.RotateAround(player.transform.position, Vector3.down, moveX * moveOffset);

            float moveY = Input.GetAxis("Mouse Y");
            transform.RotateAround(player.transform.position, Vector3.right, moveY * moveOffset);
            */

            rotateX = Quaternion.AngleAxis(Input.GetAxis("Mouse X") * camSpeed, Vector3.up) * rotateX;
            transform.position = player.transform.position + rotateX;
            transform.LookAt(player.transform.position);
            //Debug.Log(isPaused);
        }
        
        /*
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            rotateY = Quaternion.AngleAxis(Input.GetAxis("Mouse ScrollWheel") * camSpeed, Vector3.right) * rotateY;
            transform.position = player.transform.position + rotateY;
            transform.LookAt(player.transform.position);
        }

        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            rotateY = Quaternion.AngleAxis(Input.GetAxis("Mouse ScrollWheel") * camSpeed, Vector3.left) * rotateY;
            transform.position = player.transform.position - rotateY;
            transform.LookAt(player.transform.position);
            Debug.Log(Input.GetAxis("Mouse ScrollWheel"));
        }
        */

        /*
        if (Input.GetMouseButton(0))
        {
            //transform.rotation = initRotate;            
        }
        */
    }
}
