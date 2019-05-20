﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : SingletonMonobehaviour<EnemyManager>
{
    public List<GameObject> enemys;
    public int amountOfEnemysInGame = 7;
    public int EnemysActiveInGame = 0;

    void Start()
    {
        if (amountOfEnemysInGame > enemys.Count)
        {
            amountOfEnemysInGame = enemys.Count;
        }
        else if (amountOfEnemysInGame < 0)
        {
            amountOfEnemysInGame = 1;
        }
        EnemysActiveInGame = amountOfEnemysInGame;
        for (int i = 0; i < amountOfEnemysInGame; i++)
        {
            enemys[i].gameObject.SetActive(true);
        }
        Enemy.OnEnemyDie += RemoveEnemyFromList;
    }

    void RemoveEnemyFromList(Enemy e)
    {
        EnemysActiveInGame--;
    }
}