using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Player;

namespace UI {
    /// <summary> Controls UI Interaction script </summary>
    public class ControlsUI : GameUI {
        /// <summary> Player UI ID used to identify a player UI script. </summary>
        public static string ID = "Controls";

        /// <summary> Place where all the controls should be loaded </summary>
        public GameObject controls;

        /// <summary> Paths to control UI resources. </summary>
        private const string SECT = "UI/KeySection", KEY = "UI/KeyControl";
        /// <summary> Formats for GameObject names </summary>
        private const string SECT_NAME = "Section {0}", KEY_NAME = "KeyControl {0}";

        /// <summary> Flags to toggle the rebinding listening. </summary>
        private bool listen, listenFilp;
        /// <summary> Buttons currently being rebound </summary>
        private GameObject src, reset;
        /// <summary> The key pressed during rebinding listening phase </summary>
        private KeyControl key;

        // Use this for initialization
        new void Start() {
            base.Start();
            SetId();

            GameObject done = transform.Find("BG").Find("Done").gameObject;
            done.GetComponent<Button>().onClick.AddListener(Exit);
        }

        // Update is called once per frame
        void Update() {
            if(transition) {
                transition = !transition;
                controls.SetActive(false);
                LoadControls();
                controls.SetActive(true);
            }

            if(listenFilp) {
                listen = !listen;
                listenFilp = !listenFilp;
                return;
            }

            if(!Active || listen)
                return;

            Cursor.lockState = CursorLockMode.None;

            // UI Transitions
            if(Input.GetKeyUp(Controls.Back.key))
                Exit();
        }

        void OnGUI() {
            if(!Active)
                return;

            // Listen for next Button press
            if(listen) {
                Event e = Event.current;
                KeyCode k = KeyCode.None;
                
                if(e.type == EventType.KeyUp)
                    k = e.keyCode;
                if(e.type == EventType.MouseUp) {
                    switch(e.button) {
                        case 0: k = KeyCode.Mouse0; break;
                        case 1: k = KeyCode.Mouse1; break;
                        case 2: k = KeyCode.Mouse2; break;
                        case 3: k = KeyCode.Mouse3; break;
                        case 4: k = KeyCode.Mouse4; break;
                        case 5: k = KeyCode.Mouse5; break;
                        case 6: k = KeyCode.Mouse6; break;
                    }
                }

                // If something was pressed, customize key binding.
                if(k != KeyCode.None) {
                    listenFilp = true;

                    if(k == Controls.Back.key)
                        k = key.key;
                    else {
                        key.key = k;
                        UnBind(key);
                    }

                    UpdateKeyBtns(key, src, reset, k.ToString());
                    return;
                }
            }
        }

        void OnEnable() {
            listen = false;
            listenFilp = false;
            src = null;
            key = null;
        }

        /// <summary> Reloads the Control UI menu. </summary>
        public void LoadControls() {
            GameObject sectPre = (GameObject)Resources.Load(SECT), keyPre = (GameObject)Resources.Load(KEY);

            controls.transform.DetachChildren();
            foreach(string sect in Controls.controlSects.Keys) {
                GameObject sectObj = Instantiate(sectPre),
                    sectLabel = sectObj.transform.Find("Label").gameObject;

                // SetUp control section in UI
                sectObj.transform.name = string.Format(format: SECT_NAME, arg0: sect);
                sectObj.transform.SetParent(controls.transform);

                if(sect != "")
                    sectLabel.GetComponent<Text>().text = sect;
                else
                    sectLabel.SetActive(false);

                Controls.controlSects[sect].Sort();
                foreach(KeyControl k in Controls.controlSects[sect]) {
                    GameObject kObj = Instantiate(keyPre),
                        kLabel = kObj.transform.Find("Label").gameObject,
                        kKey = FindKeyButton(kObj.transform),
                        kReset = FindResetButton(kObj.transform);

                    // Set the text for a KeyControl UI segment
                    kObj.transform.name = string.Format(format: KEY_NAME, arg0: k.name);
                    kObj.transform.SetParent(sectObj.transform);
                    kLabel.GetComponent<Text>().text = k.name;
                    UpdateKeyBtns(k, kKey, kReset, k.key.ToString());

                    // Add button listeners
                    kKey.GetComponent<Button>().onClick.AddListener(delegate { KeyListen(kKey, kReset, k); });
                    kReset.GetComponent<Button>().onClick.AddListener(delegate { Reset(kKey, kReset, k); });
                }
            }
        }

        /// <summary> Sets the id for the UI canvas </summary>
        public override void SetId() { id = ID; }

        /// <summary> Helper to toggle the listening functionality of the UI on </summary>
        /// <param name="src">The input button pressed</param>
        /// <param name="reset">The reset button for the input</param>
        /// <param name="key">The input being changed</param>
        private void KeyListen(GameObject src, GameObject reset, KeyControl key) {
            if(!listen) {
                UpdateKeyBtns(key, src, reset, "...");
                listen = true;
                this.src = src;
                this.reset = reset;
                this.key = key;
            }
        }

        /// <summary> Helper to reset an input </summary>
        /// <param name="src">The input button to reset</param>
        /// <param name="reset">The reset button for the input</param>
        /// <param name="key">The input being reset</param>
        private void Reset(GameObject src, GameObject reset, KeyControl key) {
            if(!listen) {
                key.key = key.Def;
                UnBind(key);
                UpdateKeyBtns(key, src, reset, key.key.ToString());
            }
        }

        /// <summary> Updates the button for the specified KeyControl </summary>
        /// <param name="src">The key button</param>
        /// <param name="reset">The reset button</param>
        /// <param name="display">The display for the key button</param>
        private void UpdateKeyBtns(KeyControl key, GameObject src, GameObject reset, string display) {
            src.GetComponentInChildren<Text>().text = display;
            reset.GetComponent<Button>().interactable = !key.IsDefault();
        }

        /// <summary> Helper method to unbind any conflicting key binding. </summary>
        /// <param name="key"></param>
        private void UnBind(KeyControl key) {
            GameObject sect = GameObject.Find(string.Format(SECT_NAME, key.Section));

            foreach(KeyControl k in Controls.controlSects[key.Section])
                if(k != key && k.key == key.key) {
                    Transform keySect = sect.transform.Find(string.Format(KEY_NAME, k.name));
                    GameObject keyGO = FindKeyButton(keySect),
                        resetGO = FindResetButton(keySect);

                    k.key = KeyCode.None;
                    UpdateKeyBtns(k, keyGO, resetGO, k.key.ToString());
                }
        }

        /// <summary> Locates the key button in a KeyContorl prefab group. </summary>
        /// <param name="keySect">The KeyControl to search</param>
        /// <returns>The key button's game object</returns>
        private GameObject FindKeyButton(Transform keySect) {
            return keySect.Find("Btns").Find("Key").gameObject;
        }

        /// <summary> Locates the reset button in a KeyContorl prefab group. </summary>
        /// <param name="keySect">The KeyControl to search</param>
        /// <returns>The reset button's game object</returns>
        private GameObject FindResetButton(Transform keySect) {
            return keySect.Find("Btns").Find("Reset").gameObject;
        }

        /// <summary> Helper transition to Exit UI </summary>
        private void Exit() {
            if(!listen) {
                Manager.Transition(this, Pause.ID, false);
            }
        }
    }
}
