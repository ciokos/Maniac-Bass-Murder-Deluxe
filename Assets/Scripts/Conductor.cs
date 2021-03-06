﻿using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.Events;
using UnityEngine;

[System.Serializable]
public class IntervalEvent : UnityEvent<float>
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
    public UnityEvent newLoopEvent;
    public IntervalEvent Interval; // Event invoked every interval
    public UnityEvent Beat;
    public float beatUnit = 0.25f;

    private float prevInterval = 0;
    private int prevBeat = 0;





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
        loopPositionInBeats = (songPositionInBeats - completedLoops * beatsPerLoop);

        if (loopPositionInBeats - prevInterval >= beatUnit && loopPositionInBeats - prevInterval <= 2*beatUnit)
        {
            prevInterval += beatUnit;
            if (prevInterval >= beatsPerLoop - beatUnit)
                prevInterval = 0;
            Interval.Invoke(prevInterval);
        }

        if ((int)loopPositionInBeats != prevBeat)
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

    public float getBeatValue()
    {
        return loopPositionInBeats;
    }

    public float getBeatsCount()
    {
        return beatsPerLoop;
    }
}
