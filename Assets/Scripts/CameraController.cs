using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    public GameObject player;
    public GameObject pivot;
    public float minX;
    public float maxX;
    public int camSpeed;

    private Vector3 rotatorHoldX;
    private Vector3 rotatorHoldY;
    private Vector3 distanceOffset;
    private Vector3 rotateX;
    private Vector3 rotateY;
    private bool isPaused;

    // Start is called before the first frame update
    void Start()
    {
        distanceOffset = transform.position - player.transform.position;
        transform.position = player.transform.position + distanceOffset;
        rotatorHoldX = Vector3.zero;
        rotatorHoldY = Vector3.zero;
        rotateX = Vector3.zero;
        rotateY = Vector3.zero;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        isPaused = player.GetComponent<PlayerController>().paused;

        pivot.transform.position = player.transform.position;

        if (Input.GetAxis("Mouse ScrollWheel") > 0 && isPaused != true)
        {
            rotateX = Quaternion.AngleAxis(Input.GetAxis("Mouse ScrollWheel"), Vector3.right) * Vector3.one;
            rotatorHoldX.x -= (rotateX.x * camSpeed);
            if (rotatorHoldX.x < minX)
            {
                rotatorHoldX.x = minX;
            }
        }

        if (Input.GetAxis("Mouse ScrollWheel") < 0 && isPaused != true)
        {
            rotateX = Quaternion.AngleAxis(Input.GetAxis("Mouse ScrollWheel"), Vector3.right) * Vector3.one;
            rotatorHoldX.x += (rotateX.x * camSpeed);
            if (rotatorHoldX.x > maxX)
            {
                rotatorHoldX.x = maxX;
            }
        }

        if (Input.GetMouseButton(1) && Input.GetAxis("Mouse X") > 0 && isPaused != true)
        {
            rotateY = Quaternion.AngleAxis(Input.GetAxis("Mouse X"), Vector3.up) * Vector3.one;
            rotatorHoldY.y += (rotateY.y * camSpeed);
        }

        if (Input.GetMouseButton(1) && Input.GetAxis("Mouse X") < 0 && isPaused != true)
        {
            rotateY = Quaternion.AngleAxis(Input.GetAxis("Mouse X"), Vector3.up) * Vector3.one;
            rotatorHoldY.y -= (rotateY.y * camSpeed);
        }

        if (Input.GetMouseButton(0) && isPaused != true)
        {
            rotatorHoldX = Vector3.zero;
            rotatorHoldY = Vector3.zero;
        }

        pivot.transform.eulerAngles = new Vector3(rotatorHoldX.x, rotatorHoldY.y, 0);
                        
        transform.LookAt(player.transform.position);
    }
}
