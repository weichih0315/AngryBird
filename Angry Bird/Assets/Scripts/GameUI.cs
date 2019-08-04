using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    public GameObject winMenu;

    public static GameUI instance;

    private void Awake()
    {
        instance = this;
    }

    public void Win()
    {
        winMenu.SetActive(true);
    }

    public void Restart()
    {
        AudioManager.instance.PlaySound2D("Click");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Home()
    {
        AudioManager.instance.PlaySound2D("Click");
        SceneManager.LoadScene("Menu");
    }

    public void Next()
    {
        AudioManager.instance.PlaySound2D("Click");
        SceneManager.LoadScene("Level " + GameManager.instance.nextLevel);
    }
}
