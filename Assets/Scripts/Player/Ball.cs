using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UI;
using Entity;

namespace Player {
    public class Ball : Item {
        // -------------------------- Unity Script Variables --------------------------
        
        /// <summary> A list of all tags that can trigger item pickup </summary>
        public List<string> pickUpTags = new List<string>() { "Player" };
        /// <summary> A list of all tags that can trigger item removal </summary>
        public List<string> removeTags = new List<string>() { "Item Remover" };
        /// <summary> The amount of thime before the player can pick up the item again. </summary>
        public float pickupDelay = 0.5f;
        /// <summary> The amount of thime before the ball can stick to a surface. </summary>
        public float stickyDelay = 0.1f;
        /// <summary> Flags the ball to stick to the first surface it touches. </summary>
        public bool sticky = false;
        /// <summary> The force the player throws the ball </summary>
        public Vector3 throwForce = new Vector3(0, 250, 500);
        /// <summary> The alpha value of the projection </summary>
        public Material projMat;
        /// <summary> The characteric of the ball's bounce. </summary>
        public float bounciness = 0.0f;
        /// <summary> The object throwing the ball </summary>
        public GameObject baseObj;
        /// <summary> The player's UI during gameplay </summary>
        public GameUI ui;

		public AudioClip ballClip, hitClip;

        // -------------------------- Class Variables --------------------------

        /// <summary> The rigid body that all physics is applied to </summary>
        private Rigidbody rigid;
        /// <summary> The stats for a ball gravity. </summary>
        private EntityStat stat;
        /// <summary> The ball who spawned this ball instance </summary>
        private Ball source;
        /// <summary> A reference to the 'Ghost' ball </summary>
        private Ball proj;
        /// <summary> Flags when the ball should be paused </summary>
        private bool paused;
        /// <summary> Saves ball body motion when paused </summary>
        private Vector3 saveVel, saveAngVel;

		private AudioSource MusicSource;

        // -------------------------- Player's Held Ball Functions --------------------------

        public Ball() {
            proj = null;
        }

        public override void Aim(InputType type) {
            if (type == InputType.HOLD && available && proj == null) {
                proj = CreateBall(string.Format("{0} Projection", gameObject.name));
                if (proj.gameObject.tag == "Heavy Trigger")
                    proj.gameObject.tag = "Heavy Projection";
                else
                    proj.gameObject.tag = "Projection";

                proj.GetComponent<Renderer>().material = projMat;
            }

            if (type == InputType.UP && proj != null)
                Destroy(proj.gameObject);
        }

        public override void Fire(InputType type) {
            if (type == InputType.UP && available) {
                if (proj != null)
                    Destroy(proj.gameObject);

                CreateBall(gameObject.name);
                available = false;
            }
        }

        /// <summary> Helper Method to spawn a Ball in the scene. </summary>
        private Ball CreateBall(string name) {
            alive = 0.0f;

            Ball b = Instantiate(this, transform.position, baseObj.transform.rotation);
            b.gameObject.SetActive(true);
            b.source = this;
            b.gameObject.name = name;
            return b;
        }

        // -------------------------- Player Created Ball Functions --------------------------
        // Note: 'source != null' is the proper check to determine if the ball was created


        // Use this for initialization
        void Start() {
            rigid = GetComponent<Rigidbody>();
            saveVel = Vector3.zero;
            saveAngVel = Vector3.zero;
            stat = GetComponent<EntityStat>();
			MusicSource = GetComponent<AudioSource>();
            if(GetComponent<Collider>() != null)
                GetComponent<Collider>().material.bounciness = bounciness;

            if (source != null) {
                source.alive = 0.0f;
                rigid.AddForce(transform.TransformDirection(throwForce));
            } else
                alive = respawnDelay;
			MusicSource.volume = 1f;
			MusicSource.clip = ballClip;
			MusicSource.Play();
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
            rigid.useGravity = stat == null || !stat.ownGravity;
            if (!rigid.useGravity) {
                rigid.useGravity = false;
                rigid.AddForce(rigid.mass * stat.gravity);
            }
        }

        void OnTriggerEnter(Collider coll) {
            if (source != null) {
                if (removeTags.Contains(coll.gameObject.tag) 
                     || (pickupDelay <= source.alive && pickUpTags.Contains(coll.gameObject.tag)))
                    Destroy(this.gameObject);
            }
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

        private void OnCollisionEnter(Collision collision) {
			MusicSource.volume /= 1.5f;
			MusicSource.clip = hitClip;
			MusicSource.Play();

            if(stickyDelay <= source.alive && sticky && rigid.useGravity) {
                stat.ownGravity = true;
                
                rigid.AddForce(-rigid.velocity, ForceMode.VelocityChange);
                rigid.AddTorque(-rigid.angularVelocity, ForceMode.VelocityChange);
                stat.gravity = Vector3.zero;
            }
        }
    }
}
