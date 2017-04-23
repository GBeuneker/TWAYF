using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AudioManager : Singleton<AudioManager>
{
    public delegate void OnAudioClipFinish();

    public enum Channel
    {
        SFX,
        Music
    }

    #region static

    public static bool mute = false;
    /// <summary> Toggles wether the sound is muted or not. </summary>
    public static bool ToggleSoundMute()
    {
        mute = !mute;
        foreach (AudioSourceWrapper s in Instance.sources.Values)
        {
            s.audioSource.mute = mute;
        }
        return mute;
    }

    /// <summary> Toggles wether the sound is muted or not. </summary>
    public static bool ToggleSoundMute(Channel channel)
    {
        AudioSourceWrapper source = Instance.sources[channel];
        source.audioSource.mute = !source.audioSource.mute;
        return source.audioSource.mute;
    }


    public static void StopAudio()
    {
        foreach (AudioSourceWrapper s in Instance.sources.Values)
        {
            s.audioSource.Stop();
        }
    }


    /// <summary>Play the specified clip, volume, layer and interruptOld.</summary>
    /// <param name="interruptOld">If set to <c>true</c> interrupt the now playing clip.</param>
    public static void Play(
        AudioClip clip,
        Channel layer,
        float volume = 1f,
        bool interruptOld = false,
        bool loop = false,
        OnAudioClipFinish finish = null,
        System.Object audioPlayingObjectContext = null
        )
    {
        AudioSourceWrapper source = Instance.sources[layer];
        source.audioSource.clip = clip;
        source.audioSource.volume = volume;
        source.audioSource.loop = loop;

        if (interruptOld)
        {
            source.audioSource.Stop();
            if (source.onFinish != null && source.onFinish.Target != null) source.onFinish();
            source.onFinish = finish;
            source.audioPlayingObjectContext = audioPlayingObjectContext;
        }
        if (!source.audioSource.isPlaying || interruptOld)
            source.audioSource.Play();
    }

    /// <summary>
    /// Finds if any channel is playing audio associated with the supplied object context.
    /// </summary>
    /// <param name="audioPlayingObjectContext">The audio playing object context supplied when a clip is being played with Play()</param>
    /// <returns></returns>
    public bool IsPlaying(System.Object audioPlayingObjectContext)
    {
        foreach (Channel l in System.Enum.GetValues(typeof(Channel)))
        {
            if (sources[l].audioPlayingObjectContext == audioPlayingObjectContext)
            {
                if (sources[l].audioSource.isPlaying && sources[l].onFinish != null && sources[l].onFinish.Target != null)
                    return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Stops any channels which are playing audio in context of the supplied object
    /// </summary>
    /// <param name="audioPlayingObjectContext"></param>
    public void Stop(System.Object audioPlayingObjectContext, bool fireEvent = true)
    {
        foreach (Channel l in System.Enum.GetValues(typeof(Channel)))
        {
            if (sources[l].audioPlayingObjectContext == audioPlayingObjectContext)
            {
                sources[l].audioSource.Stop();
                if (sources[l].onFinish != null && sources[l].onFinish.Target != null && fireEvent) sources[l].onFinish();
                sources[l].onFinish = null;
            }
        }
    }

    public void Stop(Channel channel)
    {
        sources[channel].audioSource.Stop();
    }

    #endregion

    private Dictionary<Channel, AudioSourceWrapper> sources = new Dictionary<Channel, AudioSourceWrapper>();

    public override void Awake()
    {
        base.Awake();

        // Create sources for the layers
        foreach (Channel l in System.Enum.GetValues(typeof(Channel)))
        {
            sources.Add(l, new AudioSourceWrapper(gameObject.AddComponent<AudioSource>()));
        }

        StartCoroutine(CheckIsAudioFinishedLoop());
    }

    /// <summary>
    /// Makes sure this AudioManager is instantiated 
    /// </summary>
    public static void EnsureInstance()
    {
        AudioManager am = Instance; // makes sure it gets created if it doesnt exist yet
    }

    /// <summary>
    /// Checks if there was an audio playing and if it is finished, and calls the OnFinish delegate.
    /// </summary>
    /// <returns></returns>
    private IEnumerator CheckIsAudioFinishedLoop()
    {
        while (true)
        {
            foreach (Channel l in System.Enum.GetValues(typeof(Channel)))
            {
                AudioSourceWrapper asw = sources[l];

                if (!asw.audioSource.isPlaying && asw.onFinish != null && asw.onFinish.Target != null)
                {
                    asw.onFinish();
                    asw.onFinish = null;
                }
            }

            yield return new WaitForSeconds(0.1f);
        }
    }

    private class AudioSourceWrapper
    {
        public AudioSource audioSource;
        public OnAudioClipFinish onFinish;
        public System.Object audioPlayingObjectContext;

        public AudioSourceWrapper(AudioSource audioSource)
        {
            this.audioSource = audioSource;
        }
    }
}

