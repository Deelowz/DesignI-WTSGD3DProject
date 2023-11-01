using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager: MonoBehaviour
{
    public GameObject backButtonUI;

    public void MainMenu()
    {
        // Load LevelOne scene
        SceneManager.LoadScene("MainMenu");
    }

    public void Play()
    {
        // Load LevelOne scene
        SceneManager.LoadScene("Level1");
    }
    public void Level2()
    {
        //If Level1 is complete, load Level2
        SceneManager.LoadScene("Level2");
    }

    public void Quit()
    {
        // Quit the game
        Application.Quit();
    }
    public void Settings()
    {
        // Load Settings scene
        SceneManager.LoadScene("Settings");
    }
    public void Credits()
    {
        // Load Credits scene
        SceneManager.LoadScene("Credits");
    }

    public void GoBack()
    {
        // Show the main menu and hide the back button
        MainMenu();
        backButtonUI.SetActive(false);
    }

    public void ShowBackButton()
    {
        // Show the back button and hide the main menu(change to current menu and add scenes)
        MainMenu();
        Settings();
        Credits();
        backButtonUI.SetActive(true);
    }
}
