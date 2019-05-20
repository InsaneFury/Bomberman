using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : SingletonMonobehaviour<ScoreManager>
{
    [Header("ScoreSettings")]
    public int score = 0;
    public int highScore = 0;

    public override void Awake()
    {
        base.Awake();
    }

    void Start()
    {
        Enemy.OnEnemyDie += AddScoreFromEnemy;
        DestructibleObject.OnObjectDestroy += AddScoreFromBrickWall;
        highScore = PlayerPrefs.GetInt("HighScore");
        UIGameplayManager.Get().RefreshScoreUI();
    }

    void AddScoreFromEnemy(Enemy e)
    {
        score += e.score;
        UIGameplayManager.Get().RefreshScoreUI();
    }

    void AddScoreFromBrickWall(DestructibleObject d)
    {
        score += d.score;
        UIGameplayManager.Get().RefreshScoreUI();
    }

    public void AddHighScore()
    {
        if (score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt("HighScore", highScore);
            UIGameplayManager.Get().RefreshScoreUI();
        }   
    }

    public void ResetHighScore()
    {
        PlayerPrefs.SetInt("HighScore", 0);
        UIGameplayManager.Get().RefreshScoreUI();
    }
}
