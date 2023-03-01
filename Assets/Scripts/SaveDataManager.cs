using System;
using System.IO;
using UnityEngine;

public class SaveDataManager : MonoBehaviour
{
    public static SaveDataManager Instance { get; private set; }
    private string _playerName;
    private RecordedScore _highScore = null;

    public string PlayerName
    {
        get
        {
            return _playerName;
        }
        set
        {
            _playerName = value;
            SaveSaveData();
        }
    }

    public RecordedScore HighScore
    {
        get
        {
            return _highScore;
        }
        set
        {
            _highScore = value;
            SaveSaveData();
        }
    }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadSaveData();
        }
        else
        {
            Destroy(gameObject);
        }

    }

    [Serializable]
    public class SaveData
    {
        public string PlayerName;
        public RecordedScore HighScore;
    }

    public void LoadSaveData()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            _playerName = data.PlayerName;
            _highScore = data.HighScore;
        }
    }

    public void SaveSaveData()
    {
        SaveData data = new SaveData
        {
            PlayerName = _playerName,
            HighScore = _highScore
        };

        string json = JsonUtility.ToJson(data);

        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
        Debug.Log("Saved data to " + Application.persistentDataPath + "/savefile.json");
    }
}
