using System;
using System.Collections.Generic;
using UnityEngine;

namespace Player {
    /// <summary> Player Control Specification. Details the keyCode plus extra details about its game use. </summary>
    public class KeyControl : IEquatable<KeyControl>, IComparable<KeyControl> {
        /// <summary> Details the category of the player control. </summary>
        public string Section {
            get { return _sect; }
            set {
                // Manage the dictionary for controls.
                if (_added) {
                    // Remove old entry
                    if (_sect != null && Controls.controlSects.ContainsKey(_sect)) {
                        List<KeyControl> sect = Controls.controlSects[_sect];
                        sect.Remove(this);

                        if (sect.Count == 0)
                            Controls.controlSects.Remove(_sect);
                    }

                    // Add entry to new section
                    if (!Controls.controlSects.ContainsKey(value))
                        Controls.controlSects[value] = new List<KeyControl>();
                    Controls.controlSects[value].Add(this);
                }
                _sect = value;
            }
        }
        /// <summary> Details the default key code for the input. </summary>
        public KeyCode Def {
            get { return _def; }
        }
        /// <summary> Details the use of the input. </summary>
        public string name;
        /// <summary> Details the current key for the input./ </summary>
        public KeyCode key;

        /// <summary> Internal section </summary>
        private string _sect;
        /// <summary> Internal default key </summary>
        private KeyCode _def;
        /// <summary> Internal flag to identify inputs added to Controls.contolSects </summary>
        private bool _added;

        /// <summary> </summary>
        /// <param name="name">The named use for the control</param>
        /// <param name="key">The KeyCode to listen for</param>
        public KeyControl(string name, KeyCode key) : this("", name, key, false) { }

        /// <summary> </summary>
        /// <param name="sect">The section to add the control </param>
        /// <param name="name">The named use for the control</param>
        /// <param name="key">The Keycode to listen for</param>
        /// <param name="add">Flag to at it to Controls.contolSects</param>
        public KeyControl(string sect, string name, KeyCode key, bool add) {
            _added = add;
            Section = sect;
            this.key = key;
            this.name = name;
            _def = key;
        }

        /// <summary> Determines if the current KeyCode is the default KeyCode </summary>
        /// <returns>True if the current KeyCode is the same as the default KeyCode; Othwewise false.</returns>
        public bool IsDefault() { return _def == key; }

        public bool Equals(KeyControl other) {
            if (other == null) return false;

            if (!name.Equals(other.name)) return false;
            if (!key.Equals(other.key)) return false;
            if (!_sect.Equals(other._sect)) return false;
            if (!_def.Equals(other._def)) return false;
            if (!_added.Equals(other._added)) return false;

            return true;
        }

        public int CompareTo(KeyControl other) {
            if (other == null)
                return 1;

            return name.CompareTo(other.name);
        }
    }

    /// <summary> Details the universial controls used by the player. </summary>
    public class Controls {
        /// <summary> A container for registed controls sperated by sections. </summary>
        public static Dictionary<string, List<KeyControl>> controlSects = new Dictionary<string, List<KeyControl>>();

        /// <summary>/ Pauses and unpauses gameplay. </summary>
        public static KeyControl Pause = new KeyControl("", "Pause", KeyCode.Escape, true);
        /// <summary> Fires projectiles/ </summary>
        public static KeyControl Fire = new KeyControl("", "Fire", KeyCode.Mouse0, true);
        /// <summary> Aims projectiles </summary>
        public static KeyControl Aim = new KeyControl("", "Aim", KeyCode.Mouse1, true);
        /// <summary> Toggles the item selection wheel </summary>
        public static KeyControl ItemWheel = new KeyControl("", "Item Wheel", KeyCode.Q, true);

        /// <summary>/ Cancels a action or return to previous menu </summary>
        public static KeyControl Cancel = new KeyControl("Menu", "Cancel", KeyCode.Escape, true);
        /// <summary> Selects the selected menu item </summary>
        public static KeyControl Select = new KeyControl("Menu", "Select", KeyCode.Return, true);
        /// <summary> Moves selection up one item </summary>
        public static KeyControl SelectionUp = new KeyControl("Menu", "Selection Up", KeyCode.UpArrow, true);
        /// <summary> Moves selection down one item </summary>
        public static KeyControl SelectionDown = new KeyControl("Menu", "Selection Down", KeyCode.DownArrow, true);
        /// <summary> Selects item in slot 1 </summary>
        public static KeyControl ItemSlot1 = new KeyControl("Menu", "Item Slot 1", KeyCode.Alpha1, true);
        /// <summary> Selects item in slot 2 </summary>
        public static KeyControl ItemSlot2 = new KeyControl("Menu", "Item Slot 2", KeyCode.Alpha2, true);
        /// <summary>/ Selects item in slot 3 </summary>
        public static KeyControl ItemSlot3 = new KeyControl("Menu", "Item Slot 3", KeyCode.Alpha3, true);
    }
}
