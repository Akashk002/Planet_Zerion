using UnityEngine;
using System.Collections.Generic;
using System;

public class AudioManager : MonoBehaviour
{
    [Header("Settings")]
    public int poolSize = 10;
    public AudioSource prefab3DAudioSource;
    public List<SoundData> soundDataList;

    private Queue<AudioSource> audioSourcePool = new Queue<AudioSource>();

    private void Awake()
    {
        CreatePool();
    }
    private void Start()
    {
        PlayLoopingAt(GameAudioType.EnvironmentBG, Vector3.zero, 0.5f);
    }

    private void CreatePool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            AudioSource source = Instantiate(prefab3DAudioSource, transform);
            source.gameObject.SetActive(false);
            audioSourcePool.Enqueue(source);
        }
    }

    private AudioSource GetPooledSource()
    {
        if (audioSourcePool.Count > 0)
        {
            AudioSource src = audioSourcePool.Dequeue();
            src.gameObject.SetActive(true);
            return src;
        }

        // Expand pool dynamically if needed
        AudioSource extraSource = Instantiate(prefab3DAudioSource, transform);
        return extraSource;
    }

    private void ReturnSource(AudioSource source)
    {
        source.Stop();
        source.clip = null;
        source.loop = false;
        source.gameObject.SetActive(false);
        audioSourcePool.Enqueue(source);
    }

    // 🔊 Play One-shot 3D sound
    public AudioSource PlayOneShotAt(GameAudioType gameAudioType, Vector3 position, float volume = 1f)
    {
        AudioClip clip = soundDataList.Find(data => data.gameAudioType == gameAudioType)?.audioClip;

        AudioSource source = GetPooledSource();
        source.transform.position = position;
        source.clip = clip;
        source.volume = volume;
        source.loop = false;
        source.spatialBlend = 1f; // 3D sound
        source.Play();

        StartCoroutine(ReturnAfterFinish(source));
        return source;
    }

    // 🔁 Looping 3D sound at a position
    public AudioSource PlayLoopingAt(GameAudioType gameAudioType, Vector3 position, float volume = 1f)
    {
        AudioClip clip = soundDataList.Find(data => data.gameAudioType == gameAudioType)?.audioClip;
        AudioSource source = GetPooledSource();
        source.transform.position = position;
        source.clip = clip;
        source.volume = volume;
        source.loop = true;
        source.spatialBlend = 1f;
        source.Play();

        return source;
    }

    // ⏹ Stop a specific sound manually
    public void StopSound(AudioSource source)
    {
        if (source != null && source.isPlaying)
        {
            source.Stop();
            ReturnSource(source);

        }
    }

    // Auto return to pool after playing
    private System.Collections.IEnumerator ReturnAfterFinish(AudioSource source)
    {
        yield return new WaitUntil(() => !source.isPlaying);
        ReturnSource(source);
    }
}


[Serializable]
public class SoundData
{
    public GameAudioType gameAudioType;
    public AudioClip audioClip;
}

public enum SoundType
{
    SFX,
    Music,
    Ambient,
    UI
}

public enum GameAudioType
{
    ClickButton,
    CollectRock,
    DroneMoving,
    Emergency,
    EnemySpacecraftMoving,
    EnterRoom,
    EnvironmentBG,
    ExitRoom,
    MissileBlastBig,
    MissileBlastSmall,
    MissileLaunch,
    MissionComplete,
    MissionFailed,
    PlayerRun,
    PlayerWalk,
    Select,
    ShowInstruction,
    SpacecraftMoving,
}
