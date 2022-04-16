using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public AudioClip landing_SFX, pick_SFX, hit_SFX, flying_SFX;

    public GameObject soundObject;

    public AudioMixer sfxMixer;

    private void Awake()
    {
        Instance = this;
    }
    // Start is called before the first frame update
    public void CaseSoundSFX(string caseSFX)
    {
        switch (caseSFX)
        {
            case "landing":
                PlaySoundSFX(landing_SFX);
                break;
            case "picking":
                PlaySoundSFX(pick_SFX);
                break;
            case "hiting":
                PlaySoundSFX(hit_SFX);
                break;
            case "flying":
                PlaySoundSFX(flying_SFX);
                break;
            default:
                break;
        }
    }

    void PlaySoundSFX(AudioClip sfx)
    {
        //Instantiate an object in each case
        GameObject newObject = Instantiate(soundObject, transform);
        //Assign a clip to be played
        newObject.GetComponent<AudioSource>().clip = sfx;
        //Play that sound sfx
        newObject.GetComponent<AudioSource>().Play();
        newObject.GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("sfxVolume");
    }

    public void SettingAudio(float volume)
    {
        sfxMixer.SetFloat("sfxVolume", volume);
        PlayerPrefs.SetFloat("sfxVolume", volume);
    }
}
