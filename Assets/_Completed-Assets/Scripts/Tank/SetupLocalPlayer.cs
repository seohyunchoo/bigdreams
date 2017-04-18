using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SetupLocalPlayer : NetworkBehaviour {
    // Use this for initialization
	public GameObject TankTurret;
    void Start () {
		if (isLocalPlayer)
        {   
            GetComponent<Complete.TankHealth>().enabled = true;
            GetComponent <Complete.TankMovement> ().enabled = true;
            GetComponent <Complete.TankShooting> ().enabled = true;
            GetComponentInChildren<Camera>().enabled = true;
			//Disable child TankTurret's Mesh Renderer 
			TankTurret.GetComponent<MeshRenderer> ().enabled = false;
			//Make minimap camera a child of the tank turret

			//GetComponentInChildren<>
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
