using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CustomNetworkManager : NetworkManager{

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId, NetworkReader extraMessageReader) {
		Debug.Log ("HAAALLLPPPPPP");
		GameObject[] players = GameObject.FindGameObjectsWithTag ("MainPlayer");
		GameObject[] spawnPoints = GameObject.FindGameObjectsWithTag ("Respawn");
		Vector3 spawnPointPosition = spawnPoints [0].transform.position;
		Quaternion spawnPointRotation = spawnPoints [0].transform.rotation;
		Debug.Log (spawnPoints.Length.ToString());
		for (int i = 0; i < players.Length; i++) {
			if (players [i].GetComponent<Complete.TankHealth>().getSpawnPosition().Equals (spawnPoints [0].transform.position)) {
				spawnPointPosition = spawnPoints [1].transform.position;
				spawnPointRotation = spawnPoints [1].transform.rotation;
			} else {
			
			}
		}
		GameObject player = Instantiate (playerPrefab, spawnPointPosition,spawnPointRotation);
		player.GetComponent<Complete.TankHealth> ().setSpawnPosition(spawnPointPosition);
		player.GetComponent<Complete.TankHealth> ().setSpawnRotation(spawnPointRotation);
		NetworkServer.AddPlayerForConnection (conn, player, playerControllerId);
	}
}
