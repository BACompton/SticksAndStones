using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Puzzle;

public class DoorScipt : MonoBehaviour {

    private Animator anim;
    private PuzzleDevice puzzout;
	public AudioClip MusicClip;
	private AudioSource MusicSource;

	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
        puzzout = GetComponent<PuzzleDevice>();
		MusicSource = this.gameObject.gameObject.GetComponent<AudioSource>();
		MusicSource.clip = MusicClip;
	}
	
	// Update is called once per frame
	void Update () {
		if (puzzout.transmit) {
			if (!anim.GetBool ("Open")) {
				MusicSource.Play ();
			}
			anim.SetBool ("Open", true);

		}
    }
}