using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Entity{
    public class EntityStat : MonoBehaviour
    {
        public Vector3 speed = new Vector3(6.0F, 0.0f, 6.0f);
        public bool ownGravity = false;
        public Vector3 gravity = Physics.gravity;

        /// <summary>
        /// Applys an objects gravity to an objects movement.
        /// </summary>
        /// <param name="move">The movement vector</param>
        /// <returns> The movement after gravity</returns>
        public Vector3 applyGravity(Vector3 move) {
            Vector3 g = ownGravity ? gravity : Physics.gravity;
            return move + (g * Time.deltaTime);
        }
    }
}
