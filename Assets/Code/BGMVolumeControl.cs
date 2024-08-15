using UnityEngine;
using UnityEngine.UI;

public class BGMVolumeControl : MonoBehaviour
{
    // Reference to the AudioSource component that plays the BGM
    public AudioSource bgmAudioSource;

    // Reference to the UI Slider used to adjust the volume
    public Slider volumeSlider;

    // Start is called before the first frame update
    void Start()
    {
        // Set the initial volume to the value of the slider (or vice versa)
        volumeSlider.value = bgmAudioSource.volume;

        // Add a listener to the slider to call the OnVolumeChange method when the slider value changes
        volumeSlider.onValueChanged.AddListener(OnVolumeChange);
    }

    // Method called when the slider value changes
    public void OnVolumeChange(float value)
    {
        // Set the volume of the AudioSource to the value of the slider
        bgmAudioSource.volume = value;
    }
}
