using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Puzzle;

public class WindowScript : MonoBehaviour {

    private Animator anim;
    private PuzzleDevice puzzout;
    private AudioSource MusicSource;

	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
        puzzout = GetComponent<PuzzleDevice>();
        MusicSource = GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void Update () {
        bool prev = anim.GetBool("Open");

        if (puzzout.transmit)
            anim.SetBool("Open", true);
        else
            anim.SetBool("Open", false);

        if(anim.GetBool("Open") != prev)
            MusicSource.Play();
    }
}