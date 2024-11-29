using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("Timer Settings")]
    public float countdownTime = 60f; // Countdown duration in seconds
    public TextMeshProUGUI timerText; // TextMeshPro UI element to display the timer
    public List<Target> targets = new List<Target>();
    
    
    [Header("Scenes")]
    public string loseSceneName = "LoseScene"; // Name of the scene to load on loss
    public string winSceneName = "WinScene";   // Name of the scene to load on win

    private float currentCountdownTime;

    void Start()
    {
        // Initialize the countdown time
        currentCountdownTime = countdownTime;
        targets.AddRange(Object.FindObjectsByType<Target>(FindObjectsSortMode.None));
        
    }

    void Update()
    {
        // Update the countdown timer
        if (currentCountdownTime > 0)
        {
            currentCountdownTime -= Time.deltaTime;
            UpdateTimerDisplay();
        }
        else
        {
            // Timer reached zero, trigger loss condition
            LoadLoseScreen();
        }

        if (targets.Count == 0)
        {
            LoadWinScreen();
        }
    }

    private void UpdateTimerDisplay()
    {
        // Update the timer UI with TextMeshPro
        int minutes = Mathf.FloorToInt(currentCountdownTime / 60);
        int seconds = Mathf.FloorToInt(currentCountdownTime % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void LoadWinScreen()
    {
        // Load the win scene
        SceneManager.LoadScene(winSceneName);
    }

    public void LoadLoseScreen()
    {
        // Load the lose scene
        SceneManager.LoadScene(loseSceneName);
    }
    
    public void RemoveTargetFromList(Target target)
    {
        // Check if the target is in the list and remove it
        if (targets.Contains(target))
        {
            targets.Remove(target);
            Debug.Log("Target removed from the list.");
        }
    }
}