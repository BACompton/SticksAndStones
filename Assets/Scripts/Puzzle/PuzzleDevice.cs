using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Puzzle {
    /// <summary> Detatils how a device should translate input signals to output signals </summary>
    public enum DeviceBehavior { AND, OR }

    /// <summary> Details the behaviors of a puzzle device. </summary>
    public class PuzzleDevice : MonoBehaviour {
        // -------------------------- Unity Script Variables --------------------------

        /// <summary> Is the device active internally </summary>
        public bool active = false;
        /// <summary> Is the device transmitting </summary>
        public bool transmit = false;
        /// <summary> Flags the device evaluate the inputs </summary>
        public bool update = false;
        /// <summary> A list of all the device's inputs. </summary>
        public List<PuzzleDevice> inputs;
        /// <summary> A list of all the device's outputs. </summary>
        public List<PuzzleDevice> outputs;
        /// <summary> Details how the device should translate inputs to outputs </summary>
        public DeviceBehavior mode = DeviceBehavior.AND;

        // -------------------------- Unity Functions --------------------------

        void Start() {
            // Register device outputs as a output's input device
            foreach (PuzzleDevice output in outputs)
                if(!output.inputs.Contains(this))
                    output.inputs.Add(this);
        }

        void Update() {
            if(update) {
                bool prevXmit = transmit;
                transmit = IsEnabled() && active;
                
                // Signal outputs to update
                if(prevXmit != transmit)
                    foreach (PuzzleDevice device in outputs)
                        device.update = true; // TODO: Make some way to stop cycle
                update = false;
            }
        }

        // -------------------------- Helper Functions --------------------------

        /// <summary> Determines if the device is enabled according to its inputs and the devices behavior mode. </summary>
        /// <returns> True if the device is enabled; Otherwise false.</returns>
        private bool IsEnabled() {
            switch (mode) {
                case DeviceBehavior.AND:
                    foreach (PuzzleDevice input in inputs)
                        if (!input.transmit)
                            return false;
                    return true;
                case DeviceBehavior.OR:
                    foreach (PuzzleDevice input in inputs)
                        if (input.transmit)
                            return true;
                    return false;
                default: return false;
            }
        }
    }
}
