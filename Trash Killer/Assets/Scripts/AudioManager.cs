using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    #region Static Instance
    private static AudioManager instance;
    public static AudioManager Instance
    {
        get {
            if (instance == null)
            {
                instance = FindObjectOfType<AudioManager>();
                if(instance == null)
                {
                    instance = new GameObject("Spawned AudioManager", typeof(AudioManager)).GetComponent<AudioManager>();
                }
            }
            return instance;
        }
        private set
        {
            instance = value;
        }
    }
    #endregion

    #region Fields
    private AudioSource musicSource;
    private AudioSource musicSource2;
    private AudioSource sfxSource;

    private bool firstMusicSourceIsPlaying;
    #endregion

    private void Awake()
    {
        //Asegurarse de no destruir esta instancia.
        DontDestroyOnLoad(this.gameObject);

        //Crear los audio sources, y guardarlos como referencias.
        musicSource = this.gameObject.AddComponent<AudioSource>();
        musicSource2 = this.gameObject.AddComponent<AudioSource>();
        sfxSource = this.gameObject.AddComponent<AudioSource>();

        //Loop los tracks de musica.
        musicSource.loop = true;
        musicSource2.loop = true;
    }

    //Música
    public void PlayMusic(AudioClip musicClip)
    {
        //Determina que fuente de audio está activa.
        AudioSource activeSource = (firstMusicSourceIsPlaying) ? musicSource : musicSource2;

        activeSource.clip = musicClip;
        activeSource.volume = 1;
        activeSource.Play();
    }
    public void PlayMusicWithFade(AudioClip newClip, float musicVolume, float transitionTime = 1.0f)
    {
        //Determina que fuente de audio está activa.
        AudioSource activeSource = (firstMusicSourceIsPlaying) ? musicSource : musicSource2;
        StartCoroutine(UpdateMusicWithFade(activeSource, newClip, transitionTime, musicVolume));
    }
    public void PlayMusicWithCrossFade(AudioClip musicClip, float musicVolume, float transitionTime = 1.0f)
    {
        //Determinar cual está activo.
        AudioSource activeSource = (firstMusicSourceIsPlaying) ? musicSource : musicSource2;
        AudioSource newSource = (firstMusicSourceIsPlaying) ? musicSource2 : musicSource;

        //Cambiar la musica.
        firstMusicSourceIsPlaying = !firstMusicSourceIsPlaying;

        //Cambia los cambios de AudioSOurce, entonces empieza la corutina del crossfade.
        newSource.clip = musicClip;
        newSource.Play();
        StartCoroutine(UpdateMusicWithCrossFade(activeSource, newSource, transitionTime, musicVolume));
    } 
    private IEnumerator UpdateMusicWithFade(AudioSource activeSource, AudioClip newClip, float transitionTime, float musicVolume)
    {
        //Asegurarse de que está sonando.
        if (!activeSource.isPlaying)
            activeSource.Play();

        float t = 0.0f;

        //Fade out
        for (t = 0; t < transitionTime; t += Time.deltaTime)
        { 
            activeSource.volume = (musicVolume - (t / transitionTime)* musicVolume);
            yield return null;
        }

        activeSource.Stop();
        activeSource.clip = newClip;
        activeSource.Play();

        //Fade in
        for (t = 0; t < transitionTime; t += Time.deltaTime)
        {
            activeSource.volume = (t / transitionTime) * musicVolume;
            yield return null;
        }
    }
    private IEnumerator UpdateMusicWithCrossFade(AudioSource original, AudioSource newSource, float transitionTime, float musicVolume)
    {
        float t = 0.0f;

        for (t = 0; t < transitionTime; t += Time.deltaTime)
        {
            original.volume = (musicVolume - (t / transitionTime)* musicVolume);
            newSource.volume = (t / transitionTime)* musicVolume;
            yield return null;
        }

        original.Stop();
    }


    //Efectos
    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }
    public void PlaySFX(AudioClip clip, float volume)
    {
        sfxSource.PlayOneShot(clip, volume);
    }

    //Control volumen
    public void SetMusicVolume(float volumen)
    {
        musicSource.volume = volumen;
        musicSource2.volume = volumen;
    }
    public void SetFXVolume(float volume)
    {
        sfxSource.volume = volume;
    }
}
