using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Puzzle;

public class DoorScipt : MonoBehaviour {

    private Animator anim;
    private PuzzleDevice puzzout;

	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
        puzzout = GetComponent<PuzzleDevice>();
	}
	
	// Update is called once per frame
	void Update () {
        if(puzzout.transmit)
            anim.SetBool("Open", true);
    }
}