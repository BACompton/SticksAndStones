using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    /// <summary> Interface for an interactable object. </summary>
    public class Interactable : MonoBehaviour
    {
        // -------------------------- Player Interactable Behavior Functions --------------------------

        /// <summary> Method used when the player presses the interact key </summary>
        public virtual void Interact() { }
    }
}
