using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class GameDataManager : MonoBehaviour {

    public GameData gameData;

    public static GameDataManager instance;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        gameData = new GameData();
    }
}

public class GameData
{
    public OptionData optionData;

    public List<LevelData> levelDatas;

    public GameData()
    {
        optionData = new OptionData();
    }
}

public class OptionData
{
    public AudioSetting audioSetting;

    public OptionData()
    {
        audioSetting = new AudioSetting();
    }

    public void Save()
    {
        audioSetting.Save();
    }

    public void Load()
    {
        audioSetting.Load();
    }
}

public class AudioSetting
{
    public float masterVolumePercent = 0f;
    public float sfxVolumePercent = 0f;
    public float musicVolumePercent = 0f;

    public void Save()
    {
        AudioSetting audioSetting = GameDataManager.instance.gameData.optionData.audioSetting;
        string saveString = JsonUtility.ToJson(audioSetting);

        PlayerPrefs.SetString("audioSetting", saveString);
        /*StreamWriter file = new StreamWriter(System.IO.Path.Combine(Application.persistentDataPath, "audioSetting"));
        file.Write(saveString);
        file.Close();*/
    }

    public void Load()
    {
        /* file = new StreamReader(System.IO.Path.Combine(Application.persistentDataPath, "audioSetting"));
        string loadJson = file.ReadToEnd();
        file.Close();*/

        string loadJson = PlayerPrefs.GetString("audioSetting", "");

        AudioSetting loadData = new AudioSetting();

        if (JsonUtility.FromJson<AudioSetting>(loadJson) != null)
            loadData = JsonUtility.FromJson<AudioSetting>(loadJson);

        masterVolumePercent = loadData.masterVolumePercent;
        musicVolumePercent = loadData.musicVolumePercent;
        sfxVolumePercent = loadData.sfxVolumePercent;
    }
}

public class LevelData
{
    public bool isLock = true;

    public LevelData(bool isLock)
    {
        this.isLock = isLock;
    }
}