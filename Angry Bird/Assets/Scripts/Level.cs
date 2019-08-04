using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Level : MonoBehaviour {

    public string titleText;

    public GameObject lockLevel;

    public Text levelText;

    public bool isLock;

    private void OnEnable()
    {
        levelText.text = titleText;
        LoadLevelData();

        lockLevel.SetActive(isLock);
    }

    public void OnClick()
    {
        AudioManager.instance.PlaySound2D("Click");
        if (!isLock)
        {
            string sceneName = "Level " + levelText.text;
            LoadLevel(sceneName);
        }
    }

    private void LoadLevel(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    private void LoadLevelData()
    {
        string loadJson = PlayerPrefs.GetString("Level " + levelText.text, "");

        if (loadJson == "")
        {
            LevelData levelData;
            levelData = (levelText.text == "1 - 1") ? new LevelData(false) : new LevelData(true);

            string saveString = JsonUtility.ToJson(levelData);
            PlayerPrefs.SetString("Level " + levelText.text, saveString);
        }
        else
        {
            LevelData loadData;
            loadData = JsonUtility.FromJson<LevelData>(loadJson);
            isLock = loadData.isLock;
        }
    }
}
