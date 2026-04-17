using UnityEngine;
using UnityEngine.SceneManagement;

public class PressAnyKeyToStart : MonoBehaviour
{
    private bool keyPressed = false;

    private void Update()
    {
        if(!keyPressed && Input.anyKeyDown)
        {
            keyPressed = true;
            SceneManager.LoadScene("MainMenu");
        }
    }
}
