using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI {
    /// <summary>
    /// Generic Game UI interaction script.
    /// </summary>
    public class GameUI : MonoBehaviour {
        /// <summary>
        /// Flag to activate the interaction script
        /// </summary>
        public bool Active {
            get { return _active; }
            set {
                _active = value;
                Activate();
            }
        }
        /// <summary>
        /// Flag to render the UI
        /// </summary>
        public bool Render {
            get { return _render; }
            set {
                _render = value;
                Activate();
            }
        }

        /// <summary>
        /// The object that holds the UI manager script
        /// </summary>
        public Camera managerObj;
        /// <summary>
        /// The id for the UI
        /// </summary>
        protected string id;
        /// <summary>
        /// Flag to identify when the UI is transitioning
        /// </summary>
        protected bool transition;
        /// <summary>
        /// The UI manager pulled from managerObj
        /// </summary>
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
            transition = _active;
        }

        public virtual void TransitionTo() { }
        public virtual void TransitionFrom() { }

        /// <summary>
        /// Checks to see if ui is of the same type.
        /// </summary>
        /// <param name="id">The ui type to check against</param>
        /// <returns>True if they match, otherwise false.</returns>
        public bool IsID(string id) { return id.Equals(this.id); }
    }
}
