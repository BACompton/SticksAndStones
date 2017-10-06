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
        /// <summary>
        /// The amount of time before the button is depressed.
        /// </summary>
        public float deactivateDelay = 0.0f;
        /// <summary> The player's UI during gameplay </summary>
        public GameUI ui;

        /// <summary> The animatior for the button. </summary>
        private Animator anim;
        /// <summary> The button's input component. </summary>
        private PuzzleInput btnIn;
        /// <summary> The amount of time the button has been pressed with no trigger. </summary>
        private float pressed;
        /// <summary> Flag for no active trigger </summary>
        private bool noTrigger;

        // Use this for initialization
        void Start() {
            anim = GetComponent<Animator>();
            btnIn = GetComponent<PuzzleInput>();
            pressed = 0.0f;
            noTrigger = true;
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
        }

        private void OnTriggerStay(Collider other) {
            foreach (string tag in tags)
                if (other.gameObject.tag == tag) {
                    btnIn.active = true;
                    anim.SetFloat(SPEED, 1.0f);
                    noTrigger = false;
                    anim.SetBool(PRESSED, btnIn.active);
                }
        }

        void OnTriggerExit(Collider other) {
            foreach (string tag in tags)
                if (other.gameObject.tag == tag) {
                    pressed = 0.0f;
                    noTrigger = true;
                    anim.SetBool(PRESSED, false);
                }
        }
    }
}
