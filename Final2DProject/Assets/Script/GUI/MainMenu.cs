using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public static MainMenu instance;
    public int defaultLives;
    [SerializeField] PlayerData playerData;
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        defaultLives = playerData.health;
        PlayerPrefs.SetInt("PlayerDefaultLives", defaultLives);
        PlayerPrefs.SetInt("PlayerCurrentLives", defaultLives);
    }

    private void Update()
    {
        DeleteScenePlayerPrefs();
    }
    public void TurnOnLevelList()
    {
        SceneManager.LoadScene("LevelSelect");
    }

    void DeleteScenePlayerPrefs()
    {
        if (Input.GetKeyDown(KeyCode.Delete))
        {
            PlayerPrefs.DeleteKey("levelAt");
        }
    }
}
