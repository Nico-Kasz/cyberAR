using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class autofill : MonoBehaviour
{
    public bool testingVersion;

    #region Private Variables
    private string[] names;
    private string text;
    #endregion


    // Start is called before the first frame update
    void Start()
    {
        text = transform.GetChild(0).GetComponent<Text>().text;
        names = new string[4] { "jim", "sam", "chad", "cronie" };
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
                sort();
                setOptions();
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

    public void queryAndUpdate()
    {
        if (text.Length >= 3)
        {
            // send Qury and retrieve new names list
            // foreach (string name in csv) { if (name.contains(name) ) List.Add(name);}
            // names = new names :)
            List<string> strings = new List<string>();
            strings.Add("jeff");
            for (int i = 0; i < strings.Count; i++) { names[i] = strings[i]; }
            // Update
            sort();
            setOptions();
        }
    }
    #endregion

    #region Private Methods 
    private void sort()
    {
        Array.Sort(names, (s1, s2) => String.Compare(s1, s2));
    }

    private void setOptions()
    {
        var dropdown = GetComponent<Dropdown>();
        dropdown.options.Clear();
        transform.GetChild(0).GetComponent<Text>().text = text;
        dropdown.options.Add(new Dropdown.OptionData(text)); // might delete this line depending on how this works - repetetive 
        foreach (string name in names)
        {
            dropdown.options.Add(new Dropdown.OptionData(name));
        }
    }
    #endregion
}
