using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SoundManager
{
    public enum Sound
    {
        Key,
        Key_PickUp,
        Tomato_PickUp,
        Leaf_PickUp,
        PlayerDie,
        PlayerDie2,
        PlayerDie3,
        PlayerDie4,
        PlayerDie5,
        PlayerColourSwitch,
        CheckpointSetter_Interaction,
        Canvas_Selection,
        Door_EventOpen,
        Door_EventCreak,
        PlayerInflate,
        PlayerDeflate,
        PlayerMove,
        PlayerJump,
        PlayerDash,
        PlayerFall,
        DeathSlice,
        Portal,
        PlayerJump2,
    }

    private static Dictionary<Sound, float> soundTimerDictionary;

    public static void Initialize()
    {
        soundTimerDictionary = new Dictionary<Sound, float>();
        soundTimerDictionary[Sound.PlayerMove] = 0f;
    }
    
    //3D sound
    public static void PlaySound(Sound sound, Vector3 position)
    {
        if (CanPlaySound(sound) == true)
        {
            GameObject soundGameObject = new GameObject("Sound");
            soundGameObject.transform.position = position;
            AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();
            audioSource.clip = GetAudioClip(sound);
            audioSource.maxDistance = 30f;
            audioSource.spatialBlend = 1f;
            audioSource.rolloffMode = AudioRolloffMode.Linear;
            audioSource.dopplerLevel = 0f;
            audioSource.volume = 0.25f;
            audioSource.Play();
        }
    }
    
    public static void PlaySound(Sound sound)
    {
        if (CanPlaySound(sound) == true)
        {
            GameObject soundGameObject = new GameObject("Sound");
            AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();
            audioSource.PlayOneShot(GetAudioClip(sound));
        }
    }

    private static bool CanPlaySound(Sound sound)
    {
        switch (sound)
        {
            default:
                return true;
            case Sound.PlayerMove:
                if (soundTimerDictionary.ContainsKey(sound))
                {
                    float lastTimePlayed = soundTimerDictionary[sound];
                    float playerMoveTimerMax = 1.275f;
                    if (lastTimePlayed + playerMoveTimerMax < Time.time)
                    {
                        //soundTimerDictionary[sound] = Time.captureDeltaTime;
                        soundTimerDictionary[sound] = Time.time;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return true;
                }
                //break;

        }
    }
    private static AudioClip GetAudioClip(Sound sound)
    {
        foreach (GameAssets.SoundAudioClip soundAudioClip in GameAssets.i.soundAudioClipArray)
        {
            if (soundAudioClip.sound == sound)
            {
                return soundAudioClip.audioClip;
            }
        }
        Debug.LogError("Sound " + sound + " Not found sorry");
        return null;
    }

}
