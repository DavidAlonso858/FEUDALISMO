using UnityEngine;

public class GamePanelVictory : MonoBehaviour
{
    public static GamePanelVictory instance;

    private void Awake()
    {
        instance = this;
        gameObject.SetActive(false); // empieza oculto de por si
    }

    public void Show()
    {
        Time.timeScale = 0f;
        gameObject.SetActive(true);
        Debug.Log("JUEGO TERMINADO - VICTORIA");
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
