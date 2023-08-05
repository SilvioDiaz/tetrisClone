using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour{

    public bool m_musicEnabled = true;

    public bool m_fxEnabled = true;

    [Range(0,1)]public float m_musicVolume = 1.0f;

    [Range(0,1)]public float m_fxVolume = 1.0f;

    public AudioClip m_clearRowSound;

    public AudioClip m_moveSound;

    public AudioClip m_dropSound;

    public AudioClip m_gameOverSound;

    public AudioClip m_gameOverVocal;

    public AudioClip m_errorSound;

    public AudioClip[] m_vocalSound;

    public AudioClip[] m_musicClips;
        
    public AudioSource m_musicSource;

    AudioClip m_randomMusicClip;

    public iconToggle m_fxIconToggle;

    public iconToggle m_musicIconToggle;


    // Start is called before the first frame update
    void Start(){
        if(m_musicEnabled && m_musicSource && m_musicClips?.Length > 0){
            m_randomMusicClip = GetRamdomClip(m_musicClips);
            PlayBackgroundMusic(m_randomMusicClip);
        }
    }

    public AudioClip GetRamdomClip(AudioClip[] clips){
        AudioClip ramdomClip = clips[Random.Range(0, clips.Length)];
        return ramdomClip;
    }

    // Update is called once per frame
    void Update(){

    }

    public void PlayBackgroundMusic(AudioClip musicClip){
        m_musicSource.Stop();
        m_musicSource.clip = musicClip;
        m_musicSource.volume = m_musicVolume;
        m_musicSource.loop = true;
        m_musicSource.Play();
    }

    void UpdateMusic(){
        if(m_musicSource.isPlaying != m_musicEnabled && m_musicSource){
            if(m_musicEnabled){
                m_randomMusicClip = GetRamdomClip(m_musicClips);
                PlayBackgroundMusic(m_randomMusicClip);
            }else{
                m_musicSource.Stop();
            }
        }
    }

    public void ToggleMusic(){
        m_musicEnabled = !m_musicEnabled;
        if(m_musicIconToggle) ChangeIcon(m_musicIconToggle,m_musicEnabled);
        UpdateMusic();
    }

    public void ToggleFx(){
        m_fxEnabled = !m_fxEnabled;
        if(m_fxIconToggle) ChangeIcon(m_fxIconToggle,m_fxEnabled);
    }

    public void ChangeIcon(iconToggle icon, bool status){
        icon.ToggleIcon(status);
    }
}
