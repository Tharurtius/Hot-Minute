using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    /// <summary>
    ///Allows you to play a better game
    /// </summary>
    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }
    /// <summary>
    /// Loads scene by index
    /// </summary>
    /// <param name="scene">The index of the scene you want to load</param>
    public void ChangeScene(int scene)
    {
        SceneManager.LoadScene(scene);
    }
    /// <summary>
    /// Reloads current scene
    /// </summary>
    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
