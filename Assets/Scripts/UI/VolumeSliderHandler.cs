using UnityEngine;
using UnityEngine.UI;

public class VolumeSliderHandler : MonoBehaviour
{
    [SerializeField] private Slider slider;

    void Start()
    {
        slider.value = AudioListener.volume;
        slider.onValueChanged.AddListener(SetVolume);
    }

    private void SetVolume(float value)
    {
        AudioListener.volume = value;
        //Debug.Log("Volume set to " + value);
    }
}
