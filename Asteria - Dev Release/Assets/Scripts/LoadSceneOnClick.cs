using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneOnClick : MonoBehaviour
{
    public string sceneName; // Name of the scene to load

    // This method will be called when the button is clicked
    public void LoadScene()
    {
        SceneManager.LoadScene(sceneName);
    }
}
