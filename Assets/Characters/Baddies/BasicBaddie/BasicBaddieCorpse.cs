using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicBaddieCorpse : MonoBehaviour, ICorpse
{
    public float timeLeft;
    public TMPro.TextMeshProUGUI tmp;
    public static event Action<int> OnCorpseDespawn;
    public static event Action OnCorpseSpawn;
    public bool pauseCountDown;

    // Start is called before the first frame update
    void Awake()
    {
        OnCorpseSpawn?.Invoke();
        timeLeft = 5f;
    }

    void OnDestroy() {
        OnCorpseDespawn?.Invoke(Mathf.RoundToInt(Mathf.Ceil(timeLeft)));
    }

    // Update is called once per frame
    void Update()
    {
        if (pauseCountDown ||
            (transform.parent != null &&
            transform.parent.gameObject.layer == LayerMask.NameToLayer("Player"))
        ) {return;}


        if (timeLeft <=0) {
            Destroy(gameObject);
        }

        timeLeft -= Time.deltaTime;
        tmp.text = Mathf.Ceil(timeLeft).ToString();
    }

    void OnCollisionEnter2D(Collision2D collision) { 
        LayerMask layer = collision.collider.gameObject.layer;
        if (layer == LayerMask.NameToLayer("Player")) {
            pauseCountDown = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision) {
        LayerMask layer = collision.collider.gameObject.layer;
        if (layer == LayerMask.NameToLayer("Player")) {
            pauseCountDown = false;
        }
    }
}
