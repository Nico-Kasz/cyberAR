using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

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
    public GameObject guestButton; 
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
        StartCoroutine(DownloadFile("http://cyberlearnar.cs.mtsu.edu/show_uploaded/test_names.csv","Assets/Login Scene/csv bank/test_names.csv"));
        StartCoroutine(DownloadFile("http://cyberlearnar.cs.mtsu.edu/show_uploaded/crn_to_labs.csv","Assets/Login Scene/csv bank/crn_to_labs.csv"));
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
        print("Current state: " + (state)(++currState));

        switch (currState)  
        {
            case 0: 
                {
                    // Placement Scene
                    toggleLineRender(false);
                    modules.SetActive(false);
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
                    loading.SetActive(false);
                    placement_prop.SetActive(false);
                    LoginUI.SetActive(true);
                    usr.SetActive(true);
                    keyboard.SetActive(true);
                    guestButton.SetActive(true);
                    keyboard.GetComponent<VRKeyboard.Utils.KeyboardManager>().resetText();
                    pas.SetActive(false);
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
                    guestButton.SetActive(false);
                    loading.SetActive(true);


                    // THE AUTHENTICATION GOES HERE
                    authenticate(usr.transform.GetChild(0).GetComponent<Text>().text, pas.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text);
                    break;
                }

            case 4:
                {
                    // Load Modules 
                    loading.SetActive(false);
                    LoginUI.SetActive(false);
                    keyboard.SetActive(false);
                    modules.SetActive(true); 

                    break;
                }

                // Catch if looped and extends past defined states 
            default:
                { 
                    currState = -1;
                    next();
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
        // gotoState((int)state.modules);       //skips authentication if active
    }
    #endregion

    #region Private Events
    IEnumerator DownloadFile(string webpath, string path)
    {
        var uwr = new UnityWebRequest(webpath, UnityWebRequest.kHttpVerbGET);
        uwr.downloadHandler = new DownloadHandlerFile(path);
        yield return uwr.SendWebRequest();
        if (uwr.result != UnityWebRequest.Result.Success)
            Debug.LogError(uwr.error);
        else
            Debug.Log("File successfully downloaded and saved to " + path);
    }

    private void toggleLineRender(bool flag)
    {
        // print("doing the dirty work [|8^(");
        controller.GetComponent<LineRenderer>().enabled = flag;
        controller.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = flag;
    }

    private void gotoState(int state)
    {
        currState = state-1;  
        next();
    }

    private void authenticate(string usr, string pas) {
        // Psuedo: authenticate => next(); else => gotoState((int)state.usr_entry);
        if (this.usr.GetComponent<autofill>().authenticate(usr, pas))
            gotoState((int)state.modules);
        else
            gotoState((int)state.usr_entry);
    }
#endregion
}
