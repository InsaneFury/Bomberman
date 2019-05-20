using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonMonobehaviour<GameManager>
{
    [Header("Settings")]
    [Tooltip("Player Spawn Point")]
    public Transform spawnPoint;
    Player player;
    Portal portal;
    ScoreManager sManager;

    public override void Awake()
    {
        base.Awake();
    }

    void Start()
    {
        player = Player.Get();
        player.OnPlayerDie += ResetPlayer;
        portal = Portal.Get();
        sManager = ScoreManager.Get();
    }

    void Update()
    {
        if (player.lives <= 0 && player.canMove)
        {
            GameOver();
        }
        else if (player.lives > 0 && player.canMove && portal.playerIsOnPortal)
        {
            Victory();
        }
    }

    void ResetPlayer(Player player)
    {
        player.transform.position = spawnPoint.position;
        player.Revive();
    }

    void GameOver()
    {
        player.StopPlayer();
        Debug.Log("GameOver");
    }

    void Victory()
    {
        player.StopPlayer();
        sManager.AddHighScore();
        Debug.Log("Victory");
    }

}
