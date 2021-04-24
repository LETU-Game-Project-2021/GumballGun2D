using UnityEngine.Audio;
using System;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    [SerializeField]
    public Sound[] effects;
    //public GameObject pauseMenu;

    public static SoundManager instance;

    private static Slider[] sliders;
    private float masterVolume=1, musicVolume=1, sfxVolume=1;
    private bool firstTimeSetup = true;
    private static AudioSource[] sources;

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
            //Debug.Log("Creation: " + s.soundName + ".source = " + s.source);
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

    public void masterSlider(float master) {
        masterVolume = master;
        volumeSlider();
    }

    public void sfxSlider(float sfx) {
        sfxVolume = sfx;
        volumeSlider();
    }

    public void musicSlider(float music) {
        musicVolume = music;
        volumeSlider();
    }
    public void volumeSlider() {
        if(firstTimeSetup) {
            //sliders = GameObject.Find("PauseMenuCanvas").GetComponentsInChildren<Slider>();
            //Debug.Log("Found " + sliders.Length + " sliders");
            sources = instance.GetComponents<AudioSource>();

            sliders = GameObject.Find("PauseMenuCanvas").GetComponentsInChildren<Slider>();
            //Debug.Log(sources.Length);
        }
        if(sliders.Length >= 3) {
            for(int i = 0; i < sources.Length; i++) {
                //foreach(AudioSource s in sources) {
                //s.source.volume = s.volume * masterVolume * (s.isSFX ? sfxVolume : musicVolume);
                sources[i].volume = effects[i].volume * sliders[0].value * (effects[i].isSFX ? sliders[1].value : sliders[2].value);
            }
        }
    }
}
