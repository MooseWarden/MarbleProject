using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBoostCtrl : MonoBehaviour
{
    //set value in editor, should be higher than the public speed value in the playercontroller script on the player object
    public int speedBoost;

    private void Start()
    {
        //experimental code to mess around with the intensity of the color based on speed
        //not necessary but a potential aesthetic look if we want to revisit this
        /*
        //alter color to show the intensity of the boost
        Color alter = this.GetComponent<MeshRenderer>().material.color;

        alter.r += ((float)speedBoost / 2);
        alter.g -= ((float)speedBoost / 2);
        //alter.b += ((float)speedBoost / 2);

        this.GetComponent<MeshRenderer>().material.color = alter;
        */
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            //get the rigidBody from the player object
            Rigidbody rb = other.GetComponent<Rigidbody>();

            //calculate the angle of the force to be applied based on the rotation of the speedpad on the horizontal plane (rotation around the y axis)
            //MAKE SURE TO TEST DIRECTION IN GAME, if the pads are used as children of a parent, their Y rotation is flipped internally
            float angle = this.transform.eulerAngles.y;
            float targetAngle = Mathf.Atan2(0, 0) * Mathf.Rad2Deg + angle;  //Mathf.Atan2(this.transform.rotation.x, this.transform.rotation.z) * Mathf.Rad2Deg + angle;
            Vector3 moveDir = Quaternion.Euler(0.0f, targetAngle, 0.0f) * Vector3.forward;

            //cancel out player movement force to allow the speed boost to be pure
            Vector3 cancelMoveForce = new Vector3(-rb.velocity.x, 0.0f, -rb.velocity.z);
            rb.AddForce(cancelMoveForce, ForceMode.Impulse);

            //apply speed boost
            rb.AddForce(moveDir * speedBoost, ForceMode.Impulse);
        }
    }
}
