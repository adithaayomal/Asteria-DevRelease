using UnityEngine;
using UnityEngine.UI;

public class OpenWindow : MonoBehaviour
{
    public GameObject windowPanel;

    void Start()
    {
        // Ensure the window panel is initially closed
        if (windowPanel != null)
        {
            windowPanel.SetActive(false);
        }
        else
        {
            Debug.LogError("Window panel reference is not assigned!");
        }
    }

    public void OpenWindowPanel()
    {
        if (windowPanel != null)
        {
            windowPanel.SetActive(true);
        }
        else
        {
            Debug.LogError("Window panel reference is not assigned!");
        }
    }
}
