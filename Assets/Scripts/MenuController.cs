using UnityEngine;
using UnityEngine.SceneManagement;
public class MenuController : MonoBehaviour
{
    public GameObject pauseMenu;

    private bool isPaused = false;

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
