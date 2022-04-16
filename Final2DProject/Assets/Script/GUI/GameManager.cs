using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : LoadNextLevel
{
    public static GameManager instance;

    [Header("Point")]
    private Vector2 startPoint;
    public Transform startPointObject;
    public Transform enemySpawnPoint;
    public Transform itemSpawnPoint;
    public Transform cameraSpawnPoint;
    [Header("GUI")]
    public GameObject victoryWindow;
    public GameObject gameOverWindow;
    [Header("Player && Enemy")]
    public GameObject player;
    public GameObject enemyPrefab;
    public GameObject itemPrefab;
    [Header("Effect")]
    public GameObject appearEffect;
    public GameObject disappearEffect;
    [Header("Scene")]
    public int nextSceneIndex;
    private GameObject cameraObject;
    [Header("Buff")]
    public GameObject pineApple;
    public GameObject cherry;
    private void Awake()
    {
        nextSceneIndex = SceneManager.GetActiveScene().buildIndex - 1;
        startPoint = startPointObject.position;
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(AppearWaitingAtStartPoint());
        if (SceneManager.GetActiveScene().buildIndex != 5)
        {
            cameraObject = PoolManager.instance.SpawnObject(PoolObjectsType.Camera);
            cameraObject.transform.position = cameraSpawnPoint.position;
        }

        if (SceneManager.GetActiveScene().buildIndex == 5 && PlayerPrefs.GetInt("PlayerCurrentLives") > 2)
        {
            Instantiate(cherry, itemSpawnPoint.position, Quaternion.identity);
        }
        else if (SceneManager.GetActiveScene().buildIndex == 5 && PlayerPrefs.GetInt("PlayerCurrentLives") <= 2)
        {
            Instantiate(pineApple, itemSpawnPoint.position, Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(enemyPrefab.transform.childCount);
    }
    IEnumerator AppearWaitingAtStartPoint() //timer for appearing
    {
        var effect = Instantiate(appearEffect, startPoint, Quaternion.identity); //should create a variable to contain this if not u will delete the prefab in library and it makes error
        yield return new WaitForSeconds(0.3f);
        if (SceneManager.GetActiveScene().buildIndex == 5)
        {
            player.transform.position = startPoint;
            player.SetActive(true);
        }
        SpawnAllObject();
        Destroy(effect);
    }

    IEnumerator DisappearWaiting() //timer for disappearing
    {
        if (SceneManager.GetActiveScene().buildIndex == 5)
        {
            player.SetActive(false);
        }
        var effect = Instantiate(disappearEffect, player.transform.position, Quaternion.identity); //should create a variable to contain this if not u will delete the prefab in library and it makes error
        PoolManager.instance.ReturnAllObject();
        yield return new WaitForSeconds(0.3f);
        Time.timeScale = 0;
        Destroy(effect);
    }

    public void LoadLevelList()
    {
        StartCoroutine(LoadAsynchronously(3));
        Time.timeScale = 1;
    }

    public void LoadNextScene()
    {
        StartCoroutine(LoadAsynchronously(SceneManager.GetActiveScene().buildIndex + 1));
        Time.timeScale = 1;
    }

    public void Win()
    {
        StartCoroutine(DisappearWaiting());
        victoryWindow.SetActive(true);
        if (nextSceneIndex > PlayerPrefs.GetInt("levelAt"))
        {
            PlayerPrefs.SetInt("levelAt", nextSceneIndex);
        }
    }

    public void GameOver()
    {
        StartCoroutine(DisappearWaiting());
        gameOverWindow.SetActive(true);
    }
    public void ResetLevel()
    {
        if (SceneManager.GetActiveScene().buildIndex == 5)
        {
            StartCoroutine(LoadAsynchronously(SceneManager.GetActiveScene().buildIndex));
        }
        victoryWindow.SetActive(false);
        gameOverWindow.SetActive(false);
        StartCoroutine(AppearWaitingAtStartPoint());
        LivesManager.instance.ResetLives();
        PlayerController.instance.knockBackCounter = 0;
        PlayerController.instance.isShooting = false;
        PlayerController.instance.isTitan = false;
        PlayerController.instance.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
        Time.timeScale = 1;
    }
    public void Quit()
    {
        Application.Quit();
    }

    void SpawnAllObject()
    {
        if (SceneManager.GetActiveScene().buildIndex != 5)
        {
            player = PoolManager.instance.SpawnObject(PoolObjectsType.Player);
            CheckAndSetActive(player);
            player.transform.position = startPoint;
            enemyPrefab = PoolManager.instance.SpawnObject(PoolObjectsType.Enemy);
            CheckAndSetActive(enemyPrefab);
            enemyPrefab.transform.position = enemySpawnPoint.transform.position;
            itemPrefab = PoolManager.instance.SpawnObject(PoolObjectsType.Item);
            CheckAndSetActive(itemPrefab);
            itemPrefab.transform.position = itemSpawnPoint.transform.position;
        }
    }
    
    void CheckAndSetActive(GameObject obj)
    {
        for (int i = 0; i < obj.transform.childCount; i++)
        {
            if (obj.transform.GetChild(i).gameObject.activeSelf == false)
            {
                obj.transform.GetChild(i).gameObject.SetActive(true);
            }
        }
    }

    public void ResetData()
    {
        PlayerPrefs.DeleteKey("levelAt");
    }
}
