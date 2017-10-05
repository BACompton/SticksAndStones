using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UI;

namespace Player {
    /// <summary>
    /// Translates players input into player character actions.
    /// </summary>
    public class PlayerController : MonoBehaviour {
        /// <summary>
        /// The player's UI during gameplay
        /// </summary>
        public GameUI playerUI;

        /// <summary>
        /// Player controller used to handle player movement
        /// </summary>
        private CharacterController controller;
        /// <summary>
        /// The current movement direction of the player
        /// </summary>
        private Vector3 moveDirection;

        /// <summary>
        /// The movemnt setting for the player
        /// </summary>
        private PlayerStat stat;

        private void Start() {
            controller = GetComponent<CharacterController>();
            stat = GetComponent<PlayerStat>();
            moveDirection = Vector3.zero;
        }

        void Update() {
            if(playerUI != null && !playerUI.Active) {
                controller.Move(Vector3.zero);
                return;
            }

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
        }
    }
}
