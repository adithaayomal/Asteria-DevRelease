using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using TMPro;



public class PlayerController : MonoBehaviourPunCallbacks, IDamageable
{

    private List<string> inventory = new List<string>();
	//object


    [SerializeField] float mouseSensitivity, sprintSpeed, walkSpeed, jumpForce, smoothTime;
    [SerializeField] GameObject cameraHolder;
    [SerializeField] Item[] items;
	[SerializeField] Image healthbarImage;
	[SerializeField] GameObject ui;
	[SerializeField] float interactRange = 5f;
	[SerializeField] Image healthpointImage;



    
    

    
    float verticalLookRotation;
    bool grounded;
    Vector3 smoothMoveVelocity;
    Vector3 moveAmount;
    Rigidbody rb;

    PhotonView PV; // Assuming you have a PhotonView component

    const float maxHealth = 100f;
	float currentHealth = maxHealth;

    PlayerManager playerManager;

    int itemIndex;
	int previousItemIndex = -1;
	

    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        PV = GetComponent<PhotonView>(); // Get PhotonView component
		
        playerManager = PhotonView.Find((int)PV.InstantiationData[0]).GetComponent<PlayerManager>();

        
    }

    void Start()
	{
        
		if(PV.IsMine)
		{
			EquipItem(0);
			
			
		}
		else
		{
			Destroy(GetComponentInChildren<Camera>().gameObject);
			Destroy(rb);
			Destroy(ui);
			
		}
		
		
	}


    void Update()
    {
        if (!PV.IsMine)
            return;

        Look();
        Move();
        Jump();

        for(int i = 0; i < items.Length; i++)
		{
			if(Input.GetKeyDown((i + 1).ToString()))
			{
				EquipItem(i);
				break;
			}
		}

		if(Input.GetAxisRaw("Mouse ScrollWheel") > 0f)
		{
			if(itemIndex >= items.Length - 1)
			{
				EquipItem(0);
			}
			else
			{
				EquipItem(itemIndex + 1);
			}
		}
		else if(Input.GetAxisRaw("Mouse ScrollWheel") < 0f)
		{
			if(itemIndex <= 0)
			{
				EquipItem(items.Length - 1);
			}
			else
			{
				EquipItem(itemIndex - 1);
			}
		}

        if(Input.GetMouseButtonDown(0))
		{
			items[itemIndex].Use();
		}
		if(transform.position.y < -10f) // Die if you fall out of the world
		{
			Die();
		}
		if (Input.GetKeyDown(KeyCode.F))
        {
            TryInteract();
        }
    }

    void EquipItem(int _index)
	{

        if(_index == previousItemIndex)
			return;
		
		itemIndex = _index;

		items[itemIndex].itemGameObject.SetActive(true);

		if(previousItemIndex != -1)
		{
			items[previousItemIndex].itemGameObject.SetActive(false);
		}

		previousItemIndex = itemIndex;

        if(PV.IsMine)
		{
			Hashtable hash = new Hashtable();
			hash.Add("itemIndex", itemIndex);
			PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
		}

		
	}

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
	{
		if(changedProps.ContainsKey("itemIndex") && !PV.IsMine && targetPlayer == PV.Owner)
		{
			EquipItem((int)changedProps["itemIndex"]);
		}

		if (targetPlayer != null && targetPlayer == photonView.Owner)
        {
            if (changedProps.ContainsKey("Inventory"))
            {
                // Update inventory based on received custom properties
                string[] receivedInventory = (string[])changedProps["Inventory"];
                inventory = new List<string>(receivedInventory);
            }
        }
        
	}

    void Look()
    {
        transform.Rotate(Vector3.up * Input.GetAxisRaw("Mouse X") * mouseSensitivity);

        verticalLookRotation += Input.GetAxisRaw("Mouse Y") * mouseSensitivity;
        verticalLookRotation = Mathf.Clamp(verticalLookRotation, -90f, 90f);

        cameraHolder.transform.localEulerAngles = Vector3.left * verticalLookRotation;
    }

    void Move()
    {
        Vector3 moveDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;

        moveAmount = Vector3.SmoothDamp(moveAmount, moveDir * (Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : walkSpeed), ref smoothMoveVelocity, smoothTime);

        rb.MovePosition(rb.position + transform.TransformDirection(moveAmount) * Time.deltaTime);
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && grounded)
        {
            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        }
    }

    public void SetGroundedState(bool _grounded)
    {
        grounded = _grounded;
    }

    void FixedUpdate()
	{
		if(!PV.IsMine)
			return;

		rb.MovePosition(rb.position + transform.TransformDirection(moveAmount) * Time.fixedDeltaTime);
	}

    public void TakeDamage(float damage)
	{
        
		PV.RPC(nameof(RPC_TakeDamage), PV.Owner, damage);
	}
    [PunRPC]
	void RPC_TakeDamage(float damage)
	{
		
        if(!PV.IsMine)
			return;
        currentHealth -= damage;

		healthbarImage.fillAmount = currentHealth / maxHealth;
		if(currentHealth <= 0)
		{
			Die();
			
		}
	}

    void Die()
	{
		playerManager.Die();
	}


	void TryInteract()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, interactRange))
        {
            CollectableItem collectable = hit.collider.GetComponent<CollectableItem>();
            if (collectable != null)
            {
                float distance = Vector3.Distance(transform.position, collectable.transform.position);
                if (distance <= interactRange)
                {
					healthpointImage.fillAmount += 0.1f;	
                    collectable.CollectItem(this);
                }
            }
        }
    }


	public void AddItemToInventory(string itemName)
    {
        if (!PV.IsMine)
            return;

        inventory.Add(itemName);

        // Optionally sync inventory over the network using Photon's custom properties
        Hashtable hash = new Hashtable();
        hash.Add("Inventory", inventory.ToArray());
        PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
    }
    
  
    
		
	
}
