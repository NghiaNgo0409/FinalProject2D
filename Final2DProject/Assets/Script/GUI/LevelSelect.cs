using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelect : LoadNextLevel
{
    public Button[] levelButtonList;
    public Button[] levelLockButtonList;

    private void Start()
    {
        int levelAtIndex = PlayerPrefs.GetInt("levelAt", 2);
        for (int i = 0; i < levelButtonList.Length; i++)
        {
            if (i + 2 > levelAtIndex)
            {
                levelButtonList[i].gameObject.SetActive(false);
                levelLockButtonList[i].gameObject.SetActive(true);
            }
        }
    }
    public void ReturnToMenu()
    {
        StartCoroutine(LoadAsynchronously(2));
    }

    public void LoadLevel(int index)
    {
        PlayerPrefs.SetInt("PlayerDefaultLives", MainMenu.instance.defaultLives);
        PlayerPrefs.SetInt("PlayerCurrentLives", MainMenu.instance.defaultLives);
        StartCoroutine(LoadAsynchronously(index));
    }
}
