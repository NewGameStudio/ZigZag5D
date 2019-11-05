using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using GooglePlayGames;
using GooglePlayGames.BasicApi;

public class LeaderboardMaster : MonoBehaviour
{
    public static int BestWorldScore { get; private set; } = -1;

    private static bool Initialized;
    private const string LeaderboardID = "CgkIjt-Uhc0GEAIQAg";


    private static void OnInitialize(bool success)
    {
        try
        {
            Debug.Log("NGSMSG::OnInitialize() - start");

            Debug.Log("NGSMSG::Success - " + success);

            if (success)
            {
                Initialized = true;

                UpdateLeaderboard();
            }

            Debug.Log("NGSMSG::OnInitialize() - end");
        }
        catch (System.Exception ex)
        {
            Debug.Log("NGSMSG::OnInitialize() - error : " + ex.Message);
        }
    }

    private void OnDestroy()
    {

    }


    public static void Initialize()
    {
        try
        {
            Debug.Log("NGSMSG::Initialize() - start");

            PlayGamesPlatform.Activate();
            Social.localUser.Authenticate(OnInitialize);

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
                Initialize();
                return;
            }

            Social.LoadScores(LeaderboardID, (IScore[] scores) =>
            {
                if (scores == null || scores.Length == 0)
                {
                    Debug.Log("NGSMSG::scores less then 1");
                    SubmitPlayerScore(ScoresMaster.BestScore);
                    return;
                }

                BestWorldScore = (int)scores[0].value;

                for (int i = 0; i < scores.Length; i++)
                    Debug.Log("NGSMSG::Score[" + i + "] is " + scores[i].value);
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
                Initialize();
                return;
            }

            Social.ReportScore(score, LeaderboardID, (bool success) =>
            {
                Debug.Log("NGSMSG::ReportState - " + success);
            });
            
            Debug.Log("NGSMSG::SubmitPlayerScore() - end");
        }
        catch (System.Exception ex)
        {
            Debug.Log("NGSMSG::SubmitPlayerScore() - error : " + ex.Message);
        }
    }
}
