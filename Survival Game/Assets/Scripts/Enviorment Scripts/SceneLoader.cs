using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void GoToMainMenu() {
        SceneManager.LoadScene("Main Menu");
    }

    public void GoToGame() {
        SceneManager.LoadScene("SampleScene");
    }

    public void Quit() {
        Application.Quit();
    }
}