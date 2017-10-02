using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraY : MonoBehaviour {

	// Update is called once per frame
	void Update () {
        float mouseVertical = -(Input.GetAxis("Mouse Y"));
        Vector3 lookVert = new Vector3(mouseVertical, 0);
        transform.Rotate(lookVert);
    }
}
