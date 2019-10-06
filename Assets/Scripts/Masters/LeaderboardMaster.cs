using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using CloudOnce;

public class LeaderboardMaster : MonoBehaviour
{
    public static int BestWorldScore { get; private set; }
    private static bool Initialized;


    private static void OnInitialize()
    {
        try
        {
            Debug.Log("NGSMSG::OnInitialize() - start");

            Initialized = true;

            Cloud.OnInitializeComplete -= OnInitialize;

            UpdateLeaderboard();

            Debug.Log("NGSMSG::OnInitialize() - end");
        }
        catch (System.Exception ex)
        {
            Debug.Log("NGSMSG::OnInitialize() - error : " + ex.Message);
        }
    }

    public static void Initialize()
    {
        try
        {
            Debug.Log("NGSMSG::Initialize() - start");

            BestWorldScore = -1;
            Initialized = false;

            Cloud.OnInitializeComplete += OnInitialize;
            Cloud.OnSignInFailed += () => { Debug.Log("NGSMSG::SignInFailed"); };

            Cloud.Initialize(false, true);

            Debug.Log("NGSMSG::Initialize() - end");
        }
        catch (System.Exception ex)
        {
            Debug.Log("NGSMSG::Initialize() - error : " + ex.Message);
        }
    }

    public static void UpdateLeaderboard()
    {
        try
        {
            Debug.Log("NGSMSG::UpdateLeaderboard() - start");

            if (!Initialized)
            {
                Debug.Log("NGSMSG::UpdateLeaderboard() - not initialized");
                Cloud.Initialize(false, true);
                return;
            }

            Leaderboards.BestScoreInWorld.LoadScores((IScore[] scores) =>
            {
                Debug.Log("NGSMSG::ScoresLoaded : " + (scores == null ? "null" : scores.Length.ToString()));

                if (scores == null || scores.Length == 0)
                    return;

                for (int i = 0; i < scores.Length; i++)
                    Debug.Log("NGSMSG::Score[" + i + "] - " + scores[i].value);

                BestWorldScore = (int)scores[scores.Length - 1].value;
            });

            Debug.Log("NGSMSG::UpdateLeaderboard() - end");
        }
        catch (System.Exception ex)
        {
            Debug.Log("NGSMSG::UpdateLeaderboard() - error : " + ex.Message);
        }
    }

    public static void SubmitPlayerScore(int score)
    {
        try
        {
            Debug.Log("NGSMSG::SubmitPlayerScore() - start");

            if (!Initialized)
            {
                Debug.Log("NGSMSG::SubmitPlayerScore() - not Initialized");
                Cloud.Initialize(false, true);
                return;
            }

            Leaderboards.BestScoreInWorld.SubmitScore(score);

            Debug.Log("NGSMSG::SubmitPlayerScore() - end");
        }
        catch (System.Exception ex)
        {
            Debug.Log("NGSMSG::SubmitPlayerScore() - error : " + ex.Message);
        }
    }
}
