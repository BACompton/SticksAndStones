using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UI;
using Player;

namespace Puzzle
{
    public class Lever : Interactable
    {

        // -------------------------- Static Class Variables -------------------------- 

        /// <summary> The animation variable to set for pressed. </summary>
        private static string STATE { get { return "State"; } }
        /// <summary> The animation variable to set animation speed. </summary>
        private static string SPEED { get { return "Speed"; } }
        /// <summary> The number of possible states for the lever </summary>
        private static int STATE_LENGTH { get { return 3; } }

        // -------------------------- Unity Script Variables -------------------------- 

        /// <summary> Flag to create a timer lever </summary>
        public bool twoStates = false;
        /// <summary> Flag to create a timer lever </summary>
        public bool timer = false;
        /// <summary> The amount of time before the lever retutrns to default position. </summary>
        public float deactivateDelay = 1.0f;
        /// <summary> The animation speed of the button (only used when in toggle mode). </summary>
        public float speed = 1.0f;
        /// <summary> The player's UI during gameplay </summary>
        public GameUI ui;
        /// <summary> The current state of the button. </summary>
        public int state = 0;
        /// <summary> Delays the state of lever until the animation is finished </summary>
        public int activeState;
        /// <summary> Details if the puzzle device should be active in a certain state. </summary>
        public bool[] stateActivate = { false, true, true };


        // -------------------------- Class Variables --------------------------

        /// <summary> The animatior for the lever. </summary>
        private Animator anim;
        /// <summary> The lever's input component. </summary>
        private PuzzleDevice puzzleIn;
        /// <summary> The animation speed before being paused. </summary>
        private float prevSpeed;
        /// <summary> Flag to start the timer once the lever is in the proper state. </summary>
        private float startTimer, passedTime;
		/// <summary> The source of the sound </summary>
		private AudioSource MusicSource;
        
        /// <summary> Details how many animations needed to go through before updateing state. </summary>
        private int numOfAnin;

        // -------------------------- Unity Functios --------------------------

        // Use this for initialization
        void Start()
        {
            anim = GetComponent<Animator>();
            puzzleIn = GetComponent<PuzzleDevice>();
            prevSpeed = -1.0f;
            activeState = state;
            startTimer = -1.0f;
            numOfAnin = 0;
			MusicSource = GetComponent<AudioSource>();

            anim.SetInteger(STATE, state % STATE_LENGTH);
            anim.speed = 10;
        }

        // Update is called once per frame
        void Update()
        {
            if (ui != null && !ui.Active){
                prevSpeed = anim.GetFloat(SPEED);
                anim.speed = 0.0f;
                return;
            } else if (prevSpeed > 0.0f) {
                anim.speed = prevSpeed;
                prevSpeed = -1.0f;
            }

            if (!anim.IsInTransition(0) && numOfAnin > 0)
                numOfAnin--;

            if (numOfAnin == 0) {
                activeState = state;
                puzzleIn.update = true;
                numOfAnin = -1;
            }
            
            puzzleIn.active = stateActivate[activeState];

            if (startTimer > 0.0f)
            {
                if (passedTime >= startTimer)
                    Timer();
                else
                    passedTime += Time.deltaTime;
            }
        }

        // -------------------------- Player Interactable Behavior Functions --------------------------

        public override void Interact() {
            int diff = state;
            float spd = speed;
			MusicSource.Play ();
            if (timer) {
                startTimer = 1 / spd;
                state = state == 0 ? STATE_LENGTH - 1 : 0;
            }
            else {
                startTimer = -1.0f;
                state = ++state % STATE_LENGTH;

                if (twoStates && state == 1)
                    state = 2;
            }
            
            passedTime = 0.0f;
            diff = Mathf.Abs(diff - state);
            numOfAnin = diff;
            SetAnim(spd * diff);
        }

        // -------------------------- Helper Functions --------------------------

        private void Timer()
        {
            int diff = state;
            float spd = speed;

            spd = 1 / deactivateDelay;
            activeState = state;
            state = state == 0 ? STATE_LENGTH - 1 : 0;

            startTimer = -1.0f;
            diff = Mathf.Abs(diff - state);
            numOfAnin = diff;
            SetAnim(spd * diff);
        }

        private void SetAnim(float speed){
            anim.speed = speed;
            anim.SetInteger(STATE, state);
        }

    }
}
