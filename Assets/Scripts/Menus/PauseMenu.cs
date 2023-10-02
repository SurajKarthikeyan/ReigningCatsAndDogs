using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public static bool isPaused = false;
    
    public GameObject pauseMenuUI;

    [SerializeField] private Button resumeButton;
    [SerializeField] private Button mainMenuButton;


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        resumeButton.onClick.AddListener(OnResumeClick);
        mainMenuButton.onClick.AddListener(OnMainMenuClick);
        isPaused = true;
    }

    public void LoadMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu"); // replace "MainMenu" with the name of your main menu scene
    }

    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        Application.Quit();
    }

    private void OnResumeClick()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    private void OnMainMenuClick()
    {
        Time.timeScale = 1f;
        resumeButton.onClick.RemoveListener(OnResumeClick);
        mainMenuButton.onClick.RemoveListener(OnMainMenuClick);
        isPaused = false;
        SceneManager.LoadScene("MainMenu"); // replace "MainMenu" with the name of your main menu scene
    }
}
