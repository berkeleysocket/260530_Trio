using TMPro;
using UnityEngine;

namespace Runtime.UI
{
    public class RoomUserElementUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text txt_visitorNickname;
        
        public string VisitorNickname { get; private set; }

        public void Initialize(string name)
        {
            this.VisitorNickname = name;
            this.txt_visitorNickname.text = name;
        }
    }
}