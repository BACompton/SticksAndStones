using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Player;

namespace UI {
    public class Pause : GameUI {

        public static string ID = "Pause";

        // Use this for initialization
        new void Start() {
            base.Start();
            SetId();

            GameObject done = transform.Find("BG").Find("Options").Find("Controls").gameObject;
            GameObject quit = transform.Find("BG").Find("Options").Find("Quit").gameObject;

            done.GetComponent<Button>().onClick.AddListener(ToControls);
            quit.GetComponent<Button>().onClick.AddListener(ToQuit);
        }

        // Update is called once per frame
        void Update() {
            if(!active)
                return;

            // Cursor Control
            Cursor.lockState = CursorLockMode.None;

            // UI Transitions
            if(Input.GetKeyUp(Controls.Pause.key))
                manager.Transition(this, Player.ID, false);
        }

        /// <summary>
        /// Sets the id for the UI canvas
        /// </summary>
        public override void SetId() { id = ID; }

        /// <summary>
        /// Helper method to transition to the Controls UI
        /// </summary>
        private void ToControls() {
            manager.Transition(this, ControlsUI.ID, false);
        }

        /// <summary>
        /// Helper method to transition to any necessary Quit UI
        /// </summary>
        private void ToQuit() {
            Application.Quit();
        }
    }
}
