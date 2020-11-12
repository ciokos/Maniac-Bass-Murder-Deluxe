using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyMovement : MonoBehaviour
{

    public float moveSpeed = 0.1f;
    private Vector2 movement;
    private int angle = 90;
    private Transform target;
    private Conductor conductor;
    private int beatsPerLoop;
    private int directionChangeBeat;

    private void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        conductor = (Conductor)GameObject.FindObjectOfType<Conductor>();
        conductor.Beat.AddListener(onBeat);
        beatsPerLoop = (int)conductor.getBeatsCount();
        directionChangeBeat = getNextRandomBeat();
    }

    private void FixedUpdate()
    {
        Vector2 forward = this.transform.position - target.position;

        movement = Rotate(forward, angle);
        movement *= moveSpeed;
        movement *= Time.fixedDeltaTime;
        this.transform.position += new Vector3(movement.x, movement.y, 0);
    }
    private Vector2 Rotate(Vector2 v, float degrees)
    {
        float sin = Mathf.Sin(degrees * Mathf.Deg2Rad);
        float cos = Mathf.Cos(degrees * Mathf.Deg2Rad);

        float tx = v.x;
        float ty = v.y;
        v.x = (cos * tx) - (sin * ty);
        v.y = (sin * tx) + (cos * ty);
        return v;
    }

    private int getNextRandomBeat()
    {
        System.Random r = new System.Random();
        int rInt = r.Next(0, beatsPerLoop);
        return rInt;
    }

    private void onBeat(float beatValue)
    {
        if ((int)beatValue == directionChangeBeat)
        {
            angle = -angle;
            directionChangeBeat = getNextRandomBeat();
        }
    }
}