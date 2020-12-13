using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayModifier : MonoBehaviour, IModifier
{
    public void EnemyHit(bool isEmpowered)
    {
        return;
    }

    public void Modify(List<BulletParameters> bullets)
    {
        float delay = FindObjectOfType<Conductor>().secPerBeat/4;
        if (bullets[0].isEmpowered)
        {
            BulletParameters delayedBulletParameters = new BulletParameters(bullets[0])
            {
                delay = delay
            };
            bullets.Add(delayedBulletParameters);
        }
        return;
    }
}
