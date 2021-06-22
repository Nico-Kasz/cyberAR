using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class sendInfo : MonoBehaviour
{
    #region public variables
    public Text usr_text;
    public Text pas_text;
    #endregion

    #region MonoBehaaviours 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    #endregion

    #region collect info
    public void Info()
    {
        Debug.Log("{ " + usr_text.text + " }\n{ " + pas_text.text + " }");
        return "{ " + usr_text.text + " }\n{ " + pas_text.text + " }";  
    }
    #endregion
}
