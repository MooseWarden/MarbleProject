using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    public float frequency = 2.0f;
    public float amplitude = 1.0f;
    public bool isOsc = false;
    public bool moveInX = false;
    public bool moveInY = false;
    public bool moveInZ = false;

    private float startX;
    private float startY;
    private float startZ;
    private float localTimeStorage;
    private float localTime;

    private void Start()
    {
        startX = this.transform.position.x;
        startY = this.transform.position.y;
        startZ = this.transform.position.z;
        localTime = 0;
        localTimeStorage = localTime;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(15, 30, 45) * Time.deltaTime);
        Move(isOsc);
        localTime += Time.deltaTime;
    }

    void Move(bool truth)
    {
        if(truth == true)
        {
            float t = 2 * Mathf.PI * localTime / frequency;
            float sine = Mathf.Sin(t);
            float offsetx = startX + amplitude * sine;
            float offsety = startY + amplitude * sine;
            float offsetz = startZ + amplitude * sine;


            Vector3 pos = this.transform.position;
            
            if(moveInX == true)
            {
                pos.x = offsetx;

            }
            if (moveInY == true)
            {
                pos.y = offsety;

            }
            if (moveInZ == true)
            {
                pos.z = offsetz;

            }

            transform.position = pos;

            localTimeStorage = localTime;
        }
        else
        {
            localTime = localTimeStorage;
        }
    }
}
