using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSettings : MonoBehaviour
{
    public GameObject MusicGameObject;
    public GameObject ShiftSoundEffectGameObject;
    public GameObject ButtonSoundEffectGameObject;

    private void Start()
    {
        AudioManager AUDIO_MANAGER = gameObject.GetComponent<AudioManager>();

        if (PlayerPrefs.HasKey("music"))
        {
            // saved music setting found -> load this setting
            if (PlayerPrefs.GetInt("music") == 0)
            {
                // music shall be muted
                MusicGameObject.GetComponent<AudioSource>().mute = true;
                AUDIO_MANAGER.SetButtonSprite(AUDIO_MANAGER.MusicButton, AUDIO_MANAGER.SoundOff);
            }
            else
            {
                // music shall not be muted
                MusicGameObject.GetComponent<AudioSource>().mute = false;
            }
        }

        if (PlayerPrefs.HasKey("soundEffects"))
        {
            // saved sound effects setting found -> load this setting
            if (PlayerPrefs.GetInt("soundEffects") == 0)
            {
                // sound effects shall be deactivated
                ShiftSoundEffectGameObject.GetComponent<AudioSource>().mute = true;
                ButtonSoundEffectGameObject.GetComponent<AudioSource>().mute = true;
                AUDIO_MANAGER.SetButtonSprite(AUDIO_MANAGER.SoundEffectsButton, AUDIO_MANAGER.SoundOff);
            }
            else
            {
                // sound effects shall be activated
                ShiftSoundEffectGameObject.GetComponent<AudioSource>().mute = false;
                ButtonSoundEffectGameObject.GetComponent<AudioSource>().mute = false;
            }
        }
    }

    /// <summary>
    /// This methods sets the PlayerPref with keyName <c>setting</c> to value <c>settingActive</c>.
    /// </summary>
    /// <param name="setting"></param>
    /// <param name="settingActive"></param>
    public static void SaveAudioSetting(string setting, int settingActive)
    {
        PlayerPrefs.SetInt(setting, settingActive);
    }
}
