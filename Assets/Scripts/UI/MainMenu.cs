using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private string nextSceneName;
    public void PlayGame()
    {
        StartCoroutine(StartGame());
    }

    private IEnumerator StartGame()
    {
        if (FadeController.Instance != null)
            yield return FadeController.Instance.FadeOut();

        SceneManager.LoadScene(nextSceneName);
    }

    public void QuitGame()
    {
        Debug.Log("QUIT");
        Application.Quit();
    }
}
