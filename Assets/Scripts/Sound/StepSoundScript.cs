using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepSoundScript : MonoBehaviour {
	//public AudioClip MusicClip;
	//public AudioSource MusicSource;

	/// <summary> Player controller used to handle player movement </summary>
	private CharacterController cc;
	public AudioClip MusicClip;
	public AudioSource MusicSource;

	public float lowPitchRange = .95f;              //The lowest a sound effect will be randomly pitched.
	public float highPitchRange = 1.05f;            //The highest a sound effect will be randomly pitched.

	// Use this for initialization
	void Start () {
		MusicSource.clip = MusicClip;
		cc = GetComponent<CharacterController> ();
	}

	//RandomizeSfx chooses randomly between various audio clips and slightly changes their pitch.
	public void RandomizeSfx (params AudioClip[] clips)
	{
		//Generate a random number between 0 and the length of our array of clips passed in.
		int randomIndex = Random.Range(0, clips.Length);

		//Choose a random pitch to play back our clip at between our high and low pitch ranges.
		float randomPitch = Random.Range(lowPitchRange, highPitchRange);

		//Set the pitch of the audio source to the randomly chosen pitch.
		MusicSource.pitch = randomPitch;

		//Set the clip to the clip at our randomly chosen index.
		MusicSource.clip = clips[randomIndex];

		//Play the clip.
		MusicSource.Play();
	}

	// Update is called once per frame
	void Update () {
		/*if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.S)
			|| Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.UpArrow) ) {
			MusicSource.Play();
		}*/
		if (cc.isGrounded == true && cc.velocity.magnitude > 2f && MusicSource.isPlaying == false) {
			MusicSource.Play();
		}
	}
}