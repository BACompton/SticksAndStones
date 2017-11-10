using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Puzzle {
    public class Wire : MonoBehaviour {
        // -------------------------- Unity Script Variables --------------------------

        /// <summary> The color the wires appears as when off. </summary>
        public Color off = new Color(0, 0, 0);
        /// <summary> The amount of time before the wire is fully active </summary>
        public float delay = 0; // TODO: Translate visually to Device

        // -------------------------- Class Variables --------------------------

        /// <summary> The button's input component. </summary>
        private PuzzleDevice device;
        /// <summary> Objects primary render material </summary>
        private Material mat;
        private float left;
        /// <summary> The target color </summary>
        private Color target, curr;
        /// <summary> The transition color </summary>
        private float r, g, b;

        // -------------------------- Unity Script Functions -------------------------- 

        // Use this for initialization
        void Start() {
            device = GetComponent<PuzzleDevice>();
            mat = GetComponent<Renderer>().material;

            left = delay;
            mat.color = off;
            device.trasmitColor = off;
            target = off;
        }

        // Update is called once per frame
        void Update() {
            if(device.update) {
                target = off;

                if (device.transmit)
                    target = device.trasmitColor;
                
                r = (target.r - mat.color.r) / delay;
                g = (target.g - mat.color.g) / delay;
                b = (target.b - mat.color.b) / delay;

                left = 0;
                curr = mat.color;
            }

            if (left < delay) {
                curr.r += (r * Time.deltaTime);
                curr.g += (g * Time.deltaTime);
                curr.b += (b * Time.deltaTime);

                mat.color = curr;
                left += Time.deltaTime;
            }  else
                mat.color = target;
            device.trasmitColor = mat.color;
        }
    }
}
