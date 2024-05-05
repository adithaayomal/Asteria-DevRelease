using UnityEngine;
using Photon.Pun;

public class CollectableItem : MonoBehaviourPun
{
    [SerializeField] string itemName; // Name or identifier of the item

    // Method called when the item is collected by a player
    public void CollectItem(PlayerController player)
    {
        if (!photonView.IsMine)
            return;

        // Notify all clients to destroy the collected item
        PhotonNetwork.Destroy(gameObject);

        // Add the item to the player's inventory
        player.AddItemToInventory(itemName);
    }
}
