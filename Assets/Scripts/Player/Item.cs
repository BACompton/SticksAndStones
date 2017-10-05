using UnityEngine;

namespace Player {
    /// <summary> Identifies the type of input. </summary>
    public enum InputType { HOLD, DOWN, UP }

    /// <summary> Interface for an player item. </summary>
    public class Item : MonoBehaviour {
        /// <summary> Details if the item is avilable for use </summary>
        public bool available = true;
        /// <summary> Details if the item is disabled </summary>
        public bool disabled = false;
        /// <summary> The amount of time before recalling back to the player </summary>
        public float respawnDelay = 15.0f;
        /// <summary> The amount of time the ball have been alive </summary>
        public float alive = 0.0f;


        /// <summary> Method used when the player presses the fire key </summary>
        public virtual void Fire(InputType type) { }

        /// <summary> Method used when the player presses the block button </summary>
        public virtual void Aim(InputType type) { }
    }
}
