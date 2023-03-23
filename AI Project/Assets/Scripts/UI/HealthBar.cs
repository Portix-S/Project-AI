using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] Image image;
    public float currentHealth;
    public  float targetHealth;
    [SerializeField] float maxHealth;
    public event EventHandler OnHealthChanged;

    // Start is called before the first frame update
    void Start()
    {
        //image = GetComponent<Image>();
        targetHealth = currentHealth;
        maxHealth = currentHealth;
    }

    private void Update()
    {
        image.fillAmount = currentHealth / maxHealth;
        UpdateHealth();

    }

    public void TakeDamage(float amount) //
    {
        if (currentHealth - amount > 0)
        {
            targetHealth = currentHealth - amount;
        }
        else
        {
            targetHealth = 0f;
        }
    }

    public void UpdateHealth()
    {

        if (currentHealth > targetHealth)
        {
            currentHealth--;
        }   
        else
        {
            currentHealth = targetHealth;
            OnHealthChanged?.Invoke(this, EventArgs.Empty);

        }


    }

}
