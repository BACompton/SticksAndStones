using UnityEngine;
using UI;

namespace Player {
    public class PlayerCamera : MonoBehaviour {

        public GameObject player;
        public bool playerCamControl = true;
        public float yMin = -80.0f, yMax = 80.0f;

        private PlayerStat stat;
        private UIManager manager;
        private GameUI playerUI;
        private float yRot;

        void Start() {
            // Try to loacte the player
            if(player == null) {
                // Assuming Player is named Player
                if(GameObject.Find("Player") != null)
                    player = GameObject.Find("Player");
                // Assuming Player is Tagged Player
                if(GameObject.FindGameObjectWithTag("Player") != null)
                    player = GameObject.FindGameObjectWithTag("Player");
            }

            manager = GetComponent<UIManager>();
            playerUI = manager.GetUI(UI.Player.ID);
            yRot = 0.0f;

            // Load Player Stats
            if(player != null) {
                player = GameObject.Find(player.name);
                stat = player.GetComponent<PlayerStat>();
            } else
                stat = null;
        }

        // Update is called once per frame
        void Update() {
            // Stops Input calculations according
            if(!playerCamControl || Cursor.lockState != CursorLockMode.Locked || !playerUI.active)
                 return;
                
            // Record Inputs
            Vector2 mouseRot = new Vector2(Input.GetAxis("Mouse X"), -Input.GetAxis("Mouse Y"));

            // Adjust to player senstivity
            if(stat != null)
                mouseRot.Scale(stat.rotSensitivity);

            yRot += mouseRot.y;
            yRot = Mathf.Clamp(yRot, yMin, yMax);
            Vector3 lookPlayer = new Vector3(0, mouseRot.x);

            // Apply Rotations
            transform.rotation = Quaternion.Euler(yRot, transform.eulerAngles.y, transform.eulerAngles.z);
            if(player != null)
                player.transform.Rotate(lookPlayer);
        }
    }
}