using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI {
    public class Player : GameUI {

        public static string ID = "Player";

        // Use this for initialization
        new void Start() {
            base.Start();
            SetId();
        }

        // Update is called once per frame
        void Update() {
            if(!active)
                return;

            // Cursor Control
            Cursor.lockState = CursorLockMode.Locked;

            // UI Transitions
            if(Input.GetKeyDown(KeyCode.V))
                manager.Transition(this, Pause.ID, true);                
        }

        /// <summary>
        /// Sets the id for the UI canvas
        /// </summary>
        public override void SetId() { id = ID; }
    }
}
