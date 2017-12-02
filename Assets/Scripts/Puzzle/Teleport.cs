using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour {

    public Vector3 dest = Vector3.zero;
    public List<string> whitelist = new List<string>() { "Player" };

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if (whitelist.Contains(other.gameObject.tag))
        {
            other.gameObject.transform.SetPositionAndRotation(dest, other.gameObject.transform.rotation);
            Debug.Log("Why");
        }
    }
}
