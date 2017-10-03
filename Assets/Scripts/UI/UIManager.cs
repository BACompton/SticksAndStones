using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI {
    public class UIManager : MonoBehaviour {

        public List<Canvas> canvases = new List<Canvas>();

        private List<GameUI> uis;

        // Use this for initialization
        void Start() {
            uis = new List<GameUI>();

            // Load Game UIs within the scene
            foreach(Canvas canvas in canvases) {
                GameUI ui = canvas.GetComponent<GameUI>();

                if(ui != null) {
                    ui.SetId();
                    uis.Add(ui);
                }
            }

            // Enable the first UI
            if(uis.Count > 0) {
                ActivateRender(uis[0], true, true);

                for(int i = 1; i < uis.Count; i++)
                    ActivateRender(uis[i], false, false);
            }
        }

        /// <summary>
        /// Transitions UI screens
        /// </summary>
        /// <param name="curr">The current ui screen</param>
        /// <param name="uiID">The id of the ui screen to transition</param>
        /// <param name="currRender">Flag to continue to render the previous ui</param>
        public void Transition(GameUI curr, string uiID, bool currRender) {
            foreach(GameUI ui in uis)
                if(ui.IsID(uiID)) {
                    ActivateRender(curr, false, currRender);
                    ActivateRender(ui, true, true);
                }
        }

        /// <summary>
        /// Looks for the specified UI.
        /// </summary>
        /// <param name="uiID">The id of the ui to seek</param>
        /// <returns>the ui if found, otherwise null.</returns>
        public GameUI GetUI(string uiID) {
            foreach(GameUI ui in uis) {
                if(ui.IsID(uiID))
                    return ui;
            }
            return null;
        }

        /// <summary>
        /// Simple helper method to set both the active and render flag for a UI.
        /// </summary>
        /// <param name="ui">The UI to set the flags</param>
        /// <param name="active">The active flag</param>
        /// <param name="render">The render flag</param>
        public static void ActivateRender(GameUI ui, bool active, bool render) {
            ui.render = render;
            ui.active = active;
        }
    }
}
