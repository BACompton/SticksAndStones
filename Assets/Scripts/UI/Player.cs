using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;

namespace UI {
    /// <summary> Player UI Interaction script </summary>
    public class Player : GameUI {
        /// <summary> Player UI ID used to identify a player UI script. </summary>
        public static string ID = "Player";

        // Use this for initialization
        new void Start() {
            base.Start();
            SetId();
        }

        // Update is called once per frame
        void Update() {
            if(!Active)
                return;

            // Cursor Control
            Cursor.lockState = CursorLockMode.Locked;

            // UI Transitions
            if(Input.GetKeyUp(Controls.Pause.key))
                Manager.Transition(this, Pause.ID, true);
        }

        /// <summary> Sets the id for the UI canvas </summary>
        public override void SetId() { id = ID; }
    }
}
