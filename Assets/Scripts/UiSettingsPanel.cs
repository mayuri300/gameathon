using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiSettingsPanel : MonoBehaviour
{
    public Button ExitBTN;
    public Slider MusicSlider;
    public Slider SoundSlider;
    // Start is called before the first frame update
    void Start()
    {
        ExitBTN.onClick.AddListener(ExitSettings);
        MusicSlider.onValueChanged.AddListener(delegate { VolumeControl(); });
        SoundSlider.onValueChanged.AddListener(delegate { VolumeControl(); });
    }
    private void OnDestroy()
    {
        ExitBTN.onClick.RemoveAllListeners();
    }
  
    public void ExitSettings()
    {
        Destroy(this.gameObject);
    }
    public void VolumeControl()
    {
        MyData.MusicVolume = MusicSlider.value;
        MyData.SFXVolume = SoundSlider.value;
        AudioManager.Instance.SetVolume();
    }
}
