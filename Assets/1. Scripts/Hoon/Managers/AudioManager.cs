using UnityEngine;
using System;

public enum SoundName
{
    bgm1, bgm2,
    dig, dig_fail, dig_success,
    whistel1, whistel2,
    lock_open, lock_explode,
    snap_item, key_spawn, open_final_door, spit,
    speak_merchant, speak_guard
}

[System.Serializable]
public class Sound
{
    public SoundName name;
    public AudioClip clip;
    [Range(0f, 1f)] public float volume = 1f;
    [Range(.1f, 3f)] public float pitch = 1f;
    public bool loop;
    [HideInInspector] public AudioSource source;
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("사운드 리스트 등록")]
    [SerializeField] private Sound[] sounds;

    // 전체 볼륨 배율 (0~1)
    private float _bgmVolume = 1f;
    private float _sfxVolume = 1f;

    private bool _bgmMuted = false;
    private bool _sfxMuted = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.source.spatialBlend = 0f;

            // 초기 볼륨 적용
            if (s.loop)
                s.source.volume = s.volume * _bgmVolume;
            else
                s.source.volume = s.volume * _sfxVolume;
        }

        Play2D(SoundName.bgm1);
    }

    // === 볼륨 설정 (슬라이더에서 호출) ===
    public void SetBGMVolume(float value)
    {
        _bgmVolume = value;
        // 현재 재생 중인 BGM만 즉시 반영
        foreach (Sound s in sounds)
        {
            if (s.loop && s.source != null)
                s.source.volume = s.volume * _bgmVolume;
        }
    }

    public void SetSFXVolume(float value)
    {
        _sfxVolume = value;
    }

    public float GetBGMVolume() => _bgmVolume;
    public float GetSFXVolume() => _sfxVolume;


    public void ToggleBGMMute()
    {
        _bgmMuted = !_bgmMuted;
        foreach (Sound s in sounds)
        {
            if (s.loop && s.source != null)
                s.source.volume = _bgmMuted ? 0f : s.volume * _bgmVolume;
        }
    }

    public void ToggleSFXMute()
    {
        _sfxMuted = !_sfxMuted;
    }

    public bool IsBGMMuted() => _bgmMuted;
    public bool IsSFXMuted() => _sfxMuted;

    // === 재생 ===

    public void Play2D(SoundName name)
    {
        Sound s = Array.Find(sounds, item => item.name == name);
        if (s == null)
        {
            Debug.LogWarning($"AudioManager: {name} 사운드 없음!");
            return;
        }

        if (s.loop)
        {
            if (_bgmMuted) return;
            foreach (Sound otherSound in sounds)
            {
                if (otherSound.loop && otherSound.source != null && otherSound.source.isPlaying)
                {
                    otherSound.source.Stop();
                }
            }
            s.source.volume = s.volume * _bgmVolume;
            s.source.Play();
        }
        else
        {
            if (_sfxMuted) return;
            s.source.PlayOneShot(s.source.clip, s.volume * _sfxVolume);
        }
    }

    public void Stop2D(SoundName name)
    {
        Sound s = Array.Find(sounds, item => item.name == name);
        if (s != null) s.source.Stop();
    }

    public void Play3D(SoundName name, Vector3 position)
    {
        if (_sfxMuted) return;
        Sound s = Array.Find(sounds, item => item.name == name);
        if (s == null)
        {
            Debug.LogWarning($"AudioManager: {name} 사운드 없음!");
            return;
        }

        GameObject tempAudioGo = new GameObject("Temp3DAudio_" + name);
        tempAudioGo.transform.position = position;

        AudioSource aSource = tempAudioGo.AddComponent<AudioSource>();
        aSource.clip = s.clip;
        aSource.volume = s.volume * _sfxVolume;
        aSource.pitch = s.pitch;
        aSource.spatialBlend = 1.0f;
        aSource.minDistance = 1.0f;
        aSource.maxDistance = 15.0f;
        aSource.rolloffMode = AudioRolloffMode.Logarithmic;
        aSource.Play();

        Destroy(tempAudioGo, aSource.clip.length);
    }

    public void PlayNPCVoice(SoundName name, float minPitch = 0.9f, float maxPitch = 1.2f)
    {
        if (_sfxMuted) return;
        Sound s = Array.Find(sounds, item => item.name == name);
        if (s == null) return;

        // 소리를 낼 때마다 오디오 소스의 피치를 랜덤하게 조절해서 웅얼거리는 느낌을 줌
        s.source.pitch = UnityEngine.Random.Range(minPitch, maxPitch);
        s.source.PlayOneShot(s.source.clip, s.volume * _sfxVolume);
    }
}