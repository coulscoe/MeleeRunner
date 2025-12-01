using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class LoadingScreen : MonoBehaviour
{

    public UnityEvent onLoaded;

    void Start()
    {
        StartCoroutine(LoadSceneAsync());
    }

    IEnumerator LoadSceneAsync()
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(SceneLoader.sceneToLoad);
        op.allowSceneActivation = false;

        while (!op.isDone)
        {
            if (op.progress >= .9f)
            {
                onLoaded.Invoke();
                yield return new WaitForSeconds(2);
                op.allowSceneActivation = true;
            }
        }
    }
}
