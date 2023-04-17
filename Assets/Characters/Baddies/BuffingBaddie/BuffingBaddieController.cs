using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffingBaddieController : MonoBehaviour, IHittable, IBaddie
{
    public GameObject buffProjectilePrefab;
    public GameObject corpsePrefab;

    public float health = 5;
    public float moveSpeed = 0;
    public float attackSpeed = 4;
    public float timeSinceLastAttack;

    private Rigidbody2D rb2d;
    private BaddieUtils utils;

    // Start is called before the first frame update
    void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>(); 
        utils = GetComponent<BaddieUtils>();
        timeSinceLastAttack = attackSpeed;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        timeSinceLastAttack -= Time.fixedDeltaTime;

        if (timeSinceLastAttack <= 0) {
            doBuff();
            timeSinceLastAttack = attackSpeed;
        }
    }

    private void doBuff() {
        Vector3 positionToSpawn = transform.position;
        positionToSpawn.y -= 3;
        Instantiate(buffProjectilePrefab, positionToSpawn, transform.localRotation);
    }

    public void onDamage(float damage) {
        health -= damage;
        if (health <= 0) {
            utils.EmitBaddieKilledEvent();
            Instantiate(corpsePrefab, transform.position, transform.localRotation);
            Destroy(gameObject);
        }
    }

    public void init() {
        return;
    }

    public GameObject GetGameObject() {
        return gameObject;
    }

    public int GetCost() {
        return 5;
    }


}
