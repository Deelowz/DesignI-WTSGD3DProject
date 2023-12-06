using UnityEngine;
using UnityEngine.UI;

public class VolumeManager : MonoBehaviour
{
    public static VolumeManager instance; // Singleton instance
    private Slider volumeSlider; // Reference to the volume slider

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject); // Persist across scenes

        // Find the volume slider in the scene
        volumeSlider = GameObject.Find("VolumeSlider").GetComponent<Slider>();

        // Set the initial volume value from PlayerPrefs or default to 0.5f
        float currentVolume = PlayerPrefs.GetFloat("Volume", 0.5f);
        volumeSlider.value = currentVolume;
        SetVolume(currentVolume); // Apply initial volume
    }

    // Function to apply the volume to AudioSources
    public void SetVolume(float volume)
    {
        AudioSource[] allAudioSources = FindObjectsOfType<AudioSource>();
        foreach (AudioSource audioSource in allAudioSources)
        {
            audioSource.volume = volume;
        }
        PlayerPrefs.SetFloat("Volume", volume); // Save the volume value
        PlayerPrefs.Save(); // Save PlayerPrefs immediately
    }

    // Function to retrieve the current volume level
    public float GetCurrentVolume()
    {
        return volumeSlider.value;
    }
}
