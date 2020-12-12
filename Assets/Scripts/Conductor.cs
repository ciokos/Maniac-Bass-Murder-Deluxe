using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.Events;
using UnityEngine;

[System.Serializable]
public class IntervalEvent : UnityEvent<decimal>
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
    private decimal prevInterval = 0;
    private int prevBeat = 0;

    public UnityEvent newLoopEvent;
    public IntervalEvent Interval; // Event invoked every interval
    public UnityEvent Beat;



    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        secPerBeat = 60 / songBpm;

        dspSongTime = (float)AudioSettings.dspTime;

        audioSource.Play();

        if (newLoopEvent == null)
            newLoopEvent = new UnityEvent();

        if (Interval == null)
            Interval = new IntervalEvent();

        if (Beat == null)
            Beat = new UnityEvent();

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

        if (roundedPosition != prevInterval && roundedPosition % (decimal)0.25 == 0 )
        {
            prevInterval = roundedPosition;
            Interval.Invoke(loopPositionInBeats);
        }

        if((int)loopPositionInBeats != prevBeat)
        {
            prevBeat = (int)loopPositionInBeats;
            Beat.Invoke();
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
