using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public GameObject Music;
    public GameObject ShiftSoundEffect;
    public GameObject MusicButton;
    public GameObject SoundEffectsButton;
    public Sprite SoundOn;
    public Sprite SoundOff;

    /// <value>
    /// The property <c>ShiftSoundPlayedThisRound</c> is <c>true</c> if <c>_shiftSoundEffect.Play()</c> 
    /// has already been called this round.
    /// </value>
    public bool ShiftSoundPlayedThisRound { get; set; }

    private AudioSource _music;
    private AudioSource _shiftSoundEffect;

    // Start is called before the first frame update
    void Start()
    {
        _music = Music.GetComponent<AudioSource>();
        _shiftSoundEffect = ShiftSoundEffect.GetComponent<AudioSource>();
    }

    /// <summary>
    /// This method plays a sound effect for shifting tiles.
    /// </summary>
    public void PlayShiftSoundEffect()
    {
        if (ShiftSoundPlayedThisRound)
        {
            // only play the shift sound effect once per round.
            return;
        }
        _shiftSoundEffect.Play();
        ShiftSoundPlayedThisRound = true;
    }

    /// <summary>
    /// This method mutes or unmutes all sound effects.
    /// </summary>
    public void ToggleSoundEffects()
    {
        _shiftSoundEffect.mute = !_shiftSoundEffect.mute;

        // change button symbol
        if (SoundEffectsButton.GetComponent<Image>().sprite == SoundOn)
        {
            SetButtonSprite(SoundEffectsButton, SoundOff);
            AudioSettings.SaveAudioSetting("soundEffects", 0);
        }
        else
        {
            SetButtonSprite(SoundEffectsButton, SoundOn);
            AudioSettings.SaveAudioSetting("soundEffects", 1);
        }
    }

    /// <summary>
    /// This method mutes or unmutes the music.
    /// </summary>
    public void ToggleMusic()
    {
        _music.mute = !_music.mute;

        // change button symbol
        if (MusicButton.GetComponent<Image>().sprite == SoundOn)
        {
            SetButtonSprite(MusicButton, SoundOff);
            AudioSettings.SaveAudioSetting("music", 0);
        }
        else
        {
            SetButtonSprite(MusicButton, SoundOn);
            AudioSettings.SaveAudioSetting("music", 1);
        }
    }

    /// <summary>
    /// This method sets the sprite representing <c>ButtonGameObject</c> in the game.
    /// </summary>
    /// <param name="ButtonGameObject"></param>
    /// <param name="sprite"></param>
    public void SetButtonSprite(GameObject ButtonGameObject, Sprite sprite)
    {
        ButtonGameObject.GetComponent<Image>().sprite = sprite;
    }

}
