using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder;

public class PauseMenuController : MonoBehaviour
{
    public static bool gamePaused;

    [SerializeField]
    private InputManager inputManager;
    
    [SerializeField]
    private CursorDisable cursorDisable;

    [SerializeField]
    private GameObject pauseMenuUI;


    [Header("Additional Ui Components")]
    [SerializeField]
    private GameObject healthBar;
    [SerializeField]
    private GameObject itemCounter;
    [SerializeField]
    private GameObject itemDisplay;

    

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(gamePaused)
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
        healthBar.SetActive(true);
        itemCounter.SetActive(true);
        itemDisplay.SetActive(true);
        inputManager.enabled = true;
        cursorDisable.enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1f;
        gamePaused = false;
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        healthBar.SetActive(false);
        itemCounter.SetActive(false);
        itemDisplay.SetActive(false);
        inputManager.enabled = false;
        cursorDisable.enabled = false;
        Cursor.lockState = CursorLockMode.Confined;
        Time.timeScale = Mathf.Epsilon;
        gamePaused = true;
    }
}
