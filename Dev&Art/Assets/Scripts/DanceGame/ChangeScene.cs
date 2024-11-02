using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    public void ChangeScenes(int scene)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(scene);
    }
}
