using UnityEngine;

public class DelayModifierObject : MonoBehaviour
{
    private DelayModifier modifier;

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.name == "Player")
        {
            modifier = gameObject.AddComponent<DelayModifier>();
            col.gameObject.GetComponent<Shooting>().AddModifier(modifier);
            Destroy(gameObject);
        }
    }
}

    