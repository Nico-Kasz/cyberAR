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
    public GameObject placement_prop;
    #endregion

    #region Private Variables 
    private bool UIStarted = false;
    private bool repeatUser = true; // This is for testing, this is replaced by authenticating user/pass server-side to determine whether or not to repeat
    #endregion

    #region MonoBehaviour
    // Start is called before the first frame update
    void Start()
    {
        intro.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    { 
        // After initial animation, this will initiate login screen 
        if (!UIStarted && intro.gameObject.transform.GetChild(0).gameObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            intro.gameObject.transform.GetChild(0).gameObject.SetActive(false);
            StartLoginUI();
        }
    }
    #endregion

    #region Public Events
    // Keep the flow of events involving the Login UI
    public void next()
    {
        if (usr.active)
        {
            usr.SetActive(false);
            pas.SetActive(true);
            // Sets the keyboard text box to the Password Text box
            keyboard.GetComponent<VRKeyboard.Utils.KeyboardManager>().setText(pas.gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).GetComponent<Text>());
        }
        else if (pas.active)
        {
            pas.SetActive(false);
            keyboard.SetActive(false);
            loading.SetActive(true);
            // Send Request to server w/ encrypted user/pass 
            // Psudo: 
            // repeatUser = authenticate.valid(usr,pass);

            // wait(5); // simulate authentication - TODO (doesn't work)
            if (!repeatUser)
            {
                LoginUI.SetActive(false);
                #if PLATFORM_LUMIN
                //GameObject.find("[AR_CONTENT]").GetComponent<ControlInput>().GetChild(0).SetActive(false);
                #endif

                // Show mesh 
                placement_prop.SetActive(true);
            }
            // else { next(); } // disable this line to remain on the loading screen during tests 
        }
        else
        {
            // If loop back this far TODO: readd placeholders and clear text
            LoginUI.SetActive(true);
            usr.SetActive(true);
            keyboard.SetActive(true);
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
        keyboard.SetActive(true); // for ease of usage - after dropbox was added 
    }
#endregion
}
