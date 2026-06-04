using Utility.Debug;
using BackEnd;
using UnityEngine;

namespace Runtime.Shared
{
    public class BackendManager : MonoBehaviour
    {
        public void Initialize()
        {
            BackendReturnObject bro = Backend.Initialize();

            if (bro.IsSuccess())
            {
                CustomLog.LogSuccess("초기화 성공 : " + bro);
            }
            else
            {
                CustomLog.LogError("초기화 실패 : " + bro);
            }
        }
    }
}
