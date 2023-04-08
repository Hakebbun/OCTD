using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBuffController : MonoBehaviour
{

    public float speed;

    private Rigidbody2D rb2d;
    private Vector2 direction = Vector2.down;

    // Start is called before the first frame update
    void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        rb2d.AddForce(direction * speed);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        IBaddieBuffable buffable = other.gameObject.GetComponent<IBaddieBuffable>();
        if (buffable != null) {
            buffable.BuffSpeed();
        }

        // breaks on contact with hittables (base mostly, but maybe other things?)
        BaseController hittable = other.gameObject.GetComponent<BaseController>();
        if (hittable != null)
        {
            Destroy(gameObject);
        }
    }
}