using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;

public class Menu : MonoBehaviour {

	public GameObject mainMenuHolder;
	public GameObject optionsMenuHolder;
    public GameObject levelSelection;

    public Slider[] volumeSliders;

	void Start() {
		volumeSliders [0].value = AudioManager.instance.masterVolumePercent;
		volumeSliders [1].value = AudioManager.instance.musicVolumePercent;
		volumeSliders [2].value = AudioManager.instance.sfxVolumePercent;
    }

    public void Play()
    {
        AudioManager.instance.PlaySound2D("Click");
        mainMenuHolder.SetActive(false);
        optionsMenuHolder.SetActive(false);
        levelSelection.SetActive(true);
	}

    public void OptionsMenu()
    {
        AudioManager.instance.PlaySound2D("Click");
        mainMenuHolder.SetActive(false);
        optionsMenuHolder.SetActive(true);
        levelSelection.SetActive(false);
    }

    public void MainMenu()
    {
        AudioManager.instance.PlaySound2D("Click");
        mainMenuHolder.SetActive(true);
        optionsMenuHolder.SetActive(false);
        levelSelection.SetActive(false);
    }

    public void Quit() {
		Application.Quit ();
	}

    public void Reset()
    {
        AudioManager.instance.PlaySound2D("Click");

        LevelData levelData = new LevelData(false);
        string saveString = JsonUtility.ToJson(levelData);
        PlayerPrefs.SetString("Level 1 - 1", saveString);
        
        for (int i = 2; i < 6; i++)
        {
            levelData = new LevelData(true);
            saveString = JsonUtility.ToJson(levelData);
            PlayerPrefs.SetString("Level 1 - " + i, saveString);
        }
    }

    public void SetMasterVolume(float value) {
		AudioManager.instance.SetVolume (value, AudioManager.AudioChannel.Master);
    }

	public void SetMusicVolume(float value) {
		AudioManager.instance.SetVolume (value, AudioManager.AudioChannel.Music);
    }

	public void SetSfxVolume(float value) {
		AudioManager.instance.SetVolume (value, AudioManager.AudioChannel.Sfx);
    }

}
