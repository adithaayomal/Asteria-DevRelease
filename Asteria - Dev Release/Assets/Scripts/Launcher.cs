using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Photon.Realtime;
using System.Linq;
using UnityEngine.UI;

public class Launcher : MonoBehaviourPunCallbacks
{
    public TMP_Text Code;
    public Button createRoomButton;
	public Button joinRoomButton;
    public TMP_InputField roomCodeInput;
    private string currentRoomNumber;
	public TMP_InputField usernameInput;
	//private TMP_Text savedUsernameText;
	[SerializeField] Transform playerListContent;
	[SerializeField] GameObject PlayerListItemPrefab;
	public Button startGameButton;
	

    // Dictionary to map Photon Player IDs to their corresponding player slots
    private Dictionary<int, GameObject> playerSlotDictionary = new Dictionary<int, GameObject>();


	void Start()
	{
		Debug.Log("Connecting to Master");
        PhotonNetwork.ConnectUsingSettings();
		string savedUsername = PlayerPrefs.GetString("Username", "");
        usernameInput.text = savedUsername;
        //savedUsernameText.text = savedUsername;
		
	}

	public override void OnConnectedToMaster()
	{
		Debug.Log("Connected to Master");
		PhotonNetwork.JoinLobby();
		PhotonNetwork.AutomaticallySyncScene = true;
		
		

	}

	public override void OnJoinedLobby()
	{

		Debug.Log("Joined Lobby");
		
        
        
	}

    public void CreateRoom()
	{   
		Debug.Log("Joining Room");
			if (PhotonNetwork.InRoom)
			{
				PhotonNetwork.LeaveRoom();
				StartCoroutine(DelayedCreateRoom());
			}
			else
			{
				
			
				StartCoroutine(DelayedCreateRoom());

				
			}
			// Set the current room number
			//currentRoomNumber = roomNumber;
	}
	IEnumerator DelayedCreateRoom()
    {
        createRoomButton.interactable = false; // Disable the button
        Code.text = ""; // Set text to "------"
        
        yield return new WaitForSeconds(1f); // Wait for 5 seconds
      
        string roomNumber = GenerateRandomRoomNumber();
			
		PhotonNetwork.CreateRoom(roomNumber);
		Code.text = roomNumber;

		LogCreatedRoom(roomNumber);
        yield return new WaitForSeconds(1f);
        createRoomButton.interactable = true; // Enable the button
    }
	public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("Failed to create room: " + message);
    }


	public void JoinRoom()
	{
		if (PhotonNetwork.InRoom)
		{
			if (!string.IsNullOrEmpty(roomCodeInput.text))
			{	
				LeaveRoom();
				Debug.Log("Joining Room");
				StartCoroutine(DelayedJoinRoom());
				
			}
			else
			{
				Debug.Log("Please Enter Room Code");
			}
		}
		else
		{
			if (!string.IsNullOrEmpty(roomCodeInput.text))
			{	
				Debug.Log("Joining Room");
				StartCoroutine(DelayedJoinRoom());
				
			}
			else
			{
				Debug.Log("Please Enter Room Code");
			}
		}
			
		

		
	}
	IEnumerator DelayedJoinRoom()
    {
        createRoomButton.interactable = false; // Disable the button
        Code.text = ""; // Set text to "------"
        
        yield return new WaitForSeconds(1f); // Wait for 5 seconds

        string roomCode = roomCodeInput.text; // Get the room code from the input field
		PhotonNetwork.JoinRoom(roomCode); // Join the room with the specified code

        yield return new WaitForSeconds(1f);

        createRoomButton.interactable = true; // Enable the button
    }

	public override void OnJoinedRoom()
    {
        Debug.Log("Successfully Joined Room");

        Player[] players = PhotonNetwork.PlayerList;

        foreach (Transform child in playerListContent)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < players.Length; i++)
        {
            Instantiate(PlayerListItemPrefab, playerListContent).GetComponent<PlayerListItem>().SetUp(players[i]);
        }
        startGameButton.interactable = PhotonNetwork.IsMasterClient; // Enable/disable button based on whether the player is the master client
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        startGameButton.interactable = PhotonNetwork.IsMasterClient; // Enable/disable button based on whether the player is the master client
    }
	public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log("Failed to join room: " + message);
        // Call the method to show the panel for a brief moment
        
    }


	public void LeaveRoom()
	{
		Debug.Log("Leaving Room");
		PhotonNetwork.LeaveRoom();
	
	}
    

	public override void OnLeftRoom()
	{
		
	}
	
	public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        
        foreach (RoomInfo room in roomList)
        {
            Debug.Log("Available Rooms: " + room.Name);
        }
    }

	public void StartGame()
	{
		PhotonNetwork.LoadLevel(1);

		
	}

	private string GenerateRandomRoomNumber()
{
    // Generate a random 6-digit number
    System.Random random = new System.Random();
    int randomNumber = random.Next(100000, 999999); // Generates a number between 100000 and 999999
    return randomNumber.ToString();
}
    public void LogCreatedRoom(string roomCode)
    {
        Debug.Log("Created Room with Code: " + roomCode);
        
    }

	public void SaveUsername()
    {
        string username = usernameInput.text;
        
        // Check if the username is not empty
        if (!string.IsNullOrEmpty(username))
        {
            // Save the username to PlayerPrefs
            PlayerPrefs.SetString("Username", username);
            PlayerPrefs.Save(); // Save PlayerPrefs data immediately
			PhotonNetwork.NickName = username;
            Debug.Log("Username saved: " + username);

        }
        else
        {
            Debug.Log("Username cannot be empty");
            // Optionally, you can display a message to the player indicating that they need to enter a username
        }
    }


    public override void OnPlayerEnteredRoom(Player newPlayer)
	{
		Instantiate(PlayerListItemPrefab, playerListContent).GetComponent<PlayerListItem>().SetUp(newPlayer);
	}
     

	
}