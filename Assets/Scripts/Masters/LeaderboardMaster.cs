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
            Initialized = true;

            Cloud.OnInitializeComplete -= OnInitialize;

            UpdateLeaderboard();
        }
        catch (System.Exception ex)
        {
            Debug.Log("IntError" + ex.Message);
        }
    }

    public static void Initialize()
    {
        try
        {
            BestWorldScore = -1;
            Initialized = false;

            Cloud.OnInitializeComplete += OnInitialize;
            Cloud.Initialize(false, true);
        }
        catch (System.Exception ex)
        {
            Debug.Log("IntError" + ex.Message);
        }
    }

    public static void UpdateLeaderboard()
    {
        try
        {
            if (!Initialized)
            {
                Cloud.Initialize(false, true);
                return;
            }

            Leaderboards.BestScoreInWorld.LoadScores((IScore[] scores) =>
            {
                if (scores == null || scores.Length == 0)
                    return;

                BestWorldScore = (int)scores[0].value;
            });

        }
        catch (System.Exception ex)
        {
            Debug.Log("IntError" + ex.Message);
        }
    }

    public static void SubmitPlayerScore(int score)
    {
        try
        {
            if (!Initialized)
            {
                Cloud.Initialize(false, true);
                return;
            }

            Leaderboards.BestScoreInWorld.SubmitScore(score);
        }
        catch (System.Exception ex)
        {
            Debug.Log("IntError" + ex.Message);
        }
    }
}
