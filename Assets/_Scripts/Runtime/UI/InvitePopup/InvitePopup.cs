using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.UI
{
    public class InvitePopup : MonoBehaviour
    {
        [SerializeField] private TMP_Text txt_inviteMessage;
        [SerializeField] private Button btn_accept;
        [SerializeField] private Button btn_decline;

        public void Initialize()
        {

        }

        public void SetMessage(string userName)
        {
            txt_inviteMessage.text = $"{userName}(이)가 당신에게 초대를 보냈습니다!";
        }
    }
}
