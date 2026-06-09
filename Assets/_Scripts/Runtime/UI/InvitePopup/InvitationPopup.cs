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

        private RectTransform _popupRect;
        private CanvasGroup _popupGroup;
        private Sequence _mySequence;
        private float _duration = 1f;
        private float _hidePositionX = 1000f; 
        private float _showPositionX = -50f;
        private string _inviterNickname = null;
        private Vector2 _hiddenPos;
        private Vector2 _shownPos;

        private void Awake()
        {
            Initialize();
        }

        public void Initialize()
        {
            _popupRect = GetComponent<RectTransform>();
            _popupGroup = GetComponent<CanvasGroup>();
            _mySequence = DOTween.Sequence();

            _hiddenPos = new Vector2(_hidePositionX, _popupRect.anchoredPosition.y);
            _shownPos = new Vector2(_showPositionX, _popupRect.anchoredPosition.y);

            NetworkManager.Instance.Lobby.MatchMakingRoomSomeoneInvited += HandleMatchMakingRoomSomeoneInvited;
            btn_accept.onClick.AddListener(() => NetworkManager.Instance.Lobby.RespondToRoomInvitation(_inviterNickname, true));
            btn_decline.onClick.AddListener(() => NetworkManager.Instance.Lobby.RespondToRoomInvitation(_inviterNickname, false));
            
            _popupRect.anchoredPosition = _hiddenPos;
            _popupGroup.alpha = 0;
            _popupGroup.interactable = false;
        }

        public void HandleMatchMakingRoomSomeoneInvited(string userName)
        {
            txt_inviteMessage.text = $"{userName}(이)가 당신에게 초대를 보냈습니다!";

            ShowPopup();
        }

        [ContextMenu("Show Popup")]
        public void ShowPopup()
        {
            _popupRect.DOKill();

            _mySequence
                .Join(_popupGroup.DOFade(1f, 1))
                .Join(_popupRect.DOAnchorPos(_shownPos, _duration)
                .SetEase(Ease.OutBack)
                .OnComplete(()=> _popupGroup.interactable = true));
        }

        [ContextMenu("Hide Popup")]
        public void HidePopup()
        {
            _popupGroup.interactable = false;
            _popupRect.DOKill();

            _mySequence.Join(_popupGroup.DOFade(0f, 1))
                .Join(_popupRect.DOAnchorPos(_hiddenPos, _duration)
                .SetEase(Ease.InBack));        
        }
    }
}