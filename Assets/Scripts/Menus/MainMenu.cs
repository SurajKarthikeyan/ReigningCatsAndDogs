using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private string sceneName;
    [SerializeField] private Button playButton;
    [SerializeField] private Button quitButton;

    // Start is called before the first frame update
    void Start()
    {
        playButton.onClick.AddListener(OnPlayClick);
        quitButton.onClick.AddListener(OnQuitButton);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnPlayClick()
    {
        SceneManager.LoadScene(sceneName);
    }

    private void OnQuitButton()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif   
    }
}
