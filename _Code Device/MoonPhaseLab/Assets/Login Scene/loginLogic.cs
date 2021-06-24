using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class loginLogic : MonoBehaviour
{
    #region Public Variables
    public GameObject LoginUI; 
    public GameObject usr;
    public GameObject pas;
    public GameObject loading;
    public GameObject intro; 
    #endregion

    #region Private Variables 
    private bool UIStarted;
    #endregion

    #region MonoBehaviour
    // Start is called before the first frame update
    void Start()
    {
        UIStarted = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!UIStarted && intro.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            intro.SetActive(false);
            StartLoginUI();
        }
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

    #region Private Events
    private void StartLoginUI()
    {
        UIStarted = true;
        LoginUI.SetActive(true);
    }
    #endregion
}
