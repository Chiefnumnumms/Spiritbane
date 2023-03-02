using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public int healthLevel = 10;
    public int maxHealth;
    public int currentHealth;

    public bool isDead;

    public HealthBar healthBar;
    AnimationManager animationManager;
    public Transform player;

    public List<GameObject> killZones;

    public Transform respawnPosition;

    public CameraShake cameraShake;

    [SerializeField]
    private GameObject gameOverScreen;
    [SerializeField]
    private CursorDisable cursorDisable;

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
            animationManager.PlayTargetAnim("Death", true);
            Debug.Log("PLAYER IS DEAD");

            if(isDead)
            {
                gameOverScreen.SetActive(true);
                cursorDisable.enabled = false;
                Cursor.lockState = CursorLockMode.Confined;
                Time.timeScale = 0f;
            }
            // RESPAWN TO PROPER POINT
            //StartCoroutine(Respawn(3.5f));

            //cameraShake.ScreenShake(transform.right);
        }
    }

    public void HandleRespawn()
    {
        if(isDead)
        {
            gameOverScreen.SetActive(false);
            cursorDisable.enabled = true;
            Cursor.lockState = CursorLockMode.Locked;
            Time.timeScale = 1f;
        }
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

    private void Update()
    {
        foreach (GameObject water in killZones)
        {
            if (player.position.y < water.transform.position.y)
            {
                Debug.Log("Under The Water");
                StartCoroutine(Respawn(0f));
            }
                
        }
    }
}
