using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MagicLeapTools
{
    public class realign : MonoBehaviour
    {
        #region Public Variabls

        #endregion

        #region Private Variables
        private Camera mainCamera;
        #if PLATFORM_LUMIN
            public ControlInput controller;
        #endif
        #endregion

        #region MonoBehaviours
        // Start is called before the first frame update
        void Start()
        {
            mainCamera = Camera.main;
            // controller = GetComponent<ControlInput>();
        }

        // Update is called once per frame
        void Update()
        {

        }
        #endregion

        #region Public Events
        public void updatePos()
        {
            transform.position = controller.transform.position; // + new Vector3(0, .5f, 0); 
            transform.eulerAngles = new Vector3(0, mainCamera.transform.eulerAngles.y, 0);

            // change orientation of starfield
            GameObject.Find("Starfield").transform.eulerAngles = new Vector3(0, mainCamera.transform.eulerAngles.y, 0);
        }
        #endregion
    }
}
