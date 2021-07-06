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
    public GameObject anchor; 
    public GameObject placement_prop;
    public GameObject controller;
    public GameObject modules; 
    #endregion

    #region Private Variables 
    private bool placed = false; 
    private bool repeatUser = true; // This is for testing, this is replaced by authenticating user/pass server-side to determine whether or not to repeat
    private int currState = -1; 
    private enum state
    {
        placement,
        usr_entry,
        pass_entry,
        authentication,
        modules
    }
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
        
        // After initial animation, this will initiate placement scene, then the login screen 
        if (!placed && intro.gameObject.transform.GetChild(0).gameObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            print("Animation at idle; starting placement scene.");
            next();
        }
        if (placement_prop.active && !placed)
        {
            anchor.transform.position = controller.transform.position; // + new Vector3(0, .5f, 0); 
            anchor.transform.eulerAngles = new Vector3(0, controller.transform.eulerAngles.y, 0);
        } 
    }
    #endregion

    #region Public Events
    // Keep the flow of events involving the Login UI
    public void next()
    {
        if (++currState == 5) { currState = 0; }
        print("Current state: " + (state)currState);

        switch (currState)  
        {
            case 0: 
                {
                    // Placement Scene
                    toggleLineRender(false);
                    intro.gameObject.transform.GetChild(0).gameObject.SetActive(false);
                    placement_prop.SetActive(true);
                    placed = false;
                    break;
                }

            case 1: 
                {
                    // User Entry
                    // If loop back this far TODO: readd placeholders and clear text
                    toggleLineRender(true);
                    placement_prop.SetActive(false);
                    LoginUI.SetActive(true);
                    usr.SetActive(true);
                    keyboard.SetActive(true);
                    keyboard.GetComponent<VRKeyboard.Utils.KeyboardManager>().resetText();
                    pas.SetActive(false);
                    loading.SetActive(false);
                    break;
                }

            case 2: 
                {
                    // Pass Entry
                    usr.SetActive(false);
                    pas.SetActive(true);
                    // Sets the keyboard text box to the Password Text box
                    keyboard.GetComponent<VRKeyboard.Utils.KeyboardManager>().setText(pas.gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).GetComponent<Text>());
                    break;
                }

            case 3: 
                {
                    // Submit request
                    pas.SetActive(false);
                    keyboard.SetActive(false);
                    loading.SetActive(true);

                    // THE AUTHENTICATION GOES HERE
                    authenticate(usr.transform.GetChild(0).GetComponent<Text>().text, pas.transform.GetChild(0).GetComponent<Text>().text);
                    break;
                }

            case 4:
                {
                    // Load Modules 
                    LoginUI.SetActive(false);
                    modules.SetActive(true); 

                    break;
                }
            
        }
    }

    public void place()
    {
        if (!placed)
        {
            placed = true;
            next();
        }
    }

    public void guestLogin()
    {
        authenticate("guest", "guest");
        currState = 3;
        next();
    }
#endregion

#region Private Events
    private void toggleLineRender(bool flag)
    {
        print("doing the dirty work [|8^(");
        controller.GetComponent<LineRenderer>().enabled = flag;
        print("Line renderer: " + controller.GetComponent<LineRenderer>().enabled);
        controller.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = flag;
    }

    private void authenticate(string usr, string pas) { /* authenticate => next(); else => currstate = 0, next() */}
#endregion
}
