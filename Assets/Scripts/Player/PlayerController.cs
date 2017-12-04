using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UI;

namespace Player {
    /// <summary> Translates players input into player character actions. </summary>
    public class PlayerController : MonoBehaviour {
        // -------------------------- Static Class Variables --------------------------

        /// <summary> Details all item slots avaliable to the player. </summary>
        public static KeyControl[] ItemSlots { get { return _itemSlots; } }
        /// <summary> Internal holder for ItemSlots </summary>
        private static KeyControl[] _itemSlots = Controls.FindMatching("Item Slot");

        // -------------------------- Unity Script Variables --------------------------

        /// <summary> The player's UI during gameplay </summary>
        public GameUI playerUI;
        /// <summary> The player's inventory </summary>
        public List<Item> inventory;
        /// <summary> Identifies currently held item. </summary>
        public int itemIndex = 0;

        // -------------------------- Class Variables --------------------------

        /// <summary> Player controller used to handle player movement </summary>
        private CharacterController controller;
        /// <summary> The current movement direction of the player </summary>
        private Vector3 moveDirection;
        /// <summary> The movemnt setting for the player </summary>
        private PlayerStat stat;
        /// <summary> The ground the player is standing on </summary>
        private Rigidbody ground;
        // -------------------------- Unity Functions --------------------------

        private void Start() {
            controller = GetComponent<CharacterController>();
            stat = GetComponent<PlayerStat>();
            moveDirection = Vector3.zero;

            itemIndex = 0;
            foreach (Item i in inventory)
                i.alive = i.respawnDelay;
        }

        void Update() {
            if (playerUI != null && !playerUI.Active) {
                controller.Move(Vector3.zero);
                return;
            }

            CaptureMove();

            CaptureItemSwitch();
            if (itemIndex >= 0 && itemIndex < inventory.Count) {
                CapturFire();
                CaptureAim();
            }
        }

        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            Vector3 tran = hit.gameObject.transform.position;
            Rigidbody rigid = hit.gameObject.GetComponent<Rigidbody>();

            if (rigid != null && tran.y <= gameObject.transform.position.y)
                ground = rigid;
        }

        // -------------------------- Player Input Functions --------------------------

        /// <summary> Handle Player Movement </summary>
        private void CaptureMove() {
            if (controller.isGrounded) {
                // Scale users input to player movement
                moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
                moveDirection = transform.TransformDirection(moveDirection);
                moveDirection.Scale(stat.speed);

                
                if (ground != null)
                    moveDirection += ground.velocity;

                if (Input.GetButton("Jump"))
                    moveDirection += stat.jumpSpeed;
            }

            moveDirection = stat.ApplyGravity(moveDirection);
            controller.Move(moveDirection * Time.deltaTime);
        }

        /// <summary> Handle item switching </summary>
        private void CaptureItemSwitch() {
            // TODO: Item wheel
            for (int i = 0; i < inventory.Count && i < ItemSlots.Length; i++)
                if (Input.GetKeyDown(ItemSlots[i].key))
                    itemIndex = i;
        }

        /// <summary> Handles player fire input </summary>
        private void CapturFire() {
            if (Input.GetKeyUp(Controls.Fire.key))
                inventory[itemIndex].Fire(InputType.UP);
            if (Input.GetKeyDown(Controls.Fire.key))
                inventory[itemIndex].Fire(InputType.DOWN);
            if (Input.GetKey(Controls.Fire.key))
                inventory[itemIndex].Fire(InputType.HOLD);
        }

        /// <summary> Handles player aim input </summary>
        private void CaptureAim() {
            if (Input.GetKeyUp(Controls.Aim.key))
                inventory[itemIndex].Aim(InputType.UP);
            if (Input.GetKeyDown(Controls.Aim.key))
                inventory[itemIndex].Aim(InputType.DOWN);
            if (Input.GetKey(Controls.Aim.key))
                inventory[itemIndex].Aim(InputType.HOLD);
        }
    }
}
