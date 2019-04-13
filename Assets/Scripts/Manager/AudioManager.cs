using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ClipName
{
    BoarAttack,                           // Audio clip when boar attacks
    BoarDeath,                            // Audio clip when boar dies
    BoarInjured,                          // Audio clip when boar gets injured
    BodyHit,                              // Audio clip when hit 
    BulletImpactDirt,                     // Audio clip when bullet hits dirt
    BulletImpactFlesh,                    // Audio clip when bullet hits flesh
    BulletImpactMetal,                    // Audio clip when bullet hits metal
    BulletImpactStone,                    // Audio clip when bullet hits stone
    BulletImpactWood,                     // Audio clip when bullet hits wood
    PlayerBreathingHeavy,                 // Audio clip when player breaths heavy
    PlayerDeath,                          // Audio clip when player dies
    PlayerHurt,                           // Audio clip when player gets hurt
    ZombieAttack,                         // Audio clip when zombie attacks
    ZombieDeath,                          // Audio clip when zombie dies
    ZombieInjured,                        // Audio clip when zombie gets injured
    ZombieScream                          // Audio clip when zombie screams
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    private AudioClip[] audioClip;
    private Dictionary<string, AudioClip> audioClipDic;

    private void Awake()
    {
        Instance = this;
        audioClip = Resources.LoadAll<AudioClip>("Audios/All/");
        audioClipDic = new Dictionary<string, AudioClip>();
        
        for(int i = 0; i < audioClip.Length; i++)
        {
            audioClipDic.Add(audioClip[i].name, audioClip[i]);
        }
    }

    public AudioClip GetAudioClipByName(ClipName name)
    {
        AudioClip tempClip = null;
        audioClipDic.TryGetValue(name.ToString(), out tempClip);
        return tempClip;
    }

    public void PlayAudioClipByName(ClipName name, Vector3 position)
    {
        AudioSource.PlayClipAtPoint(GetAudioClipByName(name), position);
    }

    public AudioSource AddAudioSourceComponent(GameObject obj, ClipName clipName, bool playOnAwake = true, bool loop = true)
    {
        AudioSource tempAudio = obj.AddComponent<AudioSource>();
        tempAudio.clip = GetAudioClipByName(clipName);
        if (playOnAwake) tempAudio.Play();
        tempAudio.loop = loop;
        return tempAudio;
    }
}
