using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
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


    public static void OnStartGame()
    {
        Score = 0;
    }

    public static void OnIncScore()
    {
        Score++;
    }

    public static void OnEndGame()
    {
        if (Score > BestScore)
        {
            BestScore = Score;
            SaveBestScore();
        }
    }


    public static void LoadBestScore()
    {
        if (!File.Exists(FullSavePath))
        {
            BestScore = 0;
            SaveBestScore();
            return;
        }

        using (FileStream stream = new FileStream(FullSavePath, FileMode.Open))
        {
            BinaryFormatter formatter = new BinaryFormatter();

            BestScore = (int) formatter.Deserialize(stream);
        }
    }

    private static void SaveBestScore()
    {
        if (!Directory.Exists(SaveDirectory))
            Directory.CreateDirectory(SaveDirectory);

        using (FileStream stream = new FileStream(FullSavePath, FileMode.Create))
        {
            BinaryFormatter formatter = new BinaryFormatter();

            formatter.Serialize(stream, BestScore);
        }
    }
}
