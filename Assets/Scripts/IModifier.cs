using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IModifier 
{
    void Modify(List<GameObject> bullets, bool isEmpowered);
    void EnemyHit(bool isEmpowered);
}
