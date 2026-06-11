using Runtime.Pattern;
using System.Linq;
using UnityEngine;
using Utility.Debug;

namespace Runtime.UI
{
    public class UIManager : MonoSingleton<UIManager>
    {
        public void Initialize()
        {
            Canvas[] canvases = FindObjectsByType<Canvas>(FindObjectsSortMode.None);

            foreach(var canvas in canvases)
            {
                Window[] windows = canvas.GetComponentsInChildren<Window>(true);

                foreach(var window in windows)
                    window.Initialize();
                
                CustomLog.Assert(windows != null && windows.Count() > 0, "windows is not found");
            }

            CustomLog.Assert(canvases != null && canvases.Count() > 0, "canvases is not found");
        }
    }
}
