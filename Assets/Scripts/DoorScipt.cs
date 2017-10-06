﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Entity;

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
        anim.SetBool("Open", puzzout.AllActivated());
        print(puzzout.AllActivated());

    }
}
