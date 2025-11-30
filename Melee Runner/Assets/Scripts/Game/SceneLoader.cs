using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    
    public static string sceneToLoad;

    public static void Load(string sceneName)
    {
        sceneToLoad = sceneName;
        SceneManager.LoadScene("Loading Scene");
    }

}
