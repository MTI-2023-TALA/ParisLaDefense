using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    [SerializeField] Slider volumeSlider;
    private OptionManager optionManager;

    void Start()
    {
        optionManager = GameObject.Find(ObjectName.optionManager).GetComponent<OptionManager>();
        PlayerPrefs.SetFloat("musicVolume", optionManager.volume);

        Load();
    }

    public void ChangeVolume()
    {
        AudioListener.volume = volumeSlider.value;
        optionManager.volume = volumeSlider.value;

        Save();
    }

    public void Load()
    {
        volumeSlider.value = PlayerPrefs.GetFloat("musicVolume");
    }

    private void Save()
    {
        PlayerPrefs.SetFloat("musicVolume", volumeSlider.value);
    }
}
