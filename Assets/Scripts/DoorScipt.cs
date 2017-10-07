using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Puzzle;

public class DoorScipt : MonoBehaviour {

    private Animator anim;
    private PuzzleOutput puzzout;

	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
        puzzout = GetComponent<PuzzleOutput>();
	}
	
	// Update is called once per frame
	void Update () {
        if(puzzout.AllActivated())
            anim.SetBool("Open", true);
    }
}