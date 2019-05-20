using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIGameplayManager : SingletonMonobehaviour<UIGameplayManager>
{
    public GameObject[] lives;
    public GameObject portalText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI highScoreText;
    public TextMeshProUGUI bombsText;
    public TextMeshProUGUI explosionDistText;
    public TextMeshProUGUI enemysText;
    public TextMeshProUGUI timeText;

    Player player;
    ScoreManager sManager;
    GameManager gManager;
    EnemyManager eManager;

    public override void Awake()
    {
        base.Awake();
    }

    void Start()
    {
        player = Player.Get();
        sManager = ScoreManager.Get();
        gManager = GameManager.Get();
        eManager = EnemyManager.Get();
        scoreText.text = sManager.score.ToString();
        highScoreText.text = sManager.highScore.ToString();
        bombsText.text = player.currentBombs.ToString();
        explosionDistText.text = player.sizeOfExplosion.ToString();
        enemysText.text = eManager.EnemysActiveInGame.ToString();
        timeText.text = gManager.time;

        for (int i = 0; i < player.startLives; i++)
        {
            lives[i].GetComponent<Life>().LifeEnabled = true;
        }
        player.OnPlayerDropBomb += RefreshBombUI;
    }

    void Update()
    {
        if (player.lives == 1)
        {
            lives[1].GetComponent<Life>().LifeEnabled = false;
        }
        if (player.lives == 0)
        {
            lives[0].GetComponent<Life>().LifeEnabled = false;
        }
        if (!gManager.gameOver)
        {
            timeText.text = gManager.time;
        }
        if (eManager.EnemysActiveInGame == 0)
        {
            portalText.SetActive(true);
        }
        else
        {
            portalText.SetActive(false);
        }
    }

    void ResetLives()
    {
        for (int i = 0; i < player.startLives; i++)
        {
            lives[i].GetComponent<Life>().LifeEnabled = true;
        }
    }

    public void RefreshScoreUI()
    {
        scoreText.text = sManager.score.ToString();
        highScoreText.text = sManager.highScore.ToString();
    }

    public void RefreshBombUI(Player p)
    {
        bombsText.text = p.currentBombs.ToString();
    }

    public void RefreshCountOfEnemysUI()
    {
        enemysText.text = eManager.EnemysActiveInGame.ToString();
    }


    public void RefreshBombUI(Bomb b)
    {
        RefreshBombUI(player);
    }

}
