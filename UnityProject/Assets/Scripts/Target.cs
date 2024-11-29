using System.Collections;
using UnityEngine;

public class Target : MonoBehaviour
{
    [Header("Target Settings")] // Reference to the ScriptableObject for settings
    public Transform pointA; // First point for movement
    public Transform pointB; // Second point for movement
    public GameManager _gameManager;
    public float moveSpeed = 2f; // Speed of movement
    public string damagingTag = "Bullet";
    public int maxHealth;
    public bool movingTarget;

    public Color damageColor = Color.black;
    public float damageEffectDuration = 0.2f;


    private Renderer objectRenderer;
    private Color originalColor;

    private int currentHealth; // Target's current health

    // Target's current health
    private bool movingToPointB = true; // Direction of movement
    private Rigidbody rb; // Rigidbody for movement

    void Start()
    {
        // Initialize health from the ScriptableObject
        currentHealth = maxHealth;

        // Assign the Rigidbody
        rb = GetComponent<Rigidbody>();

        objectRenderer = GetComponent<Renderer>();
        originalColor = objectRenderer.material.color;

        _gameManager = FindObjectOfType<GameManager>();
        
    }

    void Update()
    {
        // Handle movement if the target is a moving target
        if (movingTarget && rb != null)
        {
            MoveBetweenPoints();
        }
    }

    private void MoveBetweenPoints()
    {
        // Determine the target position
        Transform targetPoint = movingToPointB ? pointB : pointA;

        // Calculate the movement step
        Vector3 direction = (targetPoint.position - transform.position).normalized;
        Vector3 movement = direction * moveSpeed * Time.deltaTime;

        // Move the Rigidbody towards the target point
        rb.MovePosition(rb.position + movement);

        // Check if the target reached the destination
        if (Vector3.Distance(transform.position, targetPoint.position) < 0.1f)
        {
            movingToPointB = !movingToPointB; // Reverse the direction
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Check if the object colliding has the damaging tag
        if (collision.gameObject.CompareTag(damagingTag))
        {
            TakeDamage(1); // Take 1 damage
        }
    }

    private void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log($"Target took {damage} damage. Remaining health: {currentHealth}");
        StartCoroutine(DamageEffect());
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Target has died.");
        _gameManager.RemoveTargetFromList(this);
        // Add your die logic here (e.g., destroy the object, play an animation, etc.)
        Destroy(gameObject);
    }
    
    
    private IEnumerator DamageEffect()
    {
        if (objectRenderer != null)
        {
            // Calculate the tinted color (blend of original and damageColor)
            Color tintedColor = Color.Lerp(originalColor, damageColor, 0.5f); // Adjust 0.5f for the intensity of the tint
            objectRenderer.material.color = tintedColor;

            // Wait for the duration of the effect
            yield return new WaitForSeconds(damageEffectDuration);

            // Restore the original color
            objectRenderer.material.color = originalColor;
        }
    }
}