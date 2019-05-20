using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoaderManager : SingletonMonobehaviour<LoaderManager>
{
    public float loadingProgress;
    public bool fakeLoad;
    public float timeLoading;
    public float minTimeToLoad = 2;
    public string scene;

    private void Start()
    {
        LoadScene(scene);
    }
    public void LoadScene(string sceneName)
    {
        if (fakeLoad)
        {
            StartCoroutine(AsynchronousLoadWithFake(sceneName));  
        }
        else
        {
            StartCoroutine(AsynchronousLoad(sceneName));    
        }
    }

    IEnumerator AsynchronousLoad(string scene)
    {
        loadingProgress = 0;

        yield return null;

        AsyncOperation ao = SceneManager.LoadSceneAsync(scene);
        ao.allowSceneActivation = false;

        while (!ao.isDone)
        {
            loadingProgress = ao.progress + 0.1f;

            // Loading completed
            if (ao.progress >= 0.9f)
            {
                ao.allowSceneActivation = true;
            }

            yield return null;
        }
    }

    IEnumerator AsynchronousLoadWithFake(string scene)
    {
        loadingProgress = 0;
        timeLoading = 0;
        yield return null;

        AsyncOperation ao = SceneManager.LoadSceneAsync(scene);
        ao.allowSceneActivation = false;

        while (!ao.isDone)
        {
            timeLoading += Time.deltaTime;
            loadingProgress = ao.progress + 0.1f;
            loadingProgress = loadingProgress * timeLoading / minTimeToLoad;

            // Loading completed
            if (loadingProgress >= 1)
            {
                ao.allowSceneActivation = true;
            }

            yield return null;
        }
    }
}