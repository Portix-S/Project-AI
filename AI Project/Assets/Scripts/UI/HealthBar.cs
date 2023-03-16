using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    Image image;
    public float currentHealth;
    float targetHealth;

    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
    }

    public void TakeDamage(float amount) //
    {
        if(currentHealth - amount > 0)
            targetHealth = currentHealth - amount;
        else
            targetHealth = 0f;
        UpdateHealth();
    }

    private void UpdateHealth()
    {
        if(currentHealth > targetHealth)
        {
            currentHealth--;
            Invoke("UpdateHealth",0.1f);
        }   
        else
        {
            currentHealth = targetHealth;
            image.fillAmount = currentHealth;
        }   


    }

}
