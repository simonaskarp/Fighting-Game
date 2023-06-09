using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameControl : MonoBehaviour
{
    public int nextSceneNumber;

    public int playerWins = 0;
    public int enemyWins = 0;

    public Rigidbody2D player;
    public Rigidbody2D enemy;

    public Health pHealth;
    public Health eHealth;
    public GameTime gameTime;

    public Image pWin1;
    public Image pWin2;
    public Image eWin1;
    public Image eWin2;

    AudioSource audio;

    public AudioClip round;
    public AudioClip roundNumber;
    public AudioClip fight;
    public AudioClip background;
    public AudioClip death;

    public Image fightImg;

    public GameObject revengeB;
    public GameObject menuB;
    public Image pWin;
    public Image eWin;
    public Image draw;

    private void OnDisable()
    {
        PlayerPrefs.SetInt("pWins", playerWins);
        PlayerPrefs.SetInt("eWins", enemyWins);
    }

    private void OnEnable()
    {
        playerWins = PlayerPrefs.GetInt("pWins");
        enemyWins = PlayerPrefs.GetInt("eWins");
    }

    private void Awake()
    {
        pHealth.onDeath.AddListener(OnDeathPlayer);
        eHealth.onDeath.AddListener(OnDeathEnemy);
        gameTime.onTime.AddListener(OnDraw);
    }

    private void Start()
    {
        audio = gameObject.AddComponent<AudioSource>();
        fightImg.enabled = false;
        pWin.enabled = false;
        eWin.enabled = false;
        draw.enabled = false;
        menuB.SetActive(false);
        revengeB.SetActive(false);
        player.bodyType = RigidbodyType2D.Static;
        enemy.bodyType = RigidbodyType2D.Static;
        Invoke("Round", 0.2f);
    }

    void Round()
    {
        audio.clip = round;
        audio.Play();
        Invoke("Number", 0.8f);
    }

    void Number()
    {
        audio.clip = roundNumber;
        audio.Play();
        Invoke("Fight", 0.8f);
    }

    void Fight()
    {
        audio.clip = fight;
        audio.Play();
        fightImg.enabled = true;
        Invoke("Gameplay", 0.8f);
    }

    void Gameplay()
    {
        fightImg.enabled = false;
        audio.clip = background;
        audio.Play();
        audio.loop = true;
        gameTime.timerOn = true;
        player.bodyType = RigidbodyType2D.Dynamic;
        enemy.bodyType = RigidbodyType2D.Dynamic;
    }

    private void Update()
    {
        if (playerWins >= 1) pWin1.color = Color.green;
        if (playerWins == 2) pWin2.color = Color.green;
        if (enemyWins >= 1) eWin1.color = Color.green;
        if (enemyWins == 2) eWin2.color = Color.green;
    }

    void OnDeathPlayer()
    {
        audio.clip = death;
        audio.Play();
        audio.loop = false;
        gameTime.timerOn = false;
        enemyWins++;
        if (enemyWins < 2)
        {
            Invoke("NextScene", 1.5f);
        }
        else
        {
            playerWins = 0;
            enemyWins = 0;
            eWin.enabled = true;
            Invoke("GameOver", 1.5f);
        }
    }

    public void OnDeathEnemy()
    {
        audio.clip = death;
        audio.Play();
        audio.loop = false;
        gameTime.timerOn = false;
        playerWins++;
        if (playerWins < 2)
        {
            Invoke("NextScene", 1.5f);
        }
        else
        {
            playerWins = 0;
            enemyWins = 0;
            pWin.enabled = true;
            Invoke("GameOver", 1.5f);
        }
    }

    void OnDraw()
    {
        playerWins++;
        enemyWins++;
        if (playerWins < 2 && enemyWins < 2)
        {
            Invoke("NextScene", 1.5f);
        }
        else
        {
            playerWins = 0;
            enemyWins = 0;
            draw.enabled = true;
            Invoke("Draw", 1.5f);
        }
    }

    void NextScene()
    {
        SceneManager.LoadScene(nextSceneNumber);
    }

    void GameOver()
    {
        player.bodyType = RigidbodyType2D.Static;
        enemy.bodyType = RigidbodyType2D.Static;
        menuB.SetActive(true);
        revengeB.SetActive(true);
    }

    void Draw()
    {
        player.bodyType = RigidbodyType2D.Static;
        enemy.bodyType = RigidbodyType2D.Static;
        menuB.SetActive(true);
        revengeB.SetActive(true);
    }
}
