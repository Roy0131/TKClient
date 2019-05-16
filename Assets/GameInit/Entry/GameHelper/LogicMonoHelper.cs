
using UnityEngine;

public static class LogicMonoHelper
{
    public static AudioSource CreateSoundAudio()
    {
        GameObject soundObject = new GameObject();
        AudioSource result = soundObject.AddComponent<AudioSource>();
        return result;
    }
}
