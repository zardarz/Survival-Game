using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void GoToMainMenu() {
        SceneManager.LoadScene("Main Menu");
        Time.timeScale = 1f;
        InventoryManager.UpdateInventoryBuffers();
    }

    public void GoToGame() {
        SceneManager.LoadScene("SampleScene");
        Time.timeScale = 1f;
    }

    public void Quit() {
        Application.Quit();
    }
}