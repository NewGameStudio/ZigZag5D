using System;
using UnityEngine;

namespace UICore
{
    public class UIEvents : MonoBehaviour
    {
        public static event Action onPausePressed;
        public static event Action onPlayPressed;
        public static event Action onQuitPressed;

        public void OnPausePressed()
        {
            onPausePressed?.Invoke();
        }

        public void OnPlayPressed()
        {
            onPlayPressed?.Invoke();
        }

        public void OnQuitPressed()
        {
            onQuitPressed?.Invoke();
        }
    }
}
