using UnityEngine;
using TMPro;

public class CountdownTimer : MonoBehaviour
{
    public TMP_Text minutesText;
    public TMP_Text secondsText;
    public float totalTime = 240f; // Total countdown time in seconds
    private float currentTime;

    private void Start()
    {
        currentTime = totalTime;
    }

    private void Update()
    {
        // Decrease the current time by deltaTime
        currentTime -= Time.deltaTime;

        // Ensure the timer doesn't go below zero
        currentTime = Mathf.Max(currentTime, 0f);

        // Update the UI text to display the remaining time
        UpdateTimerUI();
    }

    private void UpdateTimerUI()
    {
        // Calculate minutes and seconds from currentTime
        int minutes = Mathf.FloorToInt(currentTime / 60f);
        int seconds = Mathf.FloorToInt(currentTime % 60f);

        // Update the UI Text for minutes
        minutesText.text = minutes.ToString();

        // Update the UI Text for seconds
        secondsText.text = seconds.ToString("00");
    }

    // Public method to start the timer
    public void StartTimer()
    {
        // Start the timer from the beginning
        currentTime = totalTime;
    }
}
