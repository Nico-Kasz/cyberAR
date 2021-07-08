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
    public Dropdown dropdown;  // Made public for now bc it isn't working as private
    private int showing = 0;
    private IDictionary<string, HashSet<string>> crnLabs;
    private IDictionary<string, string> usrPas = new Dictionary<string, string>();
    #endregion


    // Start is called before the first frame update
    void Start()
    {
        input = transform.GetChild(0).GetComponent<Text>();
        currText = input.text;
        // TESTING
        //string[] nameslist = new string[] { "jim", "sam", "chad", "cronie", "crestin", "crestin2", "crestion", "crestolemu", "crester", "crestunker", "Michael", "Christopher", "Jessica", "Matthew", "Ashley", "Jennifer", "Joshua", "Amanda", "Daniel", "David", "James", "Robert", "John", "Joseph", "Andrew", "Ryan", "Brandon", "Jason", "Justin", "Sarah", "William", "Jonathan", "Stephanie", "Brian", "Nicole", "Nicholas", "Anthony", "Heather", "Eric", "Elizabeth", "Adam", "Megan", "Melissa", "Kevin", "Steven", "Thomas", "Timothy", "Christina", "Kyle", "Rachel", "Laura", "Lauren", "Amber", "Brittany", "Danielle", "Richard", "Kimberly", "Jeffrey", "Amy", "Crystal", "Michelle", "Tiffany", "Jeremy", "Benjamin", "Mark", "Emily", "Aaron", "Charles", "Rebecca", "Jacob", "Stephen", "Patrick", "Sean", "Erin", "Zachary", "Jamie", "Kelly", "Samantha", "Nathan", "Sara", "Dustin", "Paul", "Angela", "Tyler", "Scott", "Katherine", "Andrea", "Gregory", "Erica", "Mary", "Travis", "Lisa", "Kenneth", "Bryan", "Lindsey", "Kristen", "Jose", "Alexander", "Jesse", "Katie", "Lindsay", "Shannon", "Vanessa", "Courtney", "Christine", "Alicia", "Cody", "Allison", "Bradley", "Samuel", "Shawn", "April", "Derek", "Kathryn", "Kristin", "Chad", "Jenna", "Tara", "Maria", "Krystal", "Jared", "Anna", "Edward", "Julie", "Peter", "Holly", "Marcus", "Kristina", "Natalie", "Jordan", "Victoria", "Jacqueline", "Corey", "Keith", "Monica", "Juan", "Donald", "Cassandra", "Meghan", "Joel", "Shane", "Phillip", "Patricia", "Brett", "Ronald", "Catherine", "George", "Antonio", "Cynthia", "Stacy", "Kathleen", "Raymond", "Carlos", "Brandi", "Douglas", "Nathaniel", "Ian", "Craig", "Brandy", "Alex", "Valerie", "Veronica", "Cory", "Whitney", "Gary", "Derrick", "Philip", "Luis", "Diana", "Chelsea", "Leslie", "Caitlin", "Leah", "Natasha", "Erika", "Casey", "Latoya", "Erik", "Dana", "Victor", "Brent", "Dominique", "Frank", "Brittney", "Evan", "Gabriel", "Julia", "Candice", "Karen", "Melanie", "Adrian", "Stacey", "Margaret", "Sheena", "Wesley", "Vincent", "Alexandra", "Katrina", "Bethany", "Nichole", "Larry", "Jeffery", "Curtis", "Carrie", "Todd", "Blake", "Christian", "Randy", "Dennis", "Alison", "Trevor", "Seth", "Kara", "Joanna", "Rachael", "Luke", "Felicia", "Brooke", "Austin", "Candace", "Jasmine", "Jesus", "Alan", "Susan", "Sandra", "Tracy", "Kayla", "Nancy", "Tina", "Krystle", "Russell", "Jeremiah", "Carl"}; // TESTING
        //foreach (string name in nameslist) { names.Add(name.ToLower()); }                                                                                        // TESTING
        crnLabs = pullCSV(names);
        dropdown = transform.GetComponent<Dropdown>();
        sort(names);
        // printNames();
        foreach (HashSet<string> key in crnLabs.Values)
        {
            string result = "";
            foreach (string str in key)
                result += str;
            print(result);
        }
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
    public void printNames()
    {
        print("printing names");
        foreach (string str in names) { print(str); }
    }

    public bool authenticate(string usr, string pas)
    {
        return usrPas.ContainsKey(usr) && usrPas[usr].Equals(pas);
    }

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
    /* Loads names into the system ATM and pushes labs into crn dictionary
     * @param names: returned list of names 
     * @return: Dictionary of crns with set of lab IDs
     */
    private IDictionary<string, HashSet<string>> pullCSV(List<string> names)
    {
        var result = new Dictionary<string, HashSet<string>>();
        string crnPath = "C:/Users/nrk2t/Documents/GitHub/cyberARclone/_Code Device/MoonPhaseLab/Assets/Login Scene/csv bank/crn_to_labs.csv";
        string  namesPath = "C:/Users/nrk2t/Documents/GitHub/cyberARclone/_Code Device/MoonPhaseLab/Assets/Login Scene/csv bank/test_names.csv";
        string[] lines = System.IO.File.ReadAllLines(namesPath);

        for (int i = 1; i < lines.Length; i++)
        {
            string[] columns = lines[i].Split(',');
            names.Add(columns[1]);
            usrPas.Add(columns[1], columns[4]);
        }

        lines = System.IO.File.ReadAllLines(crnPath);

        for (int i = 1; i < lines.Length; i++)
        {
            string[] columns = lines[i].Split(',');
            HashSet<string> hash = new HashSet<string>();
            for (int j = 1; j < columns.Length; j++) { hash.Add(columns[j]); }
            result.Add(columns[0], hash);
        }

        return result;
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
