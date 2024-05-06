using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using TMPro;

public class ChatManager : MonoBehaviourPunCallbacks, IPunObservable
{
    public TMP_InputField messageInputField;
    public TMP_Text chatLogText;

    private const byte ChatMessageEventCode = 1;

    private void Start()
    {
        // Ensure the player is connected to the Photon network
        if (!PhotonNetwork.IsConnected)
        {
            Debug.LogError("ChatManager: Not connected to Photon network.");
            return;
        }

        // Subscribe to Photon events for receiving chat messages
        PhotonNetwork.AddCallbackTarget(this);
    }

    private void OnDestroy()
    {
        // Unsubscribe from Photon events
        PhotonNetwork.RemoveCallbackTarget(this);
    }

    // Method to send a chat message
    public void SendChatMessage()
    {
        string message = messageInputField.text.Trim();

        // Don't send empty messages
        if (string.IsNullOrEmpty(message))
            return;

        // Send the message via Photon network
        photonView.RPC("ReceiveChatMessage", RpcTarget.All, message);

        // Clear the input field after sending the message
        messageInputField.text = "";
    }

    // RPC method to receive and display chat messages
    [PunRPC]
    private void ReceiveChatMessage(string message, PhotonMessageInfo info)
    {
        string senderName = info.Sender.NickName;
        AddMessageToChatLog(senderName + ": " + message);
    }

    // Method to add a message to the chat log UI
    private void AddMessageToChatLog(string message)
    {
        chatLogText.text += "\n" + message;

        // Scroll the chat log to show the latest message
        Canvas.ForceUpdateCanvases();
        chatLogText.GetComponentInParent<ScrollRect>().verticalNormalizedPosition = 0f;
    }

    // Method to handle Photon events (unused in this script but required by interface)
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        // Unused in this script
    }
}
