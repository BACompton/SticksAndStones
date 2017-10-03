using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player {
    public class PlayerController : MonoBehaviour {
        private CharacterController controller;
        private Vector3 moveDirection;

        private PlayerStat stat;

        private void Start() {
            controller = GetComponent<CharacterController>();
            stat = GetComponent<PlayerStat>();
            moveDirection = Vector3.zero;
        }

        void Update() {
            if (controller.isGrounded) {
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
