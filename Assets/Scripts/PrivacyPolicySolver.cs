using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UICore;

public class PrivacyPolicySolver : MonoBehaviour
{
    private const string PolicyURL = "https://zigzag5dprivacypolicy.blogspot.com/2019/09/zigzag5d-privacy-policy.html";

    [SerializeField] private UIWindow _privacyPolicyWindow;
    [SerializeField] private UIWindow _tipsWindow;

    private void Start()
    {
        if (PrivacyPolicyConfirmed())
            _tipsWindow.Show();

        else
            _privacyPolicyWindow.Show();
    }

    private bool PrivacyPolicyConfirmed()
    {
        if (PlayerPrefs.HasKey("PrivacyPolicyConfirm"))
            if (PlayerPrefs.GetInt("PrivacyPolicyConfirm") == 1)
                return true;

        return false;
    }


    public void OpenPrivacyPolicy()
    {
        Application.OpenURL(PolicyURL);
    }

    public void ConfirmPrivacyPolicy()
    {
        PlayerPrefs.SetInt("PrivacyPolicyConfirm", 1);
        PlayerPrefs.Save();
    }
}
