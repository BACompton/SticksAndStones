using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Player;

namespace UI {
    /// <summary> Player UI Interaction script </summary>
    public class Player : GameUI {
        /// <summary> Player UI ID used to identify a player UI script. </summary>
        public static string ID = "Player";

        /// <summary> The UI element that holds all the item sprites </summary>
        public Transform itemContainer;
        /// <summary> The player's controlling script </summary>
        public PlayerController player;
        /// <summary> The lifetime indicator for the current item </summary>
        public Slider slider;

        /// <summary> Details the current item being show on the UI </summary>
        private int activeIndex;
        

        // Use this for initialization
        new void Start() {
            base.Start();
            SetId();

            if (player != null)
                activeIndex = player.itemIndex;
            else
                activeIndex = 0;
            ShowItem(activeIndex);
        }

        void OnEnable() {
            activeIndex = player.itemIndex;
        }

        // Update is called once per frame
        void Update() {
            if (!Active)
                return;

            // Cursor Control
            Cursor.lockState = CursorLockMode.Locked;

            // UI Transitions
            if (Input.GetKeyUp(Controls.Pause.key))
                Manager.Transition(this, Pause.ID, true);

            // Show current item
            if (player != null && itemContainer != null) {
                if (activeIndex != player.itemIndex) {
                    HideItem(activeIndex);
                    activeIndex = player.itemIndex;

                    if (activeIndex < itemContainer.childCount)
                        ShowItem(activeIndex);
                }

                Item curr = player.inventory[activeIndex];
                float lifetime = (curr.respawnDelay - curr.alive) / curr.respawnDelay;

                if (lifetime > 0) {
                    slider.gameObject.SetActive(true);
                    slider.value = lifetime;
                } else
                    slider.gameObject.SetActive(false);
            }
        }

        /// <summary> Sets the id for the UI canvas </summary>
        public override void SetId() { id = ID; }

        /// <summary> Helper method so show an item's sprite in the ui. </summary>
        /// <param name="index"> The index to show</param>
        private void ShowItem(int index) {
            itemContainer.GetChild(index).gameObject.SetActive(true);
        }

        /// <summary> Helper method so hide an item's sprite in the ui. </summary>
        /// <param name="index"> The index to show</param>
        private void HideItem(int index) {
            itemContainer.GetChild(index).gameObject.SetActive(false);
        }
    }
}
