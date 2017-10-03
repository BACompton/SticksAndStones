using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI {
    public class Pause : GameUI {

        public static string ID = "Pause";

        // Use this for initialization
        new void Start() {
            base.Start();
            SetId();
        }

        // Update is called once per frame
        void Update() {
            if(!active)
                return;

            Cursor.lockState = CursorLockMode.None;

            if(Input.GetKeyDown(KeyCode.V))
                manager.Transition(this, Player.ID, false);
        }

        /// <summary>
        /// Sets the id for the UI canvas
        /// </summary>
        public override void SetId() { id = ID; }
    }
}
