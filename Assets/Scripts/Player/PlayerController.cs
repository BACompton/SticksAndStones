using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UI;

namespace Player {
    public class PlayerController : MonoBehaviour {
        public Camera managerObj;

        private UIManager manager;
        private GameUI playerUI;

        private CharacterController controller;
        private Vector3 moveDirection;

        private PlayerStat stat;

        private void Start() {
            controller = GetComponent<CharacterController>();
            stat = GetComponent<PlayerStat>();
            moveDirection = Vector3.zero;

            manager = managerObj.GetComponent<UIManager>();
            playerUI = manager.GetUI(UI.Player.ID);
        }

        void Update() {
            if(!playerUI.active) {
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
