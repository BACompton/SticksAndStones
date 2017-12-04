using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Puzzle {
    /// <summary> Detatils how a device should translate input signals to output signals </summary>
    public enum DeviceBehavior { STATIC, SINGLE, AND, OR }

    /// <summary> Details the behaviors of a puzzle device. </summary>
    public class PuzzleDevice : MonoBehaviour {
        // -------------------------- Unity Script Variables --------------------------

        /// <summary> Is the device active internally </summary>
        public bool active = false;
        /// <summary> Is the device transmitting </summary>
        public bool transmit = false;
        /// <summary> The color of the transmition </summary>
        public Color trasmitColor = new Color(1, 1, 1);
        /// <summary> Flags the device evaluate the inputs </summary>
        public bool update = false;
        /// <summary> A list of all the device's inputs. </summary>
        public List<PuzzleDevice> inputs;
        /// <summary> A list of all the device's outputs. </summary>
        public List<PuzzleDevice> outputs;
        /// <summary> Details how the device should translate inputs to outputs </summary>
        public DeviceBehavior transmitMode = DeviceBehavior.STATIC;
        /// <summary> Details how the device should translate input colors to output colors </summary>
        public DeviceBehavior filterMode = DeviceBehavior.STATIC;

        private bool queueUpdate = false;

        // -------------------------- Unity Functions --------------------------

        void Start() {
            // Register device outputs as a output's input device
            foreach (PuzzleDevice output in outputs)
                if(!output.inputs.Contains(this))
                    output.inputs.Add(this);
        }

        void Update() {
            if(queueUpdate) {
                queueUpdate = false;
                update = false;
            }

            if(update) {
                bool prevXmit = transmit;
                transmit = IsEnabled() && active;

                if (transmit)
                    trasmitColor = FilterColor();

                // Signal outputs to update
                if(prevXmit != transmit)
                    foreach (PuzzleDevice device in outputs)
                        device.update = true;
                queueUpdate = true;
            }
        }

        // -------------------------- Helper Functions --------------------------

        /// <summary> Determines if the device is enabled according to its inputs and the devices behavior mode. </summary>
        /// <returns> True if the device is enabled; Otherwise false.</returns>
        public bool IsEnabled() {
            switch (transmitMode) {
                case DeviceBehavior.STATIC:
                    return true;
                case DeviceBehavior.SINGLE:
                    if (inputs.Count > 0)
                        return inputs[0].transmit;
                    return false;
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

        /// <summary> Determines the filtered transmit color. </summary>
        /// <returns> The filtered color </returns>
        private Color FilterColor() {
            Color c = new Color(0, 0, 0);

            switch (filterMode) {
                case DeviceBehavior.STATIC:
                    c = trasmitColor; break;
                case DeviceBehavior.SINGLE:
                    if (inputs.Count > 0 && inputs[0].transmit)
                        c = inputs[0].trasmitColor;
                    break;
                case DeviceBehavior.AND:
                    c = new Color(1, 1, 1);

                    foreach (PuzzleDevice input in inputs)
                        if (input.transmit) {
                            c.r = ((int)(c.r * 255) & (int)(input.trasmitColor.r * 255)) / 255;
                            c.g = ((int)(c.g * 255) & (int)(input.trasmitColor.g * 255)) / 255;
                            c.b = ((int)(c.b * 255) & (int)(input.trasmitColor.b * 255)) / 255;
                        }
                    break;
                case DeviceBehavior.OR:
                    foreach (PuzzleDevice input in inputs)
                        if (input.transmit) {
                            c.r = ((int)(c.r * 255) | (int)(input.trasmitColor.r * 255)) / 255;
                            c.g = ((int)(c.g * 255) | (int)(input.trasmitColor.g * 255)) / 255;
                            c.b = ((int)(c.b * 255) | (int)(input.trasmitColor.b * 255)) / 255;
                        }
                    break;
            }
            return c;
        }
    }
}
