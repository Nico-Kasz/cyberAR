using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class loginLogic : MonoBehaviour
{
    #region Public Variables
    public GameObject LoginUI; 
    public GameObject usr;
    public GameObject pas;
    public GameObject loading;
    public GameObject intro;
    public GameObject keyboard; 
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
            keyboard.GetComponent<VRKeyboard.Utils.KeyboardManager>().setText(pas.gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).GetComponent<Text>());
        } 
        else if (pas.active)
        {
            pas.SetActive(false);
            keyboard.SetActive(false);
            loading.SetActive(true);
            // Send Request to server w/ encrypted user/pass 
        }
        else
        {
            // If loop back this far TODO: readd placeholders and clear text
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
