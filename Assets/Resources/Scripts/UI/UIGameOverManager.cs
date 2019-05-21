using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIGameOverManager : SingletonMonobehaviour<UIGameplayManager>
{
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI highScoreText;
    public TextMeshProUGUI timeText;
    public GameObject gameOver;
    public GameObject victory;

    ScoreManager sManager;
    GameManager gManager;
    ScenesManager scenes;

    public override void Awake()
    {
        base.Awake();
    }

    void Start()
    {
        sManager = ScoreManager.Get();
        gManager = GameManager.Get();
        scenes = ScenesManager.Get();
        scoreText.text = sManager.score.ToString();
        highScoreText.text = sManager.highScore.ToString();
        timeText.text = gManager.time;
        if (gManager.victory)
        {
            victory.SetActive(true);
        }
        else
        {
           gameOver.SetActive(true);
        }
    }

    public void RefreshScoreUI()
    {
        scoreText.text = sManager.score.ToString();
        highScoreText.text = sManager.highScore.ToString();
    }

    public void Retry()
    {
        gManager.Retry();
        scenes.ReloadScene();
    }

    public void Exit()
    {
        Application.Quit();
    }
}
