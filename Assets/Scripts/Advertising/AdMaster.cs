using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Monetization;

namespace Advertising
{
    public enum AdType { Interstitial, Reward }

    public class AdMaster : MonoBehaviour
    {
        public static AdMaster instance { get; private set; }

        private const string _gameID = "3291806";
        private const string _interstitialPlacement = "video";
        private const string _rewardPlacement = "rewardedVideo";

        private void Awake()
        {
            instance = this;

            Monetization.Initialize(_gameID, false);
        }

        public void ShowAd(AdType adType)
        {
            string placementID = adType == AdType.Interstitial ? _interstitialPlacement : _rewardPlacement;

            StartCoroutine(ShowAdRoutine(placementID));
        }

        private IEnumerator ShowAdRoutine(string placementID)
        {
            UnityWebRequest webRequest = new UnityWebRequest("http://google.com");

            yield return webRequest;

            if (webRequest.error != null)
                yield break;

            if(!Monetization.isInitialized)
                Monetization.Initialize(_gameID, false);

            float time = 0;
            while (!Monetization.IsReady(placementID) && time < 8f)
            {
                time += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }

            ShowAdPlacementContent ad = Monetization.GetPlacementContent(placementID) as ShowAdPlacementContent;

            ad?.Show();
        }
    }
}
