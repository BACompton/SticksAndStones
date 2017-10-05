using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UI;
using Entity;

namespace Player {
    public class Ball : Item {
        /// <summary> The amount of thime before the player can pick up the item again. </summary>
        public float pickupDelay = 0.5f;
        /// <summary> The force the player throws the ball </summary>
        public Vector3 throwForce = new Vector3(0, 250, 500);
        public float projAlpha = 0.5f;
        /// <summary> The object throwing the ball </summary>
        public GameObject baseObj;
        /// <summary> The player's UI during gameplay </summary>
        public GameUI ui;

        /// <summary> The rigid body that all physics is applied to </summary>
        private Rigidbody rigid;

        private EntityStat stat;
        /// <summary> The ball who spawned this ball instance </summary>
        private Ball source;
        /// <summary> A reference to the 'Ghost' ball </summary>
        private Ball proj;
        /// <summary> Flags when the ball should be paused </summary>
        private bool paused;
        /// <summary> Saves ball body motion when paused </summary>
        private Vector3 saveVel, saveAngVel;

        public Ball() {
            proj = null;
        }

        public override void Aim(InputType type) {
            if (type == InputType.HOLD && available && proj == null) {
                proj = CreateBall("Ball Projection");

                Color c = proj.GetComponent<Renderer>().material.color;
                c.a = projAlpha;
                proj.GetComponent<Renderer>().material.color = c;
            }

            if (type == InputType.UP && proj != null)
                Destroy(proj.gameObject);
        }

        public override void Fire(InputType type) {
            if (type == InputType.UP && available) {
                if (proj != null)
                    Destroy(proj.gameObject);

                Ball b = CreateBall("Ball");
                available = false;
            }
        }

        // Use this for initialization
        void Start() {
            rigid = GetComponent<Rigidbody>();
            saveVel = Vector3.zero;
            saveAngVel = Vector3.zero;
            stat = GetComponent<EntityStat>();

            if (source != null) {
                source.alive = 0.0f;
                rigid.AddForce(transform.TransformDirection(throwForce));
            } else
                alive = respawnDelay;
        }

        // Update is called once per frame
        void Update() {
           if (paused)
                return;

            // Respawn timer for spawned balls.
            if (source != null) {
                source.alive += Time.deltaTime;

                if (source.alive >= respawnDelay)
                    Destroy(this.gameObject);
            }
        }

        void FixedUpdate() {
            bool p = ui != null ? !ui.Active : false, 
                save = !paused ? p : false,
                applySave = paused ? !p : false;
            paused = p;
            
            // Save ball's movement
            if (save) {
                saveVel = rigid.velocity;
                saveAngVel = rigid.angularVelocity;
                rigid.isKinematic = true;
            }

            // Load ball's movement
            if (applySave) {
                rigid.isKinematic = false;
                rigid.AddForce(saveVel, ForceMode.VelocityChange);
                rigid.AddTorque(saveAngVel, ForceMode.VelocityChange);
                
            }

            // Apply own Movement
            if (stat != null && stat.ownGravity) {
                rigid.useGravity = false;
                rigid.AddForce(rigid.mass * stat.gravity);
            }
        }

        void OnTriggerEnter(Collider coll) {
            if (pickupDelay <= source.alive && coll.gameObject.tag == "Player")
                Destroy(this.gameObject);
        }

        void OnDestroy() {
            if (source != null) {
                source.alive = source.respawnDelay;
                if (source.proj != null)
                    source.proj = null;
                else 
                    source.available = true;
            }
        }

        /// <summary>
        /// Helper Method to spawn a Ball in the scene.
        /// </summary>
        private Ball CreateBall(string name) {
            alive = 0.0f;

            Ball b = Instantiate(this, transform.position, baseObj.transform.rotation);
            b.gameObject.SetActive(true);
            b.source = this;
            b.gameObject.name = name;
            return b;
        }
    }
}
