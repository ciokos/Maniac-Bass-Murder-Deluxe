using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TunerModiferObject : MonoBehaviour
{
    public Sprite tunerModifierSprite;
    private TunerModifier modifier;

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.name == "Player")
        {
            modifier = new TunerModifier();
            col.gameObject.GetComponent<Shooting>().AddModifier(modifier, tunerModifierSprite);
            Destroy(gameObject);
        }
    }
}

    