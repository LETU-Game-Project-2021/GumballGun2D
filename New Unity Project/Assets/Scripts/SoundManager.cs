using UnityEngine.Audio;
using System;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField]
    public Sound[] effects;

    public static SoundManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
        foreach (Sound s in effects) {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    private void Start()
    {
        int rand = UnityEngine.Random.Range(0,2);
        switch (rand) {
            case 0:
                Play("Theme");
                break;
            case 1:
                Play("Theme2");
                break;
        }
    }

    public void Play(string name) {
        Sound s = Array.Find(effects, sound => sound.soundName == name);
        s.source.Play();
    }
}
