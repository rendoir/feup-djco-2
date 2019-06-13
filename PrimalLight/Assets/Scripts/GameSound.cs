using UnityEngine;
using System;
using System.Collections;

public class GameSound : MonoBehaviour
{
    //This class should only be used for 2D sounds

    static GameSound current;
    
    public AudioSource[] audioSources;

    void Awake()
	{
		//If a Game Sound exists and this isn't it...
		if (current != null && current != this)
		{
			//...destroy this and exit. There can only be one Game Sound
			Destroy(gameObject);
			return;
		}

		//Set this as the current game sound
		current = this;

		//Persist this object between scene reloads
		DontDestroyOnLoad(gameObject);
	}

    static public void Play(string name) {
        AudioSource source = Array.Find(current.audioSources, s => s.clip.name == name);
        if(source != null) {
            source.Play();
        }
    }

    static public void Stop(string name) {
        AudioSource source = Array.Find(current.audioSources, s => s.clip.name == name);
        if(source != null) {
            source.Stop();
        }
    }

    public static IEnumerator FadeIn(string name) {
        AudioSource source = Array.Find(current.audioSources, s => s.clip.name == name);
        float startVolume = source.volume;
        source.volume = 0f;
        source.Play();
 
        while (source.volume < startVolume) {
            source.volume += startVolume * Time.deltaTime / 5f;
 
            yield return null;
        }
 
        source.volume = startVolume;
    }

    public static IEnumerator FadeOut(string name) {
        AudioSource source = Array.Find(current.audioSources, s => s.clip.name == name);
        float startVolume = source.volume;
 
        while (source.volume > 0) {
            source.volume -= startVolume * Time.deltaTime / 5f;
 
            yield return null;
        }
 
        source.Stop();
        source.volume = startVolume;
    }
}
