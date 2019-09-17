using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UICore;

public class AchievementsMaster : MonoBehaviour
{
    [SerializeField] private List<int> _scoreValues;
    [SerializeField] private List<string> _achievements;
    [SerializeField] private float _achievementShowTime = 3f;

    [Space]

    [SerializeField] private UIWindow _achievementWindow;
    [SerializeField] private Text _achievementTextField;

    private void Awake()
    {
        if (_scoreValues == null || _achievements == null || _scoreValues.Count == 0 || _achievements.Count == 0)
        {
            Debug.Log("AchievementsMaster::achievements count less then 1");

            enabled = false;

            return;
        }

        if (_scoreValues.Count != _achievements.Count)
        {
            Debug.Log("AchievementsMaster::scoreValues length not equals achievements length");

            enabled = false;

            return;
        }

        ScoresMaster.OnScoreChanged += OnScoreChanged;
    }

    private void OnScoreChanged(int score)
    {
        if (_scoreValues.Contains(score))
            StartCoroutine(ShowAchievementRoutine(_scoreValues.IndexOf(score)));
    }

    private IEnumerator ShowAchievementRoutine(int index)
    {
        _achievementTextField.text = _achievements[index];

        _achievementWindow.Show();

        yield return new WaitForSeconds(_achievementShowTime);

        _achievementWindow.Hide();
    }
}
