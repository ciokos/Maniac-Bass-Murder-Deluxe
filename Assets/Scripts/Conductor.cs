using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.Events;
using UnityEngine;

[System.Serializable]
public class BeatEvent : UnityEvent<decimal>
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
    public decimal loopPositionInBeats;
    public float loopPositionInAnalog;
    private decimal prevBeat = 0;

    public UnityEvent newLoopEvent;
    public BeatEvent Beat; // Event invoked every beat



    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        secPerBeat = 60 / songBpm;

        dspSongTime = (float)AudioSettings.dspTime;

        audioSource.Play();

        if (newLoopEvent == null)
            newLoopEvent = new UnityEvent();

        if (Beat == null)
            Beat = new BeatEvent();

    }

    // Update is called once per fram
    void Update()
    {
        songPosition = (float)(AudioSettings.dspTime - dspSongTime);

        songPositionInBeats = songPosition / secPerBeat;
        if (songPositionInBeats >= (completedLoops + 1) * beatsPerLoop)
        {
            completedLoops++;
            newLoopEvent.Invoke();
        }
        loopPositionInBeats = (decimal)(songPositionInBeats - completedLoops * beatsPerLoop);

        decimal roundedPosition = Math.Round(loopPositionInBeats, 2);

        if (roundedPosition != prevBeat && roundedPosition % (decimal)0.25 == 0 )
        {
            prevBeat = roundedPosition;
            Beat.Invoke(loopPositionInBeats);
        }

        loopPositionInAnalog = (float)loopPositionInBeats / beatsPerLoop;
    }

    public bool isBeatEven()
    {
        return (int)loopPositionInBeats % 2 != 0;
    }

    public decimal getBeatValue()
    {
        return loopPositionInBeats;
    }

    public float getBeatsCount()
    {
        return beatsPerLoop;
    }
}
