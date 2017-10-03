using UnityEngine;
using Entity;

namespace Player {
    public class PlayerStat : EntityStat {
        public Vector3 jumpSpeed = new Vector3(0.0f, 2.0f, 0.0f);
        public Vector2 rotSensitivity = new Vector3(1.0f, 1.0f);
    }
}
