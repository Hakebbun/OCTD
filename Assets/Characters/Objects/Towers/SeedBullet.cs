using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedBullet : MonoBehaviour, IBullet
{
    public float speed;
    public float damage;
    private Vector2 direction;
    private bool isAoeUpgraded = false;
    private int numPierces = 1;

    private Rigidbody2D rb2d;

    // Start is called before the first frame update
    void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>(); 
    }

    public void SetUpgrades(bool aoeUpgrade, int damageUpgrades) {
        isAoeUpgraded = aoeUpgrade;
        damage += damageUpgrades;
    }

    public void Fire(Vector2 initialDirection) {
        direction = initialDirection;
        rb2d.AddForce(initialDirection * speed);
    }


    private void OnTriggerEnter2D(Collider2D collider) {
        IHittable hittable = collider.gameObject.GetComponent<IHittable>();

        if (hittable != null)
        {
            hittable.onDamage(damage);
            if (isAoeUpgraded && numPierces > 0) {
                numPierces -= 1;
                return;
            } else {
                Destroy(gameObject);
            }
        }
    }
}
