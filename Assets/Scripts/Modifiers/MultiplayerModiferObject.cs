using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MultiplayerModiferObject : MonoBehaviour
{
    public Sprite multiplierModifierSprite;
    public TMP_Text multiplayerText;
    private MultiplayerModifier modifier;

    private void Start()
    {
        multiplayerText = GameObject.FindGameObjectWithTag("MultiplayerText").GetComponent<TMP_Text>();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.name == "Player")
        {
            modifier = new MultiplayerModifier(multiplayerText);
            col.gameObject.GetComponent<Shooting>().AddModifier(modifier, multiplierModifierSprite);
            Destroy(gameObject);
        }
    }
}

    