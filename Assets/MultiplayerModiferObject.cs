using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MultiplayerModiferObject : MonoBehaviour
{
    public TMP_Text multiplayerText;
    private MultiplayerModifier modifier;

    private void Start()
    {
        
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.name == "Player")
        {
            modifier = new MultiplayerModifier(multiplayerText);
            col.gameObject.GetComponent<Shooting>().AddModifier(modifier);
            Destroy(gameObject);
        }
    }
}

    