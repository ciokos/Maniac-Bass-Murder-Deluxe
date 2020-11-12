using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

[System.Serializable]
public class BeatEvent : UnityEvent<float>
{
}

public class Conductor : MonoBehaviour
{

    public float songBpm;
    public float secPerBeat;
    public float songPosition;
    public float songPositionInBeats;
    public float dspSongTime;
    public AudioSource audioSource;
    public float beatsPerLoop;
    public int completedLoops = 0;
    public float loopPositionInBeats;
    public float loopPositionInAnalog;
    private int prevBeat = 0;

    public UnityEvent newLoopEvent;
    public BeatEvent Beat; // Event invoked every beat



    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        secPerBeat = 60f / songBpm;

        dspSongTime = (float)AudioSettings.dspTime;

        audioSource.Play();

        if (newLoopEvent == null)
            newLoopEvent = new UnityEvent();

        if (Beat == null)
            Beat = new BeatEvent();

    }

    // Update is called once per frame
    void Update()
    {
        songPosition = (float)(AudioSettings.dspTime - dspSongTime);

        songPositionInBeats = songPosition / secPerBeat;
        if (songPositionInBeats >= (completedLoops + 1) * beatsPerLoop)
        {
            completedLoops++;
            newLoopEvent.Invoke();
        }
        loopPositionInBeats = songPositionInBeats - completedLoops * beatsPerLoop;

        if ((int)loopPositionInBeats != prevBeat)
        {
            prevBeat = (int)loopPositionInBeats;
            Beat.Invoke(loopPositionInBeats);
        }

        loopPositionInAnalog = loopPositionInBeats / beatsPerLoop;
    }

    public bool isBeatEven()
    {
        return (int)loopPositionInBeats % 2 != 0;
    }

    public float getBeatValue()
    {
        return loopPositionInBeats;
    }

    public float getBeatsCount()
    {
        return beatsPerLoop;
    }
}
