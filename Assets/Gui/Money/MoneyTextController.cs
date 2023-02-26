using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyTextController : MonoBehaviour
{
    public EconomyController economyController;
    private TMPro.TextMeshProUGUI tmp;

    // Start is called before the first frame update
    void Start()
    {
        EconomyController.OnMoneyChange += UpdateMoneyText;
        tmp = GetComponent<TMPro.TextMeshProUGUI>();
        UpdateMoneyText(0);
    }

    void OnDestroy() {
        EconomyController.OnMoneyChange -= UpdateMoneyText;
    }

    private void UpdateMoneyText(int money) {
        tmp.text = string.Format("Money: {0}", money);
    }
}
