using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IModifier 
{
    void Modify(List<BulletParameters> bulletsParameters);
    void EnemyHit(bool isEmpowered);
}
