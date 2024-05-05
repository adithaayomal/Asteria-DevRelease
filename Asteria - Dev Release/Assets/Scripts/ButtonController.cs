using UnityEngine;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour
{
    // Reference to the Button component
    public Button button;

    // Method to disable the button when clicked
    public void DisableButtonOnClick()
    {
        button.interactable = false;
    }
}
