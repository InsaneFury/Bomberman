using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGameplayManager : MonoBehaviour
{
    public GameObject[] lives;
    Player player;

    void Start()
    {
        player = Player.Get();
        for (int i = 0; i < player.startLives; i++)
        {
            lives[i].GetComponent<Life>().LifeEnabled = true;
        }
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
    }

    void ResetLives()
    {
        for (int i = 0; i < player.startLives; i++)
        {
            lives[i].GetComponent<Life>().LifeEnabled = true;
        }
    }
}
