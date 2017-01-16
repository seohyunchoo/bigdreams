using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SetupLocalPlayer : NetworkBehaviour {
    // Use this for initialization
    void Start () {
		if (isLocalPlayer)
        {   
            GetComponent<Complete.TankHealth>().enabled = true;
            GetComponent <Complete.TankMovement> ().enabled = true;
            GetComponent <Complete.TankShooting> ().enabled = true;
            GetComponentInChildren<Camera>().enabled = true;
            Camera.main.transform.position = this.transform.position - this.transform.forward * 2 + this.transform.up * 3;
            Camera.main.transform.LookAt(this.transform.position);
            Camera.main.transform.parent = this.transform;
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
