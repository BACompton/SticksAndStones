using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/**
 * Throw class
 * 
 * should contain script to both spawn and add force to an object provided that 
 * the proper input key is hit
 */
public class ThrowBall : MonoBehaviour {

    //public Transform ballPrefab;
    public Rigidbody Player;
    public Rigidbody ballRig;
    public Rigidbody proj;
    public float forceOfThrow = 1000.0f;
    public float distance = 1.5f;
 
    // Update is called once per frame
    void Start() {
        
    }

    void Update () {
		if (Input.GetKeyDown("f"))
        {
            proj = Instantiate(ballRig, transform.position + transform.forward*distance, transform.rotation);
            //ballRig.AddForce(Vector3.forward * forceOfThrow, ForceMode.Force);
            proj.velocity = transform.TransformDirection(Vector3.forward * forceOfThrow);
        }
	}

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.tag == "Player")
        {
            Destroy(this.gameObject);
        }
    }
}
