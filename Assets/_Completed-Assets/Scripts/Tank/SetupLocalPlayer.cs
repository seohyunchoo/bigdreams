using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SetupLocalPlayer : NetworkBehaviour {
    // Use this for initialization
	public GameObject TankTurret;
	public GameObject cockpit1;
	public GameObject cockpit2;
	public GameObject cockpit3;
	public GameObject cockpit4;
	public GameObject cockpit5;
	public GameObject cockpit6;
	public GameObject cockpit7;
	public GameObject cockpit8;
	public GameObject cockpit9;
	public GameObject cockpit10;
	public GameObject cockpit11;
	public GameObject cockpit12;
	public GameObject minimapIcon1;
	public GameObject minimapIcon2;
	public GameObject cockpit;

	public GameObject m_minimapCam;
    void Start () {
		if (isLocalPlayer)
        {   
			// Make tank turret invisible
			TankTurret.GetComponent<MeshRenderer> ().enabled = false;
			// Make cockpit visible
			MeshRenderer[] renderers = cockpit.GetComponentsInChildren<MeshRenderer> ();
			for (int i = 0; i < renderers.Length; i++) {
				renderers [i].enabled = true;
			}
            GetComponent<Complete.TankHealth>().enabled = true;
            GetComponent <Complete.TankMovement> ().enabled = true;
            GetComponent <Complete.TankShooting> ().enabled = true;
            GetComponentInChildren<Camera>().enabled = true;
			MeshRenderer[] comps1 = minimapIcon1.GetComponentsInChildren<MeshRenderer> ();
			for (int i = 0; i < comps1.Length; i++) {
				comps1 [i].material.color = Color.white;
			}
			MeshRenderer[] comps2 = minimapIcon2.GetComponentsInChildren<MeshRenderer> ();
			for (int i = 0; i < comps2.Length; i++) {
				comps2 [i].material.color = Color.blue;
			}
			// Make minimap camera a child of the tank turret
			//GameObject minimapCam = GameObject.FindGameObjectWithTag("MinimapCamera");
			GameObject minimapCam = Instantiate (m_minimapCam,new Vector3(0,20,0),Quaternion.Euler(90,0,0));
			minimapCam.transform.SetParent (TankTurret.transform,false);
			// Make self cockpit visible, but not other players'
//			cockpit1.layer = LayerMask.NameToLayer("SelfCockpit");
//			cockpit2.layer = LayerMask.NameToLayer("SelfCockpit");
//			cockpit3.layer = LayerMask.NameToLayer("SelfCockpit");
//			cockpit4.layer = LayerMask.NameToLayer("SelfCockpit");
//			cockpit5.layer = LayerMask.NameToLayer("SelfCockpit");
//			cockpit6.layer = LayerMask.NameToLayer("SelfCockpit");
//			cockpit7.layer = LayerMask.NameToLayer("SelfCockpit");
//			cockpit8.layer = LayerMask.NameToLayer("SelfCockpit");
//			cockpit9.layer = LayerMask.NameToLayer("SelfCockpit");
//			cockpit10.layer = LayerMask.NameToLayer("SelfCockpit");
//			cockpit11.layer = LayerMask.NameToLayer("SelfCockpit");
//			cockpit12.layer = LayerMask.NameToLayer("SelfCockpit");
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}