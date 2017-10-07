using UnityEngine;
using Entity;

namespace Player {
    /// <summary>Holder stats for an player </summary>
    public class PlayerStat : EntityStat {
        // -------------------------- Unity Script Variables --------------------------

        /// <summary> The jump speed for a player </summary>
        public Vector3 jumpSpeed = new Vector3(0.0f, 2.0f, 0.0f);
        /// <summary> The players camara control sensitivity </summary>
        public Vector2 rotSensitivity = new Vector3(1.0f, 1.0f);
    }
}
