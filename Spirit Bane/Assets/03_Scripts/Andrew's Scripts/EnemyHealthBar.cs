using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    public Slider slider;
    float timeUntilBarIsHidden = 1.0f;

    public void SetCurrentHealth(int currentHealth)
    {
        slider.value = currentHealth;
    }

    public void SetMaxHealth(int maxHealth)
    {
        slider.maxValue = maxHealth;
        slider.value = maxHealth;
    }

    //private void Update()
    //{
    //    timeUntilBarIsHidden = timeUntilBarIsHidden * Time.deltaTime;

    //    if (timeUntilBarIsHidden <= 0)
    //    {
    //        timeUntilBarIsHidden = 0;
    //        slider.gameObject.SetActive(true);
    //    }
    //    else
    //    {
    //        if (!slider.gameObject.activeInHierarchy)
    //        {
    //            slider.gameObject.SetActive(true);
    //        }

    //        if (slider.value <= 0)
    //        {
    //            Destroy(slider.gameObject);
    //        }
    //    }
    //}
}
