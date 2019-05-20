using UnityEngine.UI;

public class UILoadingScreen : SingletonMonobehaviour<UILoadingScreen>
{
    public Text loadingText;

    public override void Awake()
    {
        base.Awake();
    }

    public void SetVisible(bool show)
    {
        gameObject.SetActive(show);
    }

    public void Update()
    {
        int loadingVal = (int) (LoaderManager.Get().loadingProgress*100);
        loadingText.text = loadingVal.ToString() + "%";
        if (LoaderManager.Get().loadingProgress >= 1)
            SetVisible(false);
    }
}