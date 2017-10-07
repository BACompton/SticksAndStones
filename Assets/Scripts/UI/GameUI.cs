using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI {
    /// <summary> Generic Game UI interaction script. </summary>
    public class GameUI : MonoBehaviour {
        // -------------------------- Class Properties --------------------------

        /// <summary> Flag to activate the interaction script </summary>
        public bool Active {
            get { return m_active; }
            set {
                m_active = value;
                Activate();
            }
        }
        /// <summary> Flag to render the UI </summary>
        public bool Render {
            get { return m_render; }
            set {
                m_render = value;
                Activate();
            }
        }
        /// <summary> The UI manager this UI is tied </summary>
        public UIManager Manager {
            get { return m_manger; }
            set { m_manger = value; }
        }

        // -------------------------- Class Variables --------------------------

        /// <summary> The id for the UI </summary>
        protected string id;
        /// <summary> Flag to identify when the UI is transitioning </summary>
        protected bool transition;

        /// <summary> Internal UI manager </summary>
        private UIManager m_manger;
        /// <summary> Internal flags </summary>
        private bool m_active, m_render;

        // -------------------------- Class Functions --------------------------

        protected void Start() {
            id = "";
        }

        /// <summary> Sets the id for the UI canvas </summary>
        public virtual void SetId() { id = ""; }

        /// <summary> Helper method to activate the ui canvas when necessary. </summary>
        public void Activate() {
            gameObject.SetActive(m_active || m_render);
            transition = m_active;
        }

        public virtual void TransitionTo() { }
        public virtual void TransitionFrom() { }

        /// <summary> Checks to see if ui is of the same type. </summary>
        /// <param name="id">The ui type to check against</param>
        /// <returns>True if they match, otherwise false.</returns>
        public bool IsID(string id) { return id.Equals(this.id); }
    }
}
