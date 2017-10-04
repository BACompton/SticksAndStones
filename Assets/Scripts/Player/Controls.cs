using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player {
    public class KeyControl {
        public string Section {
            get { return _sect; }
            set {
                // Manage the dictionary for controls.
                if(_added) {
                    // Remove old entry
                    if(_sect != null && Controls.controlSects.ContainsKey(_sect)) {
                        List<KeyControl> sect = Controls.controlSects[_sect];
                        sect.Remove(this);

                        if(sect.Count == 0)
                            Controls.controlSects.Remove(_sect);
                    }

                    // Add entry to new section
                    if(!Controls.controlSects.ContainsKey(value))
                        Controls.controlSects[value] = new List<KeyControl>();
                    Controls.controlSects[value].Add(this);
                }
                _sect = value;
            }
        }
        public KeyCode Def {
            get { return _def; }
        }
        public string name;
        public KeyCode key;
        

        private string _sect;
        private KeyCode _def;
        private bool _added;

        public KeyControl(string sect, string name, KeyCode key) : this(sect, name, key, false) {}

        public KeyControl(string sect, string name, KeyCode key, bool add) {
            _added = add;
            Section = sect;
            this.key = key;
            this.name = name;
            _def = key;
        }

        public bool IsDefault() { return _def == key; }
    }

    public class Controls {
        public static Dictionary<string, List<KeyControl>> controlSects = new Dictionary<string, List<KeyControl>>();
        
        /// <summary>
        /// Control to pause and unpause gameplay.
        /// </summary>
        public static KeyControl Pause = new KeyControl("", "Pause", KeyCode.Escape, true);
        public static KeyControl Fire = new KeyControl("", "Fire", KeyCode.Mouse0, true);
        public static KeyControl ItemWheel = new KeyControl("", "Item Wheel", KeyCode.Q, true);

        public static KeyControl Cancel = new KeyControl("Menu", "Cancel", KeyCode.Escape, true);
        public static KeyControl Select = new KeyControl("Menu", "Select", KeyCode.Return, true);
        public static KeyControl SelectionUp = new KeyControl("Menu", "Selection Up", KeyCode.UpArrow, true);
        public static KeyControl SelectionDown = new KeyControl("Menu", "Selection Down", KeyCode.DownArrow, true);
        public static KeyControl ItemSlot1 = new KeyControl("Menu", "Item Slot 1", KeyCode.Alpha1, true);
        public static KeyControl ItemSlot2 = new KeyControl("Menu", "Item Slot 2", KeyCode.Alpha2, true);
        public static KeyControl ItemSlot3 = new KeyControl("Menu", "Item Slot 3", KeyCode.Alpha3, true);
    }
}
