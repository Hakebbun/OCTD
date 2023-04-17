using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicBaddie : MonoBehaviour, IHittable, IBaddie, IBaddieBuffable
{
    public GameObject corpsePrefab;
    public Animator animator;

    public float health = 5;
    public float moveSpeed = 5;

    public float recoilTime = 0.5f;

    public float damage = 5;
    private Rigidbody2D rb2d;
    private BaddieUtils utils;
    private Vector2 direction;
    private bool recoiling = false;
    private int timesBuffed = 0;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>(); 
        utils = GetComponent<BaddieUtils>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb2d.MovePosition(rb2d.position + direction * moveSpeed * Time.fixedDeltaTime);

        if (recoiling) {
            recoilTime -= Time.fixedDeltaTime;
            if (recoilTime <= 0 ) {
                recoiling = false;
                recoilTime = 0.5f;
                direction = Vector2.down;
                animator.SetBool("isRecoiling", false);

            }
        }
    }

    public void onDamage(float damage) {
        health -= damage;
        if (health <= 0) {
            utils.EmitBaddieKilledEvent();
            Instantiate(corpsePrefab, transform.position, transform.localRotation);
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) { 
        IHittable hittable = collision.gameObject.GetComponent<IHittable>();
  
        if (hittable != null)
        {
            hittable.onDamage(damage);
            recoiling = true;
            direction = Vector2.up;
            animator.SetBool("isRecoiling", recoiling);
        }
    }

    public int GetCost() {
        return 1;
    }

    public GameObject GetGameObject() {
        return gameObject;
    }

    public void init() {
        direction = Vector2.down;
    }

    public void BuffBaddie() {
        if (timesBuffed >= 2) {
            health += 1;
        } else if ( timesBuffed == 1 ) {
            damage  = damage * 1.5f;
        } else if (timesBuffed == 0) {
            moveSpeed = moveSpeed * 3f; 
            recoilTime = recoilTime / 3f;
        } 

        timesBuffed += 1;
    }

}
