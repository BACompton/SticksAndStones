using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Player;

namespace UI {
    public class ControlsUI : GameUI {
        public static string ID = "Controls";

        public GameObject controls;
        public Scrollbar bar;

        private const string SECT = "UI/KeySection", KEY = "UI/KeyControl";

        private bool listen, listenFilp;
        private GameObject src, reset;
        private KeyControl key, rec;

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

            if(!active || listen)
                return;

            Cursor.lockState = CursorLockMode.None;

            // UI Transitions
            if(Input.GetKeyUp(Controls.Cancel.key))
                Exit();
        }

        void OnGUI() {
            if(!active)
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

                    if(k == Controls.Cancel.key)
                        k = key.key;
                    else {
                        key.key = k;
                        reset.GetComponent<Button>().interactable = !key.IsDefault();
                    }

                    src.GetComponentInChildren<Text>().text = k.ToString();
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

        /// <summary>
        /// Reloads the Control UI menu.
        /// </summary>
        public void LoadControls() {
            GameObject sectPre = (GameObject)Resources.Load(SECT), keyPre = (GameObject)Resources.Load(KEY);

            controls.transform.DetachChildren();
            foreach(string sect in Controls.controlSects.Keys) {
                GameObject sectObj = Instantiate(sectPre),
                    sectLabel = sectObj.transform.Find("Label").gameObject;

                // SetUp control section in UI
                sectObj.transform.name = string.Format(format:"Section {0}", arg0: sect);
                sectObj.transform.SetParent(controls.transform);

                if(sect != "")
                    sectLabel.GetComponent<Text>().text = sect;
                else
                    sectLabel.SetActive(false);

                foreach(KeyControl k in Controls.controlSects[sect]) {
                    GameObject kObj = Instantiate(keyPre),
                        kLabel = kObj.transform.Find("Label").gameObject,
                        kKey = kObj.transform.Find("Btns").Find("Key").gameObject,
                        kReset = kObj.transform.Find("Btns").Find("Reset").gameObject;

                    // Set the text for a KeyControl UI segment
                    kObj.transform.name = string.Format(format: "KeyControl {0}", arg0: k.name);
                    kObj.transform.SetParent(sectObj.transform);
                    kLabel.GetComponent<Text>().text = k.name;
                    kKey.GetComponentInChildren<Text>().text = k.key.ToString();

                    // Add button listeners
                    kKey.GetComponent<Button>().onClick.AddListener(delegate { KeyListen(kKey, kReset, k); });
                    kReset.GetComponent<Button>().onClick.AddListener(delegate { Reset(kKey, kReset, k); });

                    kReset.GetComponent<Button>().interactable = !k.IsDefault();
                }
            }
        }

        /// <summary>
        /// Sets the id for the UI canvas
        /// </summary>
        public override void SetId() { id = ID; }

        /// <summary>
        /// Helper to toggle the listening functionality of the UI on
        /// </summary>
        /// <param name="src">The input button pressed</param>
        /// <param name="reset">The reset button for the input</param>
        /// <param name="key">The input being changed</param>
        private void KeyListen(GameObject src, GameObject reset, KeyControl key) {
            if(!listen) {
                src.GetComponentInChildren<Text>().text = "...";
                listen = true;
                this.src = src;
                this.reset = reset;
                this.key = key;
            }
        }

        /// <summary>
        /// Helper to reset an input
        /// </summary>
        /// <param name="src">The input button to reset</param>
        /// <param name="reset">The reset button for the input</param>
        /// <param name="key">The input being reset</param>
        private void Reset(GameObject src, GameObject reset, KeyControl key) {
            if(!listen) {
                key.key = key.Def;
                src.GetComponentInChildren<Text>().text = key.key.ToString();
                reset.GetComponent<Button>().interactable = !key.IsDefault();
            }
        }

        /// <summary>
        /// Helper transition to Exit UI
        /// </summary>
        private void Exit() {
            if(!listen) {
                manager.Transition(this, Pause.ID, false);
            }
        }
    }
}
