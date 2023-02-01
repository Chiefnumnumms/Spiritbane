using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public int healthLevel = 10;
    public int maxHealth;
    public int currentHealth;

    public bool isDead;

    public HealthBar healthBar;
    AnimationManager animationManager;

    public Transform respawnPosition;

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
        healthBar.SetMaxHealth(maxHealth);
    }

    private int SetMaxHealthFromHealthLevel()
    {
        maxHealth = healthLevel * 10;

        return maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth = currentHealth - damage;

        // UPDATE CURRENT HEALTH BASED ON DAMAGE TAKEN
        healthBar.SetCurrentHealth(currentHealth);

        // PLAY HIT ANIMATION
        animationManager.PlayTargetAnim("Pain Gesture", true);

<<<<<<< HEAD:Spirit Bane/Assets/03_Scripts/Andrew's Scripts/CharacterManager.cs
        // Screen Shake Slightly Each Time

            // HANDLE DEATH AND RESPAWN
        if (currentHealth <= 0)
        {
            HandleDeath();
            // Slow Mode Shake And Focus Camera
        }


=======
        // HANDLE DEATH AND RESPAWN
        HandleDeath();

        // SHAKE THE CAMERA
        cameraShake.ScreenShake(transform.right);
>>>>>>> ef099a4afe6d643ee0b77dfe9597795e0a388c4d:Spirit Bane/Assets/03_Scripts/Andrew's Scripts/PlayerStats.cs
    }

    public void HandleDeath()
    {
            // MAKE SURE TO SET IT TO 0 EXACTLY
            currentHealth = 0;

            // PLAY DEATH ANIMATION
            isDead = true;
            animationManager.PlayTargetAnim("Death", true);
            Debug.Log("PLAYER IS DEAD");

            // RESPAWN TO PROPER POINT
            StartCoroutine(Respawn(3.5f));
<<<<<<< HEAD:Spirit Bane/Assets/03_Scripts/Andrew's Scripts/CharacterManager.cs
        
=======

            cameraShake.ScreenShake(transform.right);
        }
>>>>>>> ef099a4afe6d643ee0b77dfe9597795e0a388c4d:Spirit Bane/Assets/03_Scripts/Andrew's Scripts/PlayerStats.cs
    }

    public void HandleRespawn()
    {
        transform.position = respawnPosition.position;
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
