﻿using System.Collections;
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
    public GameObject labs;
    public GameObject guestButton;
    public GameObject labTemp;
    #endregion

    #region Private Variables 
    private List<GameObject> labList;
    Dictionary<string, string> labOptions;
    private string labSelected = "none";
    private bool placed = false;
    private bool playAnimation = true;
    private int currState = -1; 
    private enum state
    { 
        placement,
        usr_entry,
        pass_entry,
        authentication,
        lab_selection, 
        lab_initiation,
        end_of_states
    }
    #endregion

    #region MonoBehaviour
    // Start is called before the first frame update
    void Start()
    {
        labList = new List<GameObject>();
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
        if (playAnimation && intro.gameObject.transform.GetChild(0).GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            // prevents update from checking if intro is playing every frame
            playAnimation = false;

            // starts the rest of events in motion
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
            case (int)state.placement: // Placement scene
                {
                    // disable linerenderer and MTSU model to look cleaner
                    toggleLineRender(false);
                    intro.gameObject.transform.GetChild(0).gameObject.SetActive(false);

                    // Destroy Lab Selection Options if looped through to beginning - MOVEABLE
                    foreach (GameObject o in labList) { Destroy(o); }

                    // Start placement scene and ensure it hasn't already been anchored before it started 
                    placement_prop.SetActive(true);
                    placed = false;
                    break;
                }

            case (int)state.usr_entry: // User Entry
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

            case (int)state.pass_entry: // Pass Entry
                {
                    // Disable User entry
                    usr.SetActive(false);

                    // Enable Pass Entry
                    pas.SetActive(true);

                    // Sets the keyboard text box to the Password Text box
                    keyboard.GetComponent<VRKeyboard.Utils.KeyboardManager>().setText(pas.gameObject.transform.GetChild(0).GetComponent<Text>());
                    break;
                }

            case (int)state.authentication: // Authentication
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

            case (int)state.lab_selection: // Lab selection
                {
                    // Disable UI and Keyboard 
                    LoginUI.SetActive(false);
                    keyboard.SetActive(false);

                    // Load Modules 
                    labs.SetActive(true);
                    setLabs();
                    break;
                }

            case (int)state.lab_initiation: // Insantiate lab 
                {
                    // Disable all UI
                    labs.SetActive(false);

                    // Start Lab Manager
                    print("Lab selected: " + labSelected + ", but no info to load for now :^)");
                    startLab(labSelected);
                    break;
                }

                // Catch if looped and extends past defined states 
                // returns back to placement scene
            default:
                {
                    // Disable modules 
                    labs.SetActive(false);
                    print(labSelected);

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
            gotoState((int)state.lab_selection);
        else
            gotoState((int)state.usr_entry);

        // One-line alternative
        // gotoState((int)(this.usr.GetComponent<autofill>().authenticate(usr, pas)) ? state.modules : state.usr_entry);
    }

    // Instantiates UP TO 6 lab options that are clickable and load 
    private void setLabs()
    {
        // pull labs as a string list from autofill script using given username
        //              username-field    script        method               (username text)
        labOptions = usr.GetComponent<autofill>().getLabs(usr.transform.GetChild(0).GetComponent<Text>().text);

        int count = 0;
        foreach (string lab in labOptions.Keys)
        {
            // Create instance and position it
            GameObject newlab = Instantiate(labTemp, this.labs.transform);
            newlab.transform.position += new Vector3(.42f * (count % 2), -.15f * (count / 2), 0);

            // Give each lab a unique value onClick
            newlab.GetComponent<Button>().onClick.AddListener(delegate () { setLab(lab); });

            // Set Lab Title, description, name, and visibility
            newlab.transform.GetChild(0).GetComponent<Text>().text = format(lab);
            newlab.transform.GetChild(1).GetComponent<Text>().text = getDesc(labOptions[lab]);
            newlab.name = "Lab: " + lab;
            newlab.SetActive(true);

            // Keep track of Labs to destroy them later if needed 
            labList.Add(newlab);
            if (++count == 6) break;
        }
    }

    // Removes underscores and replaces them with spaces
    private string format(string lab)
    {
        char[] chars = lab.ToCharArray();
        for (int i = 0; i < chars.Length; i++)
        {
            if (chars[i] == '_') { chars[i] = ' '; }
        }

        return new string(chars);
    }

    // Returns lab description as a string  TODO
    private string getDesc(string jsonUrl)
    {
        // string temp = jsonUrl.Substring(jsonUrl.indexOf("Description: {"));
        // return jsonUrl.Substring(temp, temp.indexOf("}"));
        return jsonUrl;
    }

    // Called when a lab is clicked on; sets value of (string) labSelected
    private void setLab(string lab) 
    {
        labSelected = lab;
        Debug.Log("Lab Selected: " + format(labSelected));
        next();
    }

    // Instantiates the lab selected
    private void startLab(string lab)
    {
        // If exit is selected from the list, End program here
        // if (labSelected.Equals("Exit")) { Application.Quit(); }
        // else if (labSelected.Equals("none")) { gotoState((int)lab_selection); }

        var manifestPath = "http://cyberlearnar.cs.mtsu.edu/lab_manifest/"+lab;
        var jsonPath = labOptions[lab]; 
        // GameObject.Find("LabManager").GetComponent<LabManagerScript>().startLab(manifestPath,jsonPath);
    }

#endregion
}
