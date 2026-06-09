using UnityEngine;

namespace Runtime.UI
{
    public class AccountPage : Page
    {
        [field: SerializeField] public SignUpUI signUI { get; private set; }
        [field: SerializeField] public LoginUI loginUI { get; private set; }
    }
}
