using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class autofill : MonoBehaviour
{
    #region Private Variables
    private string[] names;
    private string text; 
    #endregion


    // Start is called before the first frame update
    void Start()
    {
        names = new string[4] { "jim", "sam", "chad", "cronie" };
        queryAndUpdate();
        printNames();
        
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    #region Public Methods 
    public void updateText()
    {
        text = GameObject.Find("Keyboard").GetComponent<VRKeyboard.Utils.KeyboardManager>().std_text_box.text;
    }

    public void printNames()
    {
        foreach (string str in names) { print(str); }
    }

    public void queryAndUpdate()
    {
        // send Qury and retrieve new names list
        // foreach (string name in csv) { if (name.contains(name) 
        // names = new names :)
        List<string> strings = new List<string>();
        strings.Add("jeff");
        for (int i = 0; i < strings.Count; i++) { names[i] = strings[i]; }
        // Update
        sort();
        setOptions();
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
        dropdown.options.Add(new Dropdown.OptionData(text));
        foreach (string name in names)
        {
            dropdown.options.Add(new Dropdown.OptionData(name));
        }
    }
    #endregion
}
