using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LivesManager : MonoBehaviour
{
    public static LivesManager instance;

    [Header("Data")]
    //[SerializeField] private int totalLives;
    public int livesCounter;
    public bool isDead;

    [Header("UI")]
    [SerializeField] private Text text;

    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        livesCounter = PlayerPrefs.GetInt("PlayerCurrentLives");
    }

    // Update is called once per frame
    void Update()
    {
        text.text = "x" + livesCounter;
        if (isDead == true)
        {
            isDead = false;
            GameManager.instance.GameOver();
        }
    }

    public void TakeLives()
    {
        PlayerController.instance.GetComponent<Animator>().Play("Hit");
        livesCounter--;
        PlayerPrefs.SetInt("PlayerCurrentLives", livesCounter);
        if (livesCounter < 1)
        {
            isDead = true;
        }
    }

    public void ResetLives()
    {
        livesCounter = PlayerPrefs.GetInt("PlayerDefaultLives");
    }
}
