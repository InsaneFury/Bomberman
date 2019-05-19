using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public List<GameObject> enemys;
    public int amountOfEnemysInGame = 7;

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
        for (int i = 0; i < amountOfEnemysInGame; i++)
        {
            enemys[i].gameObject.SetActive(true);
        }
    }
}
