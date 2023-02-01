using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    public int healthLevel = 10;
    public int maxHealth;
    public int currentHealth;

    public bool isDead;

    public EnemyHealthBar healthBar;
    AnimationManager animationManager;

    public CameraShake cameraShake;

    private void Awake()
    {
        animationManager = GetComponentInChildren<AnimationManager>();
        cameraShake = FindObjectOfType<CameraShake>();
    }

    private void Start()
    {
        maxHealth = SetMaxHealthFromHealthLevel();
        currentHealth = maxHealth;

        // SET CHARACTERS MAX HEALTH BASED ON THE HEALTH LEVEL
        //healthBar.SetMaxHealth(maxHealth);
    }

    private int SetMaxHealthFromHealthLevel()
    {
        maxHealth = healthLevel * 10;

        return maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth = currentHealth - damage;

        // PLAY HIT ANIMATION
        //animationManager.PlayTargetAnim("Pain Gesture", true);

        // HANDLE DEATH AND RESPAWN
        HandleDeath();

        // SHAKE THE CAMERA
        cameraShake.ScreenShake(transform.right);
    }

    public void HandleDeath()
    {
        // CHECK IF PLAYER IS BELOW 0 HEALTH (DEAD)
        if (currentHealth <= 0)
        {
            // MAKE SURE TO SET IT TO 0 EXACTLY
            currentHealth = 0;

            // PLAY DEATH ANIMATION
            isDead = true;
            //animationManager.PlayTargetAnim("Death", true);
            Debug.Log("PLAYER IS DEAD");

            // RESPAWN TO PROPER POINT
            //StartCoroutine(Respawn(3.5f));

            cameraShake.ScreenShake(transform.right);
        }
    }

    public void HandleRespawn()
    {
        isDead = false;
        HandleHealthCalculation();
    }

    public IEnumerator Respawn(float duration)
    {
        yield return new WaitForSeconds(duration);

        HandleRespawn();
    }

    public void HandleHealthCalculation()
    {
        maxHealth = SetMaxHealthFromHealthLevel();
        currentHealth = maxHealth;

        // SET CHARACTERS MAX HEALTH BASED ON THE HEALTH LEVEL
        healthBar.SetMaxHealth(maxHealth);
    }
}
