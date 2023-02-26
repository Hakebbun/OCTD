using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarController : MonoBehaviour
{
    private Slider healthBar;
    public BaseController baseController;

    private void Start()
    {
        GameObject baseControllerObject = GameObject.FindGameObjectWithTag("Base");
        baseController = baseControllerObject.GetComponent<BaseController>();
        healthBar = GetComponent<Slider>();
        healthBar.maxValue = baseController.maxHealth;
        healthBar.value = baseController.maxHealth;
    }

    public void SetHealth(float hp)
    {
        healthBar.value = hp;
    }
}
