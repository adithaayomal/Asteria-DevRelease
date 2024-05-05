using UnityEngine;
using UnityEngine.UI;

public class CloseWindow : MonoBehaviour
{
    public GameObject windowPanel;

    public void CloseWindowPanel()
    {
        if (windowPanel != null)
        {
            windowPanel.SetActive(false);
        }
        else
        {
            Debug.LogError("Window panel reference is not assigned!");
        }
    }
}
