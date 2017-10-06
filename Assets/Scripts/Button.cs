using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UI;

namespace Entity {
    public class Button : MonoBehaviour {

        /// <summary> The animation variable to set for pressed. </summary>
        private static string PRESSED = "Pressed";
        /// <summary> The animation variable to set animation speed. </summary>
        private static string SPEED = "Speed";
        /// <summary> The triggers that can activate the button. </summary>
        public List<string> tags = new List<string> {
                "Player",
                "Heavy Trigger"
            };
        /// <summary> Flag to toggle button state </summary>
        public bool toggle = false;
        /// <summary>
        /// The amount of time before the button is depressed.
        /// </summary>
        public float deactivateDelay = 0.0f;
        /// <summary> The amount of time before the toggle can be flipped </summary>
        public float toggleDelay = 1.0f;
        /// <summary> The player's UI during gameplay </summary>
        public GameUI ui;

        /// <summary> The animatior for the button. </summary>
        private Animator anim;
        /// <summary> The button's input component. </summary>
        private PuzzleInput btnIn;
        /// <summary> The amount of time the button has been pressed with no trigger. </summary>
        private float pressed;
        /// <summary> The amount of time the toggle has been flipped </summary>
        private float toggleAlive;
        /// <summary> Flag for no active trigger </summary>
        private bool noTrigger;

        // Use this for initialization
        void Start() {
            anim = GetComponent<Animator>();
            btnIn = GetComponent<PuzzleInput>();
            pressed = 0.0f;
            noTrigger = true;
            toggleAlive = toggleDelay;
        }

        void Update() {
            if (ui != null && !ui.Active) {
                anim.SetFloat(SPEED, 0.0f);
                return;
            }

            if (noTrigger) {
                anim.SetFloat(SPEED, 1.0f / (1 + deactivateDelay));

                if (pressed < deactivateDelay)
                    pressed += Time.deltaTime;
                else
                    btnIn.active = false;
            }

            if (toggle && toggleAlive < toggleDelay)
                toggleAlive += Time.deltaTime;
        }

        private void OnTriggerStay(Collider other) {
            foreach (string tag in tags)
                if (other.gameObject.tag == tag) {
                    if (toggle) {
                        if (toggleAlive >= toggleDelay) {
                            if (noTrigger)
                                Press();
                            else
                                Unpress();
                        }
                    } else
                        Press();
                }
        }

        void OnTriggerExit(Collider other) {
            if (!toggle)
                foreach (string tag in tags)
                    if (other.gameObject.tag == tag)
                        Unpress();
        }

        /// <summary> Presses the button. </summary>
        private void Press() {
            toggleAlive = 0.0f;
            btnIn.active = true;
            anim.SetFloat(SPEED, 1.0f);
            noTrigger = false;
            anim.SetBool(PRESSED, btnIn.active);
        }

        /// <summary> Unpresses the button </summary>
        private void Unpress() {
            toggleAlive = 0.0f;
            pressed = 0.0f;
            noTrigger = true;
            anim.SetBool(PRESSED, false);
        }
    }
}
