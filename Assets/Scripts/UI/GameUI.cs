using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI {
    public class GameUI : MonoBehaviour {
        public bool active {
            get { return _active; }
            set {
                _active = value;
                Activate();
            }
        }
        public bool render {
            get { return _render; }
            set {
                _render = value;
                Activate();
            }
        }

        public Camera managerObj;
        protected string id;
        protected UIManager manager;

        private bool _active, _render;

        protected void Start() {
            id = "";
            manager = managerObj.GetComponent<UIManager>();    
        }

        /// <summary>
        /// Sets the id for the UI canvas
        /// </summary>
        public virtual void SetId() { id = ""; }

        /// <summary>
        /// Helper method to activate the ui canvas when necessary.
        /// </summary>
        public void Activate() {
            gameObject.SetActive(_active || _render); 
        }

        /// <summary>
        /// Checks to see if ui is of the same type.
        /// </summary>
        /// <param name="id">The ui type to check against</param>
        /// <returns>True if they match, otherwise false.</returns>
        public bool IsID(string id) { return id.Equals(this.id); }
    }
}
