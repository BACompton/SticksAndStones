using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Entity{
    /// <summary> Holder stats for an entity </summary>
    public class EntityStat : MonoBehaviour {
        // -------------------------- Unity Script Variables --------------------------

        /// <summary> The movement speed of an entity </summary>
        public Vector3 speed = new Vector3(6.0F, 0.0f, 6.0f);
        /// <summary> Flag to apply seprate gravity from the project's gravity. </summary>
        public bool ownGravity = false;
        /// <summary> The entity specifc gravity to apply given when ownGravity is flagged. </summary>
        public Vector3 gravity = Physics.gravity;

        // -------------------------- Helpers --------------------------

        /// <summary> Applys an objects gravity to an objects movement. </summary>
        /// <param name="move">The movement vector</param>
        /// <returns> The movement after gravity</returns>
        public Vector3 ApplyGravity(Vector3 move) {
            Vector3 g = ownGravity ? gravity : Physics.gravity;
            return move + (g * Time.deltaTime);
        }
    }
}
