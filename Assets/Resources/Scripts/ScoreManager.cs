using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : SingletonMonobehaviour<ScoreManager>
{
    [Header("ScoreSettings")]
    public int score = 0;
    [SerializeField]
    int highScore = 0;

    public override void Awake()
    {
        base.Awake();
    }

    void Start()
    {
        Enemy.OnEnemyDie += AddScoreFromEnemy;
        DestructibleObject.OnObjectDestroy += AddScoreFromBrickWall;
        highScore = PlayerPrefs.GetInt("HighScore");
    }

    private void Update()
    {
        
    }

    void AddScoreFromEnemy(Enemy e)
    {
        score += e.score;
    }

    void AddScoreFromBrickWall(DestructibleObject d)
    {
        score += d.score;
    }

    public void AddHighScore()
    {
        if (score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt("HighScore", highScore);
        }   
    }
}
