using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void onPlayClick()
    {
        SceneManager.LoadScene("Caden_Test");
    }

    public void onExitClick()
    {
        Application.Quit();
    }
}
