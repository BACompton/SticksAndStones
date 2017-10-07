using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Puzzle {
    /// <summary> A generic puzzle output </summary>
    public class PuzzleOutput : MonoBehaviour {
        // -------------------------- Unity Script Variables --------------------------

        /// <summary> A list of all puzzle inputs. </summary>
        public List<PuzzleInput> inputs;

        // -------------------------- Helpers --------------------------

        /// <summary> Helper method to determine if all the input are active. </summary>
        /// <returns>True if all inputs are active, otherwise false</returns>
        public bool AllActivated() {
            foreach (PuzzleInput pIn in inputs)
                if (!pIn.active)
                    return false;
            return true;
        }
    }
}
