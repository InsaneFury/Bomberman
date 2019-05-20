using UnityEngine;

public class UILoadNextLevel : MonoBehaviour
{
    public void LoadNextLevel()
    {
        LoaderManager.Get().LoadScene("Menu");
        UILoadingScreen.Get().SetVisible(true);
    }
}
