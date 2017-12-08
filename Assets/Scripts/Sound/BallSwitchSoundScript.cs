using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSwitchSoundScript : MonoBehaviour {
	public AudioClip MusicClip;
	public AudioSource MusicSource;
	// Use this for initialization
	void Start () {
		MusicSource.clip = MusicClip;
	}

	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Alpha2)) {
			MusicSource.Play();
		}
	}
}