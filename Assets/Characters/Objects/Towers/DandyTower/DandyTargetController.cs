using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DandyTargetController : MonoBehaviour
{
    public float countdown;
    private int damage;

    void Awake() {
        countdown = 1.5f;
    }

    void Update() {
        countdown -= Time.deltaTime;
        if (countdown <= 0) {
            Detonate();
            Destroy(gameObject);
        }
    }

    void Detonate() {
        CircleCollider2D collider = gameObject.GetComponent<CircleCollider2D>();
        if (collider != null) {
            Collider2D[] toDamage = 
                Physics2D.OverlapCircleAll(transform.position, collider.radius, LayerMask.GetMask("Baddies"));
            for (int i = 0; i < toDamage.Length; i++) {
                Collider2D colliderToHit = toDamage[i];
                IHittable hittable = colliderToHit.gameObject.GetComponent<IHittable>();
                if (hittable != null) {
                    hittable.onDamage(damage);
                }
            }
        }
    }

    public void arm(int detonationDamage) {
        damage = detonationDamage;
    }

}
