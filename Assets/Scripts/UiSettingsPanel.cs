using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UiSettingsPanel : MonoBehaviour
{
    public Button ExitBTN;
    public Slider MusicSlider;
    public Slider SoundSlider;
    public TMP_Text MusicNumberDisplay;
    public TMP_Text SoundNumberDisplay;
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
        MusicNumberDisplay.text = string.Format("{0}", MyData.MusicVolume.ToString("P1"));
        MyData.SFXVolume = SoundSlider.value;
        SoundNumberDisplay.text = string.Format("{0}", MyData.SFXVolume.ToString("P1"));
        AudioManager.Instance.SetVolume();
    }
}
