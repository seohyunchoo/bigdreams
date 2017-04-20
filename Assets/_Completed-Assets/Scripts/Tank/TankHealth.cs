using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

namespace Complete
{
    public class TankHealth : NetworkBehaviour
    {
        public float m_StartingHealth = 100f;               // The amount of health each tank starts with.
        public Slider m_Slider;                             // The slider to represent how much health the tank currently has.
        public Image m_FillImage;                           // The image component of the slider.
        public Color m_FullHealthColor = Color.green;       // The color the health bar will be when on full health.
        public Color m_ZeroHealthColor = Color.red;         // The color the health bar will be when on no health.
        public GameObject m_ExplosionPrefab;                // A prefab that will be instantiated in Awake, then used whenever the tank dies.
		public GameObject TankTurret;
		public GameObject killScore;
		public GameObject deathScore;
		public GameObject warningSign;
		public float respawnTime;
		private float numDeaths;
		private float numKills;
        private WaitForSeconds RespawnWait;
        
		private Vector3 SpawnPosition;
		private Quaternion SpawnRotation;
        private AudioSource m_ExplosionAudio;               // The audio source to play when the tank explodes.
        private ParticleSystem m_ExplosionParticles;        // The particle system the will play when the tank is destroyed.
        

		[SyncVar(hook = "SetHealthUI")]
		private float m_CurrentHealth;                      // How much health the tank currently has.
        private bool m_Dead;                                // Has the tank been reduced beyond zero health yet?


        private void Awake ()
        {
            // Instantiate the explosion prefab and get a reference to the particle system on it.
            m_ExplosionParticles = Instantiate (m_ExplosionPrefab).GetComponent<ParticleSystem> ();

            // Get a reference to the audio source on the instantiated prefab.
            m_ExplosionAudio = m_ExplosionParticles.GetComponent<AudioSource> ();

            // Disable the prefab so it can be activated when it's required.
            m_ExplosionParticles.gameObject.SetActive (false);

            // Store the spawn location
            RespawnWait = new WaitForSeconds(respawnTime);
			SpawnPosition = transform.position;
			SpawnRotation = transform.rotation;
			numDeaths = 0;
			numKills = 0;
			m_CurrentHealth = m_StartingHealth;
			warningSign.GetComponent<MeshRenderer> ().enabled = false;
        }


        private void OnEnable()
        {
            // When the tank is enabled, reset the tank's health and whether or not it's dead.
            m_CurrentHealth = m_StartingHealth;
            m_Dead = false;
			killScore.GetComponent<TextMesh>().text = numKills.ToString();
			deathScore.GetComponent<TextMesh> ().text = numDeaths.ToString();
            // Update the health slider's value and color.
            SetHealthUI(m_CurrentHealth);
			warningSign.GetComponent<MeshRenderer> ().enabled = false;
        }


        public void TakeDamage (float amount)
        {
            if (!isServer)
            {
                return;
            }
            // Reduce current health by the amount of damage done.
            m_CurrentHealth -= amount;

            // Change the UI elements appropriately.
            //SetHealthUI (m_CurrentHealth);
			if (m_CurrentHealth < 32) {
				warningSign.GetComponent<MeshRenderer> ().enabled = true;
			}

            // If the current health is at or below zero and it has not yet been registered, call OnDeath.
            if (m_CurrentHealth <= 0f && !m_Dead)
            {
				GameObject[] players = GameObject.FindGameObjectsWithTag ("MainPlayer");
				for (int i = 0; i < players.Length; i++) {
					Debug.Log (i.ToString());
					if (!gameObject.Equals (players [i])) {
						players [i].GetComponent<TankHealth> ().RpcOnKill ();
					}
				}
                m_CurrentHealth = 0;
				warningSign.GetComponent<MeshRenderer> ().enabled = false;
                RpcOnDeath ();

            }
        }


        private void SetHealthUI (float health)
        {
            // Set the slider's value appropriately.
            m_Slider.value = health;
			if (health < 32) {
				warningSign.GetComponent<MeshRenderer> ().enabled = true;
			}

            // Interpolate the color of the bar between the choosen colours based on the current percentage of the starting health.
            m_FillImage.color = Color.Lerp (m_ZeroHealthColor, m_FullHealthColor, health / m_StartingHealth);
        }

		[ClientRpc]
		public void RpcOnKill() {
			// Set the flag so that this function is only called once.
			//m_Dead = true;

			// Turn the tank off.
			gameObject.SetActive(false);

			// Update the scoreboard
			numKills++;

			// Wait 3 seconds before respawning at the original position
			Invoke("resetPosition", respawnTime);
		
		}



        [ClientRpc]
        private void RpcOnDeath ()
        {
            if (!isLocalPlayer)
            {
                // Set the flag so that this function is only called once.
                m_Dead = true;

                // Move the instantiated explosion prefab to the tank's position and turn it on.
                m_ExplosionParticles.transform.position = transform.position;
                m_ExplosionParticles.gameObject.SetActive(true);

                // Play the particle system of the tank exploding.
                m_ExplosionParticles.Play();

                // Play the tank explosion sound effect.
                m_ExplosionAudio.Play();

                // Turn the tank off.
                gameObject.SetActive(false);

				numDeaths++;

                // Wait 3 seconds before respawning at the original position
                Invoke("resetPosition",respawnTime);
            }
            else if (isLocalPlayer) 
            {
                // Set the flag so that this function is only called once.
                m_Dead = true;

                // Move the instantiated explosion prefab to the tank's position and turn it on.
                m_ExplosionParticles.transform.position = transform.position;
                m_ExplosionParticles.gameObject.SetActive(true);

                // Play the particle system of the tank exploding.
                m_ExplosionParticles.Play();

                // Play the tank explosion sound effect.
                m_ExplosionAudio.Play();

                // Turn the tank off.
                gameObject.SetActive(false);

				// Update the scoreboard
				numDeaths++;

                // Wait 3 seconds before respawning at the original position
                Invoke("resetPosition", respawnTime);
            }
        }

		private void resetPlayers() {
			GameObject[] players = GameObject.FindGameObjectsWithTag ("MainPlayer");
			for (int i = 0; i < players.Length; i++) {
				Debug.Log (i.ToString());
				players [i].GetComponent<TankHealth>().resetPosition();
			}
		}

        public void resetPosition()
        {
            // reset the tank's original position and parameters
			m_CurrentHealth = m_StartingHealth;
            transform.position = SpawnPosition;
			transform.rotation = SpawnRotation;
			TankTurret.transform.localRotation = Quaternion.Euler (0, 0, 0);
            m_Dead = false;
            gameObject.SetActive(true);
        }
    }
}