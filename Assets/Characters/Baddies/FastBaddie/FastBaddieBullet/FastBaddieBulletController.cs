using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FastBaddieBulletController : MonoBehaviour
{
    public float speed;
    public float damage;

    private Rigidbody2D rb2d;
    private Vector2 direction = Vector2.down;

    // Start is called before the first frame update
    void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        rb2d.AddForce(direction * speed);
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        IHittable hittable = collision.gameObject.GetComponent<IHittable>();

        if (hittable != null)
        {
            hittable.onDamage(damage);
            Destroy(gameObject);
        }
    }
}
