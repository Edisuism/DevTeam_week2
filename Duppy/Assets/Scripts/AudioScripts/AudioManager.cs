using System;
using UnityEngine;
using UnityEngine.Audio;
using System.Collections;

public class AudioManager : MonoBehaviour
{

	public static AudioManager instance;

	public AudioMixerGroup mixerGroup;

	public Sound[] sounds;

	void Awake()
	{
		if (instance != null)
		{
			Destroy(gameObject);
		}
		else
		{
			instance = this;
			DontDestroyOnLoad(gameObject);
		}

		foreach (Sound s in sounds)
		{
			s.source = gameObject.AddComponent<AudioSource>();
			s.source.clip = s.clip;
			s.source.loop = s.loop;

			s.source.outputAudioMixerGroup = mixerGroup;
		}
	}

    public Sound SoundSearch(string sound)
    {
        Sound s = Array.Find(sounds, item => item.name == sound);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
        }
        return s;
    }

	public void Play(string sound)
	{
        //Sequence of sounds for when the player is caught
        if(sound == "playercaught")
        {
            sound = "whoosh";
            StartCoroutine(PlaySoundAfterPrevious("laugh", sound.Length));
        }

        Sound s = SoundSearch(sound);

		s.source.volume = s.volume * (1f + UnityEngine.Random.Range(-s.volumeVariance / 2f, s.volumeVariance / 2f));
		s.source.pitch = s.pitch * (1f + UnityEngine.Random.Range(-s.pitchVariance / 2f, s.pitchVariance / 2f));

		s.source.Play();
	}

    public void Stop(string sound)
    {
        Sound s = SoundSearch(sound);
        s.source.Stop();
    }

    public bool IsPlaying(string sound)
    {
        Sound s = SoundSearch(sound);
        return s.source.isPlaying;
    }

    IEnumerator PlaySoundAfterPrevious(string sound, float previousSoundLength)
    {
        //I want a bit of overlap for the sounds so I divide the length
        yield return new WaitForSeconds(previousSoundLength / 1.5f);
        if(sound == "laugh")
        {
            Stop("rapidbeat");
            StartCoroutine(PlaySoundAfterPrevious("gameover", sound.Length));
        }
        Play(sound);
    }

    //Called from Ghost Audio to adjust the volume based on player distance
    public void AdjustGhostVolume(string sound, float newVolume)
    {
        Sound s = SoundSearch(sound);

        s.source.volume = newVolume;

        if (!IsPlaying("ghostnoise"))
        {
            s.source.Play();
        }
    }

    //Called from Player Audio to adjust the heartbeat rate
    public void PlayHeartBeat(string sound)
    {
        if (sound != "stop")
        {
            switch (sound)
            {
                case "rapidbeat":
                    Stop("fastbeat");
                    Stop("slowbeat");
                    break;
                case "fastbeat":
                    Stop("rapidbeat");
                    Stop("slowbeat");
                    break;
                case "slowbeat":
                    Stop("fastbeat");
                    Stop("rapidbeat");
                    break;
                default:
                    Debug.LogWarning("Unknown heartbeat speed passed in");
                    break;
            }

            if (!IsPlaying(sound))
                Play(sound);
        }
        else
        {
            Stop("rapidbeat");
            Stop("fastbeat");
            Stop("slowbeat");
        }

    }
}
