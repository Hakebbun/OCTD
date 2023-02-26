using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneySpender : MonoBehaviour
{
    public static event Action<int> OnSpendMoney;

    public void SpendMoney(int amountToSpend) {
        OnSpendMoney(amountToSpend * -1);
    }
}