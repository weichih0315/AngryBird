using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public static int EnemyAlive = 0;

    public string nextLevel = "";

    public static GameManager instance;
    private void Awake()
    {
        instance = this;
    }

    public void Win()
    {
        Debug.Log("Win");
        GameUI.instance.Win();

        //儲存   暫時不需要  因為只有鎖  無其他資料
        //SaveLevel();

        //解鎖  
        string loadJson = PlayerPrefs.GetString("Level " + nextLevel, "");
        LevelData loadData = JsonUtility.FromJson<LevelData>(loadJson);

        LevelData levelData = new LevelData(false);
        string saveString = JsonUtility.ToJson(levelData);
        PlayerPrefs.SetString("Level " + nextLevel, saveString);
    }

    public void SaveLevel()
    {
        string loadJson = PlayerPrefs.GetString(SceneManager.GetActiveScene().name, "");
        LevelData loadData = JsonUtility.FromJson<LevelData>(loadJson);

        LevelData levelData = new LevelData(false);
        string saveString = JsonUtility.ToJson(levelData);
        PlayerPrefs.SetString(SceneManager.GetActiveScene().name, saveString);
    }
}
