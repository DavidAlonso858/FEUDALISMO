using UnityEngine;

public class GamePanelDeath : MonoBehaviour
{
    public static GamePanelDeath instance;

    private void Awake()
    {
        instance = this;
    }

    public void Show()
    {
        Time.timeScale = 0f;
        gameObject.SetActive(true);
        Debug.Log("JUEGO TERMINADO - DERROTA");
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