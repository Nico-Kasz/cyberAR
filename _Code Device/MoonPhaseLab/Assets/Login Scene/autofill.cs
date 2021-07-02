using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class autofill : MonoBehaviour
{
    public bool testingVersion;

    #region Private Variables
    private List<string> names = new List<string>();
    private string text;
    private Dropdown dropdown;
    #endregion


    // Start is called before the first frame update
    void Start()
    {
        text = transform.GetChild(0).GetComponent<Text>().text;
        string[] nameslist = new string[] { "jim", "sam", "chad", "cronie", "crestin", "crestolemu", "crester", "crestunker"};
        foreach (string name in nameslist) { names.Add(name); }
        dropdown = GetComponent<Dropdown>();

        queryAndUpdate();
        printNames();

    }

    // Update is called once per frame
    void Update()
    {
        if (!text.Equals(transform.GetChild(0).GetComponent<Text>().text))
        {
            text = transform.GetChild(0).GetComponent<Text>().text;
            print("Username updated to: " + text);

            if (testingVersion)
            {
                sort(names);
                setOptions(names);
            }
            else
            {
                queryAndUpdate();  // for real version
            }
        }
    }

    #region Public Methods 
    public void printNames()
    {
        foreach (string str in names) { print(str); }
    }

    // TODO: FIX THIS 
    public void queryAndUpdate()
    {
        List<string> strings = new List<string>();
        if (text.Length >= 3)
        {
            dropdown.Show();
            // send Qury and retrieve new names list
            // foreach (string name in csv) { if (name.contains(name) ) List.Add(name);}
            // names = new names :)
            foreach (string name in names) { if (name.Contains(text)) { strings.Add(name); } }
        }
        else
        {
            dropdown.Hide();
        }
        // Update
        sort(strings);
        setOptions(strings);
        dropdown.RefreshShownValue();
    }
    #endregion

    #region Private Methods 
    private void sort(List<string> names)
    {
        names.Sort();  // used for List<string> implementation 
        // Array.Sort(names, (s1, s2) => String.Compare(s1, s2));   // used for string[] implementation
    }

    private void setOptions(List<string> names)
    {
        dropdown.options.Clear();
        dropdown.options.Add(new Dropdown.OptionData(text)); // might delete this line depending on how this works - repetetive 
        foreach (string name in names)
        {
            dropdown.options.Add(new Dropdown.OptionData(name));
        }
    }
    #endregion
}
