using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TunerModifier : MonoBehaviour, IModifier
{
    public void EnemyHit(bool isEmpowered)
    {
        return;
    }

    public void Modify(List<BulletParameters> bullets)
    {
        foreach (BulletParameters b in bullets)
            b.isTuned = true;
    }
}
