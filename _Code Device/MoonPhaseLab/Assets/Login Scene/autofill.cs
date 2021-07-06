using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class autofill : MonoBehaviour
{
    #region Public Variables 
    [Tooltip("This is how many characters are typed until a new list of names is requested.")]
    public int len = 3;
    public int optionLimit = 5;
    #endregion 

    #region Private Variables
    private List<string> names = new List<string>();
    private Text input;
    private string currText;
    private Dropdown dropdown;
    private int showing = 0; 
    #endregion


    // Start is called before the first frame update
    void Start()
    {
        input = transform.GetChild(0).GetComponent<Text>();
        currText = input.text;
        string[] nameslist = new string[] { "jim", "sam", "chad", "cronie", "crestin", "crestin2", "crestion", "crestolemu", "crester", "crestunker"}; // TESTING
        foreach (string name in nameslist) { names.Add(name); }                                                                                        // TESTING
        // pullCSV(names, path);
        dropdown = GetComponent<Dropdown>();
        sort(names);
        printNames();

    }

    // Update is called once per frame
    void Update()
    {
        // Only way to consistantly make it visible 
        if (input.text.Length >= len && showing > 0) { dropdown.Show(); }
        else { dropdown.Hide(); }

        // On change of text 
        if (!currText.Equals(input.text))
        {
            currText = input.text;
            print("Username updated to: " + currText);

            queryAndUpdate();
            makeShow();
        }
    }

    #region Public Methods 
    // Unsure if this works 
    public void OnSubmit(BaseEventData eventData) { GameObject.Find("[LOGIC]").transform.GetComponent<loginLogic>().next(); }

    public void printNames()
    {
        foreach (string str in names) { print(str); }
    }

    // TODO: FIX THIS 
    public void queryAndUpdate()
    {
        dropdown.Hide(); // Helps refresh dropdown menu
        // send Qury and retrieve new names list
        // foreach (string name in csv) { if (name.contains(name) ) List.Add(name);}
        // names = new names :)
        if (input.text.Length >= len)
        {
            dropdown.options.Clear();
            int count = 0;
            foreach (string name in names)
            {
                if (name.Contains(input.text) && count < optionLimit)
                {
                    dropdown.options.Add(new Dropdown.OptionData(name));
                    count++;
                }
            }
            showing = count;        // Not sure this is operational
        }

    }
    #endregion

    #region Private Methods 
    private void pullCSV(List<string> names, string path)
    {
        string[] lines = new string[0]; 
        // string[] lines = System.IO.File.ReadAllLines(path);      // No path set as of now 
        foreach (string line in lines)
        {
            string[] columns = line.Split(',');
            names.Add(columns[2]);
        }
       
    }

    private void sort(List<string> names)
    {
        names.Sort();                                               // used for List<string> implementation 
        // Array.Sort(names, (s1, s2) => String.Compare(s1, s2));   // used for string[] implementation
    }

    private void makeShow()
    {
        if (input.text.Length >= len) { dropdown.Show(); }
    }
    #endregion
}
