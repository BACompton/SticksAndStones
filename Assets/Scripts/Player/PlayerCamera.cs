using UnityEngine;
using UI;

namespace Player {
    /// <summary>
    /// Translates players input into player rotation and camera rotation.
    /// </summary>
    public class PlayerCamera : MonoBehaviour {

        /// <summary>
        /// The gameobject the player controls
        /// </summary>
        public GameObject player;
        /// <summary>
        /// Flags when the player should be able to control their camera.
        /// </summary>
        public bool playerCamControl = true;
        /// <summary>
        /// Y rotation limits
        /// </summary>
        public float yMin = -80.0f, yMax = 80.0f;

        /// <summary>
        /// Player sensitivity stats
        /// </summary>
        private PlayerStat stat;
        /// <summary>
        /// The UI Manager for the scene
        /// </summary>
        private UIManager manager;
        /// <summary>
        /// The player's UI during gameplay
        /// </summary>
        private GameUI playerUI;
        /// <summary>
        /// The current yRot of the player's camera
        /// </summary>
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
            if(!playerCamControl || Cursor.lockState != CursorLockMode.Locked || !playerUI.Active)
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