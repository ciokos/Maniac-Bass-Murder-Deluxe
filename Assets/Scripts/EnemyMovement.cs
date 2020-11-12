using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{

    public float moveSpeed = 0.1f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        Vector2 movement = new Vector2(0.5f, 0.5f);
        movement *= moveSpeed;
        movement *= Time.fixedDeltaTime;
        this.transform.position += new Vector3(movement.x, movement.y, 0);
    }
}
