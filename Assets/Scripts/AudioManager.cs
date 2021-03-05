using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public enum SoundEffectsType { None=0,Hover,Click,PopUP,Wrong,Correct,Infected,Completed,Footsteps,Attack}
public enum MusicEffectsType { None=0,MainMenu,GamePlay,GameOver}
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { private set; get; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
            return;
        }

        foreach(SoundFX s in SoundFx)
        {
            _soundsDict[s.SFXType] = s;
        }

        foreach(MusicFX m in MusicFx)
        {
            _musicDict[m.MFXType] = m;
        }
    }
    private void OnDestroy()
    {
        if (Instance == this.gameObject)
            Instance = null;
    }

    [Header("SoundFX")]
    public SoundFX[] SoundFx;
    [Header("MusicFX")]
    public MusicFX[] MusicFx;
    [Header("Sources and Snapshots")]
    public List<AudioSource> ToggleSources = new List<AudioSource>();
    public AudioMixerGroup[] MixerGroup;


    private Dictionary<SoundEffectsType, SoundFX> _soundsDict = new Dictionary<SoundEffectsType, SoundFX>();
    private Dictionary<MusicEffectsType, MusicFX> _musicDict = new Dictionary<MusicEffectsType, MusicFX>();

    public void SetVolume()
    {
        foreach(AudioSource s in ToggleSources)
        {
            s.volume = MyData.MusicVolume;
        }
    }
    public void SetVolume(float newVolume)
    {
        foreach (AudioSource s in ToggleSources)
        {
            s.volume = newVolume;
        }
    }
    public void PlaySound(SoundEffectsType type,AudioSource source,float volumemod =1)
    {
        if (type == SoundEffectsType.None)
            return;
        SoundFX data;
        if (_soundsDict.TryGetValue(type, out data))
        {
            AudioClip c = data.clip;
            if(c!=null && source.clip != c)
            {
                source.clip = c;
                source.volume = MyData.SFXVolume;
                source.Play();
            }
        }
    }
    public void PlaySound(SoundEffectsType type,Vector3?pos = null,float volumemod=1)
    {
        if (type == SoundEffectsType.None)
            return;
        SoundFX data;
        if(_soundsDict.TryGetValue(type,out data))
        {
            Vector3 at;
            if (pos == null)
                at = Camera.main.GetComponent<AudioListener>().transform.position;
            else
                at = pos.Value;
            AudioClip c = data.clip;
            if (c != null)
                AudioSource.PlayClipAtPoint(c, at, MyData.SFXVolume * volumemod);
        }
    }
    public void PlayMusic(MusicEffectsType type, bool force = true)
    {
        if (type == MusicEffectsType.None)
            return;
        AudioSource activeSource = ToggleSources[1];
        if (!activeSource.isPlaying || force)
        {
            MusicFX data = _musicDict[type];
            activeSource.clip = data.clip;
            activeSource.Stop();
            activeSource.Play();
            activeSource.loop = true;
            //TODO add SnapShot TransitionTo logic
        }
    }
}
[System.Serializable]
public class SoundFX
{
    public SoundEffectsType SFXType;
    public AudioClip clip;
}
[System.Serializable]
public class MusicFX
{
    public MusicEffectsType MFXType;
    public AudioClip clip;
}
