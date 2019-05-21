using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheatsManager : MonoBehaviour
{
    public InputField cheatConsole;
    EnemyManager eManager;
    string freezeEnemys = "morita";
    string reset = "reset";
    void Start()
    {
        eManager = EnemyManager.Get();   
    }

    public void InputCheat()
    {
        if (cheatConsole.text == freezeEnemys)
        {
            cheatConsole.text = " ";
            foreach (GameObject go in eManager.enemys)
            {
                go.GetComponent<Enemy>().canMove = false;
            }
        }
        else if (cheatConsole.text == reset)
        {
            cheatConsole.text = " ";
            foreach (GameObject go in eManager.enemys)
            {
                go.GetComponent<Enemy>().canMove = true;
            }
        }
        else if (cheatConsole.text == "exit".ToString())
        {
            cheatConsole.text = " ";
            Application.Quit();
        }
    }
}
