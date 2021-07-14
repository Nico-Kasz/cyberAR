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
    private int currState = -1; 
    private enum state
    {
        placement,
        usr_entry,
        pass_entry,
        authentication,
        modules, 
        end_of_states
    }
    #endregion

    #region MonoBehaviour
    // Start is called before the first frame update
    void Start()
    {
        intro.SetActive(true);
        StartCoroutine(DownloadFile("http://cyberlearnar.cs.mtsu.edu/show_uploaded/test_names.csv","Assets/Login Scene/csv bank/test_names.csv"));
        StartCoroutine(DownloadFile("http://cyberlearnar.cs.mtsu.edu/show_uploaded/crn_to_labs.csv","Assets/Login Scene/csv bank/crn_to_labs.csv"));
        toggleLineRender(false);
    }

    // Update is called once per frame
    void Update()
    {
        // disable line renderer in startup animation
        // Might cuase lag due to being checked every update
        if (currState == -1) 
            toggleLineRender(false);

        // After initial animation, this will initiate placement scene, then the login screen 
        if (intro.gameObject.transform.GetChild(0).GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            print("Animation at idle; starting placement scene. :)");
            next();
        }
        if (placement_prop.active && !placed)
        {
            anchor.transform.position = controller.transform.position; // + new Vector3(0, .5f, 0); 
            // anchor.transform.eulerAngles = new Vector3(0, controller.transform.eulerAngles.y, 0);  // I Disabled this after adding simple rotation to it 
        } 
    }
    #endregion

    #region Public Events
    // OnHomeButtonDown() realigns UI to position of controller and angle head is pointing
    public void realign() 
    {
        print("Realigning UI.");
        anchor.transform.position = controller.transform.position;
        anchor.transform.eulerAngles = new Vector3(0, Camera.main.transform.eulerAngles.y, 0);

        // change orientation of starfield
        GameObject.Find("Starfield").transform.eulerAngles = new Vector3(0, Camera.main.transform.eulerAngles.y, 0);
    }

    // Keep the flow of events involving the Login UI
    public void next()
    {
        print("Current state: " + (state)(++currState) + "\n========================");

        switch (currState)  
        {
            case 0: // Placement scene
                {
                    // disable linerenderer and MTSU model to look cleaner
                    toggleLineRender(false);
                    intro.gameObject.transform.GetChild(0).gameObject.SetActive(false);

                    // Start placement scene and ensure it hasn't already been anchored before it started 
                    placement_prop.SetActive(true);
                    placed = false;
                    break;
                }

            case 1: // User Entry
                {
                    // Cleanup placement object
                    toggleLineRender(true);
                    placement_prop.SetActive(false);

                    // Cleanup failed authentication
                    loading.SetActive(false);
                    print("clearing usernames");
                    usr.GetComponent<Dropdown>().options.Clear();

                    // Start UI
                    LoginUI.SetActive(true);
                    usr.SetActive(true);
                    keyboard.SetActive(true);
                    guestButton.SetActive(true);
                    pas.SetActive(false);
                    break;
                }

            case 2: // Pass Entry
                {
                    // Disable User entry
                    usr.SetActive(false);

                    // Enable Pass Entry
                    pas.SetActive(true);

                    // Sets the keyboard text box to the Password Text box
                    keyboard.GetComponent<VRKeyboard.Utils.KeyboardManager>().setText(pas.gameObject.transform.GetChild(0).GetComponent<Text>());
                    break;
                }

            case 3: // Authentication
                {
                    // Cleanup in-case authentication fails
                    usr.GetComponent<autofill>().refreshText();
                    keyboard.GetComponent<VRKeyboard.Utils.KeyboardManager>().resetText();

                    // Submit request
                    pas.SetActive(false);
                    keyboard.SetActive(false);
                    guestButton.SetActive(false);
                    loading.SetActive(true);


                    // THE AUTHENTICATION GOES HERE
                    authenticate(usr.transform.GetChild(0).GetComponent<Text>().text, pas.transform.GetChild(0).GetComponent<Text>().text);
                    break;
                }

            case 4: // Modules
                {
                    // Disable UI and Keyboard 
                    LoginUI.SetActive(false);
                    keyboard.SetActive(false);

                    // Load Modules 
                    modules.SetActive(true);
                    setModules();
                    break;
                }

                // Catch if looped and extends past defined states 
                // returns back to placement scene
            default:
                {
                    // Disable modules 
                    modules.SetActive(false);

                    // Loop back to Start
                    gotoState(0);
                    break;
                }
        }
    }

    // Anchors scene to location of controller 
    public void place()
    {
        // Ensures that prop isn't anchored before the placement scene
        if (placement_prop.active && !placed)
        {
            placed = true;
            next();
        }
    }

    // Called to automatically log in as "guest"
    public void guestLogin()
    {
        // Fill in usr/pas perameters so no errors will be thrown 
        // Also disables placeholders so it doesn't look ugly if user didn't type in user or pas perameters and has to retern to start
        usr.transform.GetChild(0).GetComponent<Text>().text = "guest";
        usr.transform.GetChild(1).gameObject.SetActive(false);
        pas.transform.GetChild(0).GetComponent<Text>().text = "guest";
        pas.transform.GetChild(1).gameObject.SetActive(false);

        // Goes through normal authentication after filling in both perameters
        gotoState((int)state.authentication);
    }
    #endregion

    #region Private Events
    // Downloads CSVs before autofill script on usr_dropbox is instantiated 
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

    // Toggles line renderer emitted from controller
    private void toggleLineRender(bool flag)
    {
        // print("doing the dirty work [|8^(");
        controller.GetComponent<LineRenderer>().enabled = flag;
        controller.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = flag;
    }

    // Called to change state if the state requested cannot be reached calling next();
    private void gotoState(int state)
    {
        currState = state-1;  
        next();
    }

    // Calls script in autofill to authenticate based on usr/pas logged 
    private void authenticate(string usr, string pas) {
        if (this.usr.GetComponent<autofill>().authenticate(usr, pas))
            gotoState((int)state.modules);
        else
            gotoState((int)state.usr_entry);
        // One-line alternative
        // gotoState((int)(this.usr.GetComponent<autofill>().authenticate(usr, pas)) ? state.modules : state.usr_entry);
    }

    // Assigns a String to a Text Field on the Modules tab
    // NOT final implementation; just to pull crn and associated lab data together
    private void setModules()
    {
        string[] labs = usr.GetComponent<autofill>().getLabs(usr.transform.GetChild(0).GetComponent<Text>().text);
        string modulesTxt = "";
        for (int i = 0; i < labs.Length; i++)
        {
            modulesTxt += (i+1) + ": " + labs[i] + "\n";
        }
        modules.transform.GetChild(2).GetComponent<Text>().text = modulesTxt;
        print("Module Information set to:\n" + modulesTxt);
    }
#endregion
}
