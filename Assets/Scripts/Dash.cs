using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash : MonoBehaviour
{
    private float dashTime;
    public GameObject playerObj;
    public float dashSpeed = 25;
    public bool isCooldown = false;
    // Start is called before the first frame update
    public void Start()
    {
        //StartCoroutine(DashCoroutine());
    }

    // Update is called once per frame
    void Update()
    {

    }

    public IEnumerator DashCoroutine()
    {
        //float startTime = Time.time;
        isCooldown = true;
        playerObj.GetComponent<Rigidbody>().AddForce(Vector3.left * dashSpeed, ForceMode.Impulse);

        yield return new WaitForSeconds(0.25f);
        playerObj.GetComponent<Rigidbody>().AddForce(Vector3.left * -(playerObj.GetComponent<Rigidbody>().velocity.magnitude / 1.1f), ForceMode.Impulse);
        isCooldown = false;
    }

}
