using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

namespace Complete
{
    public class TankShooting : NetworkBehaviour
    {
        public int m_PlayerNumber = 1;              // Used to identify the different players.
        public GameObject m_Shell;                   // Prefab of the shell.
        public GameObject m_SuperShell;
		public GameObject reloadingText;
        public Transform m_FireTransform;           // A child of the tank where the shells are spawned.
        //public Slider m_AimSlider;                  // A child of the tank that displays the current launch force.
        public AudioSource m_ShootingAudio;         // Reference to the audio source used to play the shooting audio. NB: different to the movement audio source.
        public AudioClip m_ChargingClip;            // Audio that plays when each shot is charging up.
        public AudioClip m_FireClip;                // Audio that plays when each shot is fired.
        public float m_LaunchForce;
        public float m_SlowByAmount;
		private float prevTime;
		public float m_reloadTime = 1.0f;

        private string m_FireButton;                // The input axis that is used for launching shells.
        //private float m_CurrentLaunchForce;         // The force that will be given to the shell when the fire button is released.
        private float m_ChargeSpeed;                // How fast the launch force increases, based on the max charge time.
        private bool m_Fired;                       // Whether or not the shell has been launched with this button press.
        [SyncVar]
        private bool m_usedSuper;

        private void OnEnable()
        {
            // When the tank is turned on, reset the launch force and the UI
            //m_CurrentLaunchForce = m_MinLaunchForce;
            //m_AimSlider.value = m_MinLaunchForce;
            m_usedSuper = false;
        }


        private void Start ()
        {
            if (!isLocalPlayer)
            {
                return;
            }
            // The fire axis is based on the player number.
            m_FireButton = "Fire" + m_PlayerNumber;
            m_usedSuper = false;
			prevTime = 0;
            // The rate that the launch force charges up is the range of possible forces by the max charge time.
            //m_ChargeSpeed = (m_MaxLaunchForce - m_MinLaunchForce) / m_MaxChargeTime;
        }


        private void Update ()
        {
            if (!isLocalPlayer)
            {
                return;
            }

			float timeDiff = Time.time - prevTime;
			if (timeDiff >= m_reloadTime) {
				reloadingText.GetComponent<MeshRenderer> ().enabled = false;
				if (Input.GetKeyDown ("m") || Input.GetKeyDown ("c")) {
					// ... reset the fired flag and reset the launch force.
					m_Fired = false;
					// Change the clip to the charging clip and start it playing.
					//    m_ShootingAudio.clip = m_ChargingClip;
					//    m_ShootingAudio.Play ();
				}
            // Otherwise, if the fire button is released and the shell hasn't been launched yet...
				else if (Input.GetKeyUp ("m") && !m_Fired) { //(Input.GetButtonUp (m_FireButton) && !m_Fired)
					// ... launch the shell.
					prevTime = Time.time;
					reloadingText.GetComponent<MeshRenderer> ().enabled = true;
					CmdFire ();
				} else if (Input.GetKeyUp ("c") && !m_Fired && !m_usedSuper) {
					m_usedSuper = true;
					prevTime = Time.time;
					reloadingText.GetComponent<MeshRenderer> ().enabled = true;
					CmdSuperFire ();
				}
			}
        }

        [Command]
        private void CmdFire ()
        {
            // Set the fired flag so Fire is only called once.
            m_Fired = true;

            // Create an instance of the shell and store a reference to it's rigidbody.
            var shell = Instantiate(
                m_Shell,
                m_FireTransform.position, 
                m_FireTransform.rotation);

            // Set the shell's velocity to the launch force in the fire position's forward direction.
            shell.GetComponent<Rigidbody>().velocity = m_LaunchForce * m_FireTransform.forward;
            // to hopefully prevent collision between shooter and bullet
            Physics.IgnoreCollision(shell.GetComponent<CapsuleCollider>(), GetComponent<BoxCollider>());
            
            NetworkServer.Spawn(shell);

            // Change the clip to the firing clip and play it.
            m_ShootingAudio.clip = m_FireClip;
            m_ShootingAudio.Play ();

            // Reset the launch force.  This is a precaution in case of missing button events.
            //m_CurrentLaunchForce = m_MinLaunchForce;
        }

        [Command]
        private void CmdSuperFire ()
        {
            // Set the fired flag so Fire is only called once.
            m_Fired = true;
            m_usedSuper = true;
            // Create an instance of the shell and store a reference to it's rigidbody.
            var shell = Instantiate(
                m_SuperShell,
                m_FireTransform.position,
                m_FireTransform.rotation);

            // Set the shell's velocity to the launch force in the fire position's forward direction.
            shell.GetComponent<Rigidbody>().velocity = (m_LaunchForce/m_SlowByAmount) * m_FireTransform.forward;
            // to hopefully prevent collision between shooter and bullet
            Physics.IgnoreCollision(shell.GetComponent<CapsuleCollider>(), GetComponent<BoxCollider>());

            NetworkServer.Spawn(shell);

            // Change the clip to the firing clip and play it.
            m_ShootingAudio.clip = m_FireClip;
            m_ShootingAudio.Play();

            // Reset the launch force.  This is a precaution in case of missing button events.
            //m_CurrentLaunchForce = m_MinLaunchForce;
        }
    }
}