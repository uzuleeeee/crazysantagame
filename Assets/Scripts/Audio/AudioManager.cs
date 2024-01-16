using UnityEngine;
using UnityEngine.Audio;
using System;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

    public static AudioManager instance;

    public bool bounce;

    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        foreach (Sound s in sounds) {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    void Start() {
        Play("BGM");
        Play("HeartBeat");
    }

    void Update() {
        if (bounce) {
            foreach (Sound s in sounds) {
                if (s.name != "Static") {
                    s.source.pitch = Time.timeScale;
                } else {
                    s.source.volume = Mathf.Min(1, (1/-0.5f) * (Time.timeScale - 1));
                }
            }
        }

        bounce = Time.timeScale != 1;
    }

    public void Play(string name) {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null) {
            return;
        }
        s.source.Play();
    }
}
