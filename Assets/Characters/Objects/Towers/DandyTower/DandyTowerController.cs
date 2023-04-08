using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DandyTowerController : MonoBehaviour, ITower, IUpgradeable, ILoadable
{

    public GameObject targetPrefab;
    public TMPro.TextMeshProUGUI tmp;
    public GameObject crossHairPrefab;

    private Rigidbody2D rb2d;
    private MoneySpender moneySpender;
    private Vector2 direction;
    private GameObject crossHair;

    public int ammo = 5;
    public float aimSpeed = 40;
    public int maxAmmo = 10;
    public int damage = 3;


    // Start is called before the first frame update
    void Awake()
    {
        LevelController.OnPhaseChange += OnPhaseChange;    
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (crossHair != null) {
            Rigidbody2D crossHairRb2d = crossHair.GetComponent<Rigidbody2D>();

            if (crossHairRb2d != null) {
                crossHairRb2d.MovePosition(crossHairRb2d.position + direction * aimSpeed * Time.fixedDeltaTime);
            }
        }
    }

    public void OnFire() {
        if (ammo >= 2) {
            GameObject newTarget = Instantiate(targetPrefab, crossHair.transform.position, crossHair.transform.localRotation);
            DandyTargetController toArm = newTarget.GetComponent<DandyTargetController>();
            toArm.arm(damage);
            ammo -= 2;
        }
    }

    public void OnMove(Vector2 aimInput) {
        direction = aimInput;
    }

    public bool Load(IAmmo ammoToLoad) {
        // TODO: verify loadable ammo
        if (ammo < maxAmmo && ammoToLoad.GetType().Equals(typeof(SeedAmmo))) {
            ammo ++;
            tmp.text = ammo.ToString();
            return true;
        } else {
            return false;
        }
    }

    private void SpawnCrosshair() {
        Vector3 positionToSpawn = transform.position;
        positionToSpawn.y += 10;
        crossHair = Instantiate(crossHairPrefab, positionToSpawn, transform.localRotation) as GameObject;
        crossHair.transform.parent = transform;
    }

    void OnDestroy() {
        LevelController.OnPhaseChange -= OnPhaseChange;
    }

    public bool OnUpgrade(IUpgrade upgrade) {
        // TODO: verify loadable upgrades
        return false;
    }

    private void OnPhaseChange(Phase phase) {
        switch(phase) {
            case Phase.DAY:
                ammo = 0;
                gameObject.layer = LayerMask.NameToLayer("Moveable");  
                Destroy(crossHair);
                break;
            case Phase.NIGHT:
                SpawnCrosshair();
                gameObject.layer = LayerMask.NameToLayer("TowerLayer");
                break;
        }
    }
}
