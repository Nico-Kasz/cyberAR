using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class loginLogic : MonoBehaviour
{
    #region Public Variables
    public GameObject usr;
    public GameObject pas;
    public GameObject loading;
    #endregion

    #region MonoBehaviour
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    #endregion

    #region Public Events
    public void next()
    {
        if (usr.active)
        {
            usr.SetActive(false);
            pas.SetActive(true);
        }
        else if (pas.active)
        {
            pas.SetActive(false);
            loading.SetActive(true);
        }
        else
        {
            usr.SetActive(true);
            pas.SetActive(false);
            loading.SetActive(false);
        }

    }
    #endregion
}
