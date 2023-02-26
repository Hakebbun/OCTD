using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class UseTowerAction : MonoBehaviour
{
    public LayerMask towerMask;
    private PlayerController controller;
    public GameObject towerToUse = null;

    void Start() {
        controller = GetComponent<PlayerController>();
        LevelController.OnPhaseChange += OnPhaseChange;
    }

    void OnDestroy() {
        LevelController.OnPhaseChange -= OnPhaseChange;
    }
     

    public void useTower() {
        if (towerToUse) {
            dropTower();
        } else {
            connectToTower();
        }
    }

    public void controlTower(Vector2 moveInput) {
        if (towerToUse) {
            ITower towerImpl = towerToUse.GetComponent<ITower>();
            if (towerImpl != null) {
                towerImpl.OnMove(moveInput);
            }
        }
    }

    public void fireTower() {
        if (towerToUse) {
            towerToUse.GetComponent<ITower>().OnFire();
        }
    }

    public bool loadTower(IAmmo ammoToLoad) {
        // make sure we're not hooked into the tower
        Debug.Log("load tower");
        if (towerToUse == null) {
            Debug.Log("tower to use null");
            Collider2D toLoadObj = Physics2D.OverlapCircle(transform.position, .4f, LayerMask.GetMask("TowerLayer", "Processor"));
            if (toLoadObj) {
                Debug.Log("found something in tower mask ");
                ILoadable toLoad = toLoadObj.gameObject.GetComponent<ILoadable>();
                return toLoad.Load(ammoToLoad);
            } 
        }
        return false;
    }

    private void connectToTower() {
        Collider2D toUse = Physics2D.OverlapCircle(transform.position, .4f, towerMask);
        if (toUse) {
            towerToUse = toUse.gameObject;
            controller.moveSpeed = 0;
        }
    }

    private void dropTower() {
        controller.moveSpeed = PlayerController.BASE_MOVE_SPEED;
        towerToUse = null;
    }

    private void OnPhaseChange(Phase phase) {
        switch(phase) {
            case Phase.DAY:
                dropTower();
                break;
            case Phase.NIGHT:
                break;
        }
    }
}
