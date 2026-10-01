using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
public class MenuController : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject firstButton;

    private bool isPaused = false;
    private void Start()
    {
        pauseMenu.SetActive(false);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ContinueGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        pauseMenu.SetActive(true);

        Time.timeScale = 0f;

        isPaused = true;

        EventSystem.current.SetSelectedGameObject(firstButton);
     }

    public void ContinueGame()
    {
        pauseMenu.SetActive(false);

        Time.timeScale = 1f;

        isPaused = false;
    }

    public void ExitGame()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene("Home");
    }
}
