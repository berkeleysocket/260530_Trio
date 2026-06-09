using TMPro;
using UnityEngine;

namespace Runtime.UI
{
    public class MatchMakingUserElementUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text txt_UserName;
        [SerializeField] private TMP_Text txt_UserStatus;
        
        public string UserName { get; private set; }
        public string UserStatus { get; private set; }

        public void Initialize(string name, string status)
        {
            this.UserName = name;
            this.UserStatus = status;
            this.txt_UserName.text = name;
            this.txt_UserStatus.text = status;
        }
    }
}