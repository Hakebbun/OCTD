using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EconomyController : MonoBehaviour
{
    private static GameObject economyControllerInstance;
    public static event Action<int> OnMoneyChange;

    public static int money;

    void Start()
    {
        DontDestroyOnLoad (gameObject);
         
        if (economyControllerInstance == null) {
            economyControllerInstance = gameObject;
        } else {
            DestroyObject(gameObject);
        }

        BasicBaddieCorpse.OnCorpseDespawn += OnCorpseDespawn;
        MoneySpender.OnSpendMoney += ChangeMoney;
    }

    void OnDestroy() {
        BasicBaddieCorpse.OnCorpseDespawn -= OnCorpseDespawn;
    }

    private void OnCorpseDespawn(int value) {
        ChangeMoney(value +1);
    }

    public void ChangeMoney(int moneyDelta) {
        money += moneyDelta;
        OnMoneyChange?.Invoke(money);
    }
}
