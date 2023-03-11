using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedProcessor : MonoBehaviour, ILoadable
{
    public GameObject spawnPrefab;
    public float maxProcessingTime = 2f;
    public float timeLeft = 2f;
    public int materialsLeft = 0;
    public int maxMaterials = 5;
    public TMPro.TextMeshProUGUI tmp;
    public Animator animator;



    // Start is called before the first frame update
    void Start()
    {
        LevelController.OnPhaseChange += OnPhaseChange;
        tmp.text = materialsLeft.ToString();
    }

    void OnDestroy() {
        LevelController.OnPhaseChange -= OnPhaseChange;
    }

    // Update is called once per frame
    void Update()
    {
        checkSpawn();
    }

    private void checkSpawn() {
        if (materialsLeft <=0 || LevelController.isDay) {return;}

        if (timeLeft <= 0) {
            materialsLeft --;

            if (materialsLeft <= 0) {
                animator.SetBool("isProcessing", false);
            }

            tmp.text = materialsLeft.ToString();
            GameObject newGameObject = Instantiate(spawnPrefab, transform.position, transform.localRotation) as GameObject;
            Processable processable = newGameObject.GetComponent<Processable>();
            if (processable != null){
                processable.BounceAway();
            }
            timeLeft = maxProcessingTime;
        } else {
            timeLeft -= Time.deltaTime;
        }
    }

    public bool Load(IAmmo ammoToLoad)
    {
        // verify loadable
        if (materialsLeft < maxMaterials && ammoToLoad.GetType().Equals(typeof(Seed))) {
            materialsLeft ++;
            animator.SetBool("isProcessing", true);
            tmp.text = materialsLeft.ToString();
            return true;
        } else {
            return false;
        }
    }

        private void OnPhaseChange(Phase phase) {
        switch(phase) {
            case Phase.DAY:
                materialsLeft = 0;
                gameObject.layer = LayerMask.NameToLayer("Moveable");  
                break;
            case Phase.NIGHT:
                gameObject.layer = LayerMask.NameToLayer("Processor");
                break;
        }
    }
}
