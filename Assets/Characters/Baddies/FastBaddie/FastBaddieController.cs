using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FastBaddieController : MonoBehaviour, IHittable, IBaddie
{
    public float health = 3;
    public float moveSpeed;
    public float damage;
    public GameObject corpsePrefab;
    public GameObject bulletPrefab;
    public Animator animator;

    private int MAX_X = 40;
    private int MIN_X = -40;
    private float MOVING_TIME = 2f;

    private float timeInState;
    private Vector2 direction;

    private Rigidbody2D rb2d;
    private FastBaddieState baddieState;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        baddieState = FastBaddieState.MOVING;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        timeInState -= Time.fixedDeltaTime;

        if (baddieState == FastBaddieState.MOVING) {
            Move();
            if (timeInState <= 0) {
                baddieState = FastBaddieState.COOLING;
                Shoot();
                animator.SetBool("IsMoving", false);
                timeInState = MOVING_TIME;
            }
        } else if (baddieState == FastBaddieState.COOLING) {
            if (timeInState <= 0) {
                baddieState = FastBaddieState.MOVING;
                animator.SetBool("IsMoving", true);
                timeInState = MOVING_TIME;
            }
        }
    }

     public void onDamage(float damage) {
        health -= damage;
        if (health <= 0) {
            // Instantiate(corpsePrefab, transform.position, transform.localRotation);
            Destroy(gameObject);
        }
    }

    private void Move() {
        if (rb2d.position.x > MAX_X) {
            direction = Vector2.left;
        } else if (rb2d.position.x < MIN_X) {
            direction = Vector2.right;
        }

        rb2d.MovePosition(rb2d.position + direction * moveSpeed * Time.fixedDeltaTime);
    }

    private void Shoot() {
        Instantiate(bulletPrefab, transform.position, transform.localRotation);
    }

    public int GetCost() {
        return 2;
    }

    public GameObject GetGameObject() {
        return gameObject;
    }

    public void init() {
        direction = Vector2.left;
        timeInState = MOVING_TIME;
    }
}

public enum FastBaddieState {
    MOVING, COOLING
}
