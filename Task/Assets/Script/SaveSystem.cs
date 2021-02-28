using System.Collections;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    public static void SaveGame()
    {
        Debug.Log("saving");

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = (File.Exists(Application.persistentDataPath + "/SavedGame.dat") ?
            File.Open(Application.persistentDataPath + "/SavedGame.dat", FileMode.Open) :
            File.Create(Application.persistentDataPath + "/SavedGame.dat"));

        Datas game = new Datas();

        game.Score = Datas.current.Score;

        for (int i = 0; i < Datas.current.HistoryDatasAmount.Length; i++)
        {
            game.HistoryDatasAmount = Datas.current.HistoryDatasAmount;
            game.BetsSign = Datas.current.BetsSign;
            game.ResultSign = Datas.current.ResultSign;
        }


        bf.Serialize(file, Datas.current);
        file.Close();
    }

    public static void LoadGame()
    {
        Debug.Log("Load");
        if (File.Exists(Application.persistentDataPath + "/SavedGame.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/SavedGame.dat", FileMode.Open);
            Datas game = (Datas)bf.Deserialize(file);
            file.Close();

            Datas.current.Score = game.Score;

            for (int i = 0; i < Datas.current.HistoryDatasAmount.Length; i++)
            {
                Datas.current.HistoryDatasAmount = game.HistoryDatasAmount;
                Datas.current.BetsSign = game.BetsSign;
                Datas.current.ResultSign = game.ResultSign;
            }
        }
        else
        {
            SaveGame();
            LoadGame();
        }
    }
}