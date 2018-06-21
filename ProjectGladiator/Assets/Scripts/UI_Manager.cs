using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Gladiatorz {
    public class UI_Manager : MonoBehaviour {
        private static UI_Manager instance;

        public RectTransform healthRect;

        public Text statTextBox;

        public static UI_Manager Instance {
            get { return instance; }
        }

        public void Awake() {
            if (instance != null) {
                GameObject.Destroy(this.gameObject);
                return;
            }

            instance = this;
            Init();
        }

        public void Init() {
            StartCoroutine(UpdateUICall());
        }

        public IEnumerator UpdateUICall() {           
            while (true) {
                yield return new WaitForSeconds(0.025f);
                PollServerStatus();
            }
        }

        //TODO: Move this functionality to an outside class
        public void SetHealthRect(float percent) {
            healthRect.localScale = new Vector3(percent, 1.0f, 1.0f);
        }

        public void PollServerStatus() {
            ClientIntermediate.Instance.RequestMatchData();
        }

        public void UpdateStatTextBox(string matchStatisticsStrings) {
            statTextBox.text = matchStatisticsStrings;
        }
    }
}