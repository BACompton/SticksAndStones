using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Player;

namespace UI {
    public class Pause : GameUI {

        public static string ID = "Pause";

        private Transform options;
        private int index;

        // Use this for initialization
        new void Start() {
            base.Start();
            SetId();
            index = 0;
            options = transform.Find("BG").Find("Options");

            GameObject done = options.Find("Controls").gameObject;
            GameObject quit = options.Find("Quit").gameObject;

            SelectItem();
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

            if(Input.GetKeyUp(Controls.SelectionUp.key)) {
                DeselectItem();
                if(--index < 0) index = options.childCount - 1;
                SelectItem();
            }

            if(Input.GetKeyUp(Controls.SelectionDown.key)) {
                DeselectItem();
                index = (index + 1) % options.childCount;
                SelectItem();
            }

            if(Input.GetKeyUp(Controls.Select.key))
                options.GetChild(index).gameObject.GetComponent<Button>().onClick.Invoke();
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

        /// <summary>
        /// Identifies the selected menu item.
        /// </summary>
        private void SelectItem() {
            GameObject option = options.GetChild(index).gameObject;
            option.GetComponentInChildren<Text>().fontStyle = FontStyle.Bold;
        }

        /// <summary>
        /// Deselects a selected menu item.
        /// </summary>
        private void DeselectItem() {
            GameObject option = options.GetChild(index).gameObject;
            option.GetComponentInChildren<Text>().fontStyle = FontStyle.Normal;
        }
    }
}
