using System;
using UnityEngine;

public static class ScoresMaster
{
    public static string SaveDirectory
    {
        get
        {
            return Application.persistentDataPath + "Scores/";
        }
    }
    public static string SaveFileName
    {
        get
        {
            return "BestScore.sv";
        }
    }
    public static string FullSavePath
    {
        get
        {
            return SaveDirectory + SaveFileName;
        }
    }

    public static int Score { get; private set; }
    public static int BestScore { get; private set; }

    public static event Action<int> OnScoreChanged;


    public static void OnStartGame()
    {
        Score = 0;
    }

    public static void IncScore()
    {
        Score++;

        OnScoreChanged?.Invoke(Score);
    }

    public static void OnEndGame()
    {
        if (LeaderboardMaster.BestWorldScore > 0 && BestScore > LeaderboardMaster.BestWorldScore)
            LeaderboardMaster.SubmitPlayerScore(BestScore);

        if (Score > BestScore)
        {
            BestScore = Score;
            SaveBestScore();
        }
    }


    public static void LoadBestScore()
    {
        if (!PlayerPrefs.HasKey("BestScore"))
        {
            PlayerPrefs.SetInt("BestScore", 0);
            PlayerPrefs.Save();
        }

        BestScore = PlayerPrefs.GetInt("BestScore");
    }

    private static void SaveBestScore()
    {
        PlayerPrefs.SetInt("BestScore", BestScore);
        PlayerPrefs.Save();
    }
}
