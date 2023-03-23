using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Processable : MonoBehaviour
{

    public float speed;
    public float decelleration;
    private Rigidbody2D rb2d;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>(); 
    }

    // Update is called once per frame
    void Update()
    {
        if (speed <=0 ) {return;} 
        speed -= (decelleration * Time.deltaTime);
        
        rb2d.MovePosition(rb2d.position + Vector2.left * speed * Time.deltaTime);
    }

    public void BounceAway() {
        speed = 360;
    }
}
