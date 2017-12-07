using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UI;

public class Rotator : MonoBehaviour {

    public Vector3 direction;
    public GameUI ui;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        // Stop rotation if game is paused
        if(ui != null && !ui.Active) {
            return;
        }
        // Object rotation based
        transform.Rotate(direction * Time.deltaTime);
	}
}
