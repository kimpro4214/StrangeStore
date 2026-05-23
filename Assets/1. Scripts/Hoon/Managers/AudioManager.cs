using UnityEngine;
using System;

public enum SoundName {
    bgm1,
    dig,
    dig_fail,
    dig_success,
    whistel1,
    whistel2,
    lock_open,
    lock_explode,
    snap_item
}

// 인스펙터에서 세팅할 수 있도록 직렬화
[System.Serializable]
public class Sound
{
    public SoundName name;            // 소리 이름 (예: "ShovelHit", "KeyUnlock")
    public AudioClip clip;         // 실제 오디오 파일
    [Range(0f, 1f)] public float volume = 1f;
    [Range(.1f, 3f)] public float pitch = 1f;
    public bool loop;              // 반복 재생 여부 (BGM용)

    [HideInInspector] public AudioSource source; // 2D 오디오 전용 소스
}

public class AudioManager : MonoBehaviour
{
    // 어디서나 접근 가능한 싱글톤 인스턴스
    public static AudioManager Instance { get; private set; }

    [Header("사운드 리스트 등록")]
    [SerializeField] private Sound[] sounds;

    private void Awake()
    {
        // 싱글톤 초기화 및 씬 전환 시 파괴 방지
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

        // 등록된 모든 사운드에 대해 기본 2D AudioSource 생성 및 초기화
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.source.spatialBlend = 0f; // 0은 완벽한 2D (헤드폰 전체에서 들림)
        }
        Play2D(SoundName.bgm1);
    }

    // 2D 사운드 재생 (BGM, UI 클릭음)
    public void Play2D(SoundName name)
    {
        Sound s = Array.Find(sounds, item => item.name == name);
        if (s == null)
        {
            Debug.LogWarning($"AudioManager: {name} 이라는 이름의 사운드를 찾을 수 없습니다!");
            return;
        }

        if (s.loop) s.source.Play();
        else s.source.PlayOneShot(s.source.clip);
    }

    // 2D 사운드 정지
    public void Stop2D(SoundName name)
    {
        Sound s = Array.Find(sounds, item => item.name == name);
        if (s != null) s.source.Stop();
    }

    // 3D 사운드 재생 (삽질 소리, 자물쇠 돌리는 소리, 상인 소리 등)
    public void Play3D(SoundName name, Vector3 position)
    {
        Sound s = Array.Find(sounds, item => item.name == name);
        if (s == null)
        {
            Debug.LogWarning($"AudioManager: {name} 이라는 이름의 사운드를 찾을 수 없습니다!");
            return;
        }

        // 소리가 날 위치에 임시 게임 오브젝트 생성
        GameObject tempAudioGo = new GameObject("Temp3DAudio_" + name);
        tempAudioGo.transform.position = position;

        // 컴포넌트 추가 및 오디오 세팅 복사
        AudioSource aSource = tempAudioGo.AddComponent<AudioSource>();
        aSource.clip = s.clip;
        aSource.volume = s.volume;
        aSource.pitch = s.pitch;

        aSource.spatialBlend = 1.0f;          // 1.0은 완벽한 3D 공간음향
        aSource.minDistance = 1.0f;           // 이 거리보다 가까우면 최대 볼륨
        aSource.maxDistance = 15.0f;          // 이 거리보다 멀어지면 안 들림
        aSource.rolloffMode = AudioRolloffMode.Logarithmic; // 거리에 따른 자연스러운 감쇄

        aSource.Play();

        // 소리 재생이 끝나면 자동으로 임시 오브젝트 삭제
        Destroy(tempAudioGo, aSource.clip.length);
    }
}