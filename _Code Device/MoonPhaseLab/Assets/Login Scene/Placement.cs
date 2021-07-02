using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.VFX;

namespace MagicLeapTools
{
    public class Placement : MonoBehaviour
    {
#if PLATFORM_LUMIN
            public ControlInput controller;
            
#endif

        private bool anchored = false;
        private Vector3 position;
        private Vector3 rotation;

        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            if (!anchored)
            {
                transform.position = controller.transform.position; // + new Vector3(0, .5f, 0); 
                transform.eulerAngles = new Vector3(0, controller.transform.eulerAngles.y, 0);
            }
        }

        public void anchor()
        {
            anchored = true;
            position = transform.position;
            //GameObject.find("Starfield").GetComponent<VisualEffect>().Stop();
        }

    }
}
