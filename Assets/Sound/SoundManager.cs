using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public Slider volumeSlider;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded;

    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Game")
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        if(PlayerPrefs.HasKey("soundVolume"))
        {
            LoadVolume();
        } else {
            PlayerPrefs.SetFloat("soundVolume", 1);
            LoadVolume();
        }
    }

    public void setVolume()
    {
        AudioListener.volume = volumeSlider.value;
        SaveVolume();
    }

    public void SaveVolume()
    {
        PlayerPrefs.SetFloat("soundVolume", volumeSlider.value);
    }

    public void LoadVolume()
    {
        volumeSlider.value = PlayerPrefs.GetFloat("soundVolume");
    }
}
