using DG.Tweening;
using Runtime.Shared.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.UI
{
    public class InvitationPopup : MonoBehaviour
    {
        [SerializeField] private TMP_Text txt_inviteMessage;
        [SerializeField] private Button btn_accept;
        [SerializeField] private Button btn_decline;

        private string _inviterNickname = null;

        //
        [SerializeField] private RectTransform popupRect;

        [SerializeField] private float duration = 0.4f;
        [SerializeField] private float hidePositionX = -1200f; // 화면 왼쪽 밖 위치
        [SerializeField] private float showPositionX = 0f;    // 화면 중앙 위치

        private Vector2 _hiddenPos;
        private Vector2 _shownPos;

        private void Awake()
        {
            Initialize();

            if (popupRect == null) popupRect = GetComponent<RectTransform>();

            // 위치 벡터 미리 캐싱
            _hiddenPos = new Vector2(hidePositionX, popupRect.anchoredPosition.y);
            _shownPos = new Vector2(showPositionX, popupRect.anchoredPosition.y);

            // 시작할 때는 화면 밖에 숨겨두기
            popupRect.anchoredPosition = _hiddenPos;
            gameObject.SetActive(false);
        }

        public void Initialize()
        {
            NetworkManager.Instance.Lobby.MatchMakingRoomSomeoneInvited += (inviterNickname) => this._inviterNickname = inviterNickname;
            btn_accept.onClick.AddListener(() => NetworkManager.Instance.Lobby.RespondToRoomInvitation(_inviterNickname, true));
            btn_decline.onClick.AddListener(() => NetworkManager.Instance.Lobby.RespondToRoomInvitation(_inviterNickname, false));
        }

        public void SetMessage(string userName)
        {
            txt_inviteMessage.text = $"{userName}(이)가 당신에게 초대를 보냈습니다!";
        }

        [ContextMenu("Show Popup")]
        public void ShowPopup()
        {
            gameObject.SetActive(true);

            popupRect.DOKill();
            popupRect.anchoredPosition = _hiddenPos;

            popupRect.DOAnchorPos(_shownPos, duration)
                .SetEase(Ease.OutBack)
                .SetUpdate(true);
        }

        [ContextMenu("Hide Popup")]
        public void HidePopup()
        {
            popupRect.DOKill();

            popupRect.DOAnchorPos(_hiddenPos, duration)
                .SetEase(Ease.InQuad)
                .SetUpdate(true);
        }
    }
}