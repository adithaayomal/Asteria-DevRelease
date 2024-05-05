﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Linq;
using System.IO;
using Hashtable = ExitGames.Client.Photon.Hashtable;
public class PlayerManager : MonoBehaviour
{
    PhotonView PV;
    // Start is called before the first frame update
	GameObject controller;

	

    void Awake()
	{
		PV = GetComponent<PhotonView>();
	}
    void Start()
    {
        {
		if(PV.IsMine)
		{
			CreateController();

			
		}

		
	}
    }

    void CreateController()
	{
		Transform spawnpoint = SpawnManager.Instance.GetSpawnpoint();
		controller = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerController"), spawnpoint.position, spawnpoint.rotation, 0, new object[] { PV.ViewID });
	}

	public void Die()
	{
		PhotonNetwork.Destroy(controller);
		CreateController();

	}


	
	

   
}
