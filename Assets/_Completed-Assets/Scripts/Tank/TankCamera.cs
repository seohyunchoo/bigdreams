using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

namespace Complete
{
    public class TankCamera : NetworkBehaviour
    {
        public int m_PlayerNumber = 1;              // Used to identify the different players..
        public GameObject CameraRig;

        private void OnEnable()
        {


        }


        private void Start ()
        {
            if (!isLocalPlayer)
            {
                CameraRig.SetActive(false);
            }
        }
    }
}