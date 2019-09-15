using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Advertising
{
    public class AdManager : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("In minutes")]
        private float _adShowInterval = 5;
        private float _currentTime;

        private void Awake()
        {
            _currentTime = _adShowInterval * 60;

            FindObjectOfType<GameMaster>().onLoseGame += OnLoseGame;
        }

        private void Update()
        {
            _currentTime -= Time.deltaTime;
        }

        private void OnLoseGame()
        {
            if (_currentTime > 0)
                return;

            AdMaster.instance.ShowAd(AdType.Interstitial);

            _currentTime = _adShowInterval * 60;
        }
    }
}
