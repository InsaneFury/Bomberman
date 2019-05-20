using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIIntro : MonoBehaviour
{
    public GameObject LogoEmpresa;
    public GameObject LogoGame;
    public string scene;

    public void SwapLogo() {
        LogoEmpresa.SetActive(false);
        LogoGame.SetActive(true);
    }

    public void LoadMainMenu() {
        SceneManager.LoadScene(scene);
    }

}
