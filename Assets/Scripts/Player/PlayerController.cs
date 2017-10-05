﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UI;

namespace Player {
    /// <summary> Translates players input into player character actions. </summary>
    public class PlayerController : MonoBehaviour {
        /// <summary> The player's UI during gameplay </summary>
        public GameUI playerUI;
        /// <summary> The player's inventory </summary>
        public List<Item> inventory;

        /// <summary> Player controller used to handle player movement </summary>
        private CharacterController controller;
        /// <summary> The current movement direction of the player </summary>
        private Vector3 moveDirection;

        /// <summary> The movemnt setting for the player </summary>
        private PlayerStat stat;
        /// <summary> Identifies currently held item. </summary>
        private int itemIndex;

        private void Start() {
            controller = GetComponent<CharacterController>();
            stat = GetComponent<PlayerStat>();
            moveDirection = Vector3.zero;
            itemIndex = 0;
        }

        void Update() {
            if (playerUI != null && !playerUI.Active) {
                controller.Move(Vector3.zero);
                return;
            }

            // Handle Movement
            if (controller.isGrounded) {
                // Scale users input to player movement
                moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
                moveDirection = transform.TransformDirection(moveDirection);
                moveDirection.Scale(stat.speed);

                if (Input.GetButton("Jump"))
                    moveDirection += stat.jumpSpeed;
            }

            moveDirection = stat.applyGravity(moveDirection);
            controller.Move(moveDirection * Time.deltaTime);

            // Handle Item Actions
            if (itemIndex >= 0 && itemIndex < inventory.Count) {
                if (Input.GetKeyUp(Controls.Fire.key))
                    inventory[itemIndex].Fire(InputType.UP);
                if (Input.GetKeyDown(Controls.Fire.key))
                    inventory[itemIndex].Fire(InputType.DOWN);
                if (Input.GetKey(Controls.Fire.key))
                    inventory[itemIndex].Fire(InputType.HOLD);


                if (Input.GetKeyUp(Controls.Aim.key))
                    inventory[itemIndex].Aim(InputType.UP);
                if (Input.GetKeyDown(Controls.Aim.key))
                    inventory[itemIndex].Aim(InputType.DOWN);
                if (Input.GetKey(Controls.Aim.key))
                    inventory[itemIndex].Aim(InputType.HOLD);
            }
        }
    }
}
