using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScreen : MonoBehaviour
{
    public void RestartButton()
    {
        SceneManager.LoadScene("Lvl1");
    }

    public void ExitButton()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
