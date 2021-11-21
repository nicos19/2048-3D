using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public GameObject Music;
    public GameObject ShiftSoundEffect;
    public GameObject MergeSoundEffect;
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
    private AudioSource _mergeSoundEffect;

    // Start is called before the first frame update
    void Start()
    {
        _music = Music.GetComponent<AudioSource>();
        _shiftSoundEffect = ShiftSoundEffect.GetComponent<AudioSource>();
        _mergeSoundEffect = MergeSoundEffect.GetComponent<AudioSource>();
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
    /// This method plays a sound effect for merging tiles.
    /// </summary>
    public void PlayMergeSoundEffect()
    {
        _mergeSoundEffect.Play();
    }

    /// <summary>
    /// This method mutes or unmutes all sound effects.
    /// </summary>
    public void ToggleSoundEffects()
    {
        _shiftSoundEffect.mute = !_shiftSoundEffect.mute;
        _mergeSoundEffect.mute = !_mergeSoundEffect.mute;

        // change button symbol
        if (SoundEffectsButton.GetComponent<Image>().sprite == SoundOn)
        {
            SoundEffectsButton.GetComponent<Image>().sprite = SoundOff;
        }
        else
        {
            SoundEffectsButton.GetComponent<Image>().sprite = SoundOn;
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
            MusicButton.GetComponent<Image>().sprite = SoundOff;
        }
        else
        {
            MusicButton.GetComponent<Image>().sprite = SoundOn;
        }
    }

}
