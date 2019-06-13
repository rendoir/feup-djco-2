using UnityEngine;
using UnityEngine.Audio;
using System;

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

	void Start() {
        Play("MainTheme");
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
}
