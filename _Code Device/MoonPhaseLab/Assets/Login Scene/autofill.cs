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
        // TESTING
        string[] nameslist = new string[] { "jim", "sam", "chad", "cronie", "crestin", "crestin2", "crestion", "crestolemu", "crester", "crestunker", "Michael", "Christopher", "Jessica", "Matthew", "Ashley", "Jennifer", "Joshua", "Amanda", "Daniel", "David", "James", "Robert", "John", "Joseph", "Andrew", "Ryan", "Brandon", "Jason", "Justin", "Sarah", "William", "Jonathan", "Stephanie", "Brian", "Nicole", "Nicholas", "Anthony", "Heather", "Eric", "Elizabeth", "Adam", "Megan", "Melissa", "Kevin", "Steven", "Thomas", "Timothy", "Christina", "Kyle", "Rachel", "Laura", "Lauren", "Amber", "Brittany", "Danielle", "Richard", "Kimberly", "Jeffrey", "Amy", "Crystal", "Michelle", "Tiffany", "Jeremy", "Benjamin", "Mark", "Emily", "Aaron", "Charles", "Rebecca", "Jacob", "Stephen", "Patrick", "Sean", "Erin", "Zachary", "Jamie", "Kelly", "Samantha", "Nathan", "Sara", "Dustin", "Paul", "Angela", "Tyler", "Scott", "Katherine", "Andrea", "Gregory", "Erica", "Mary", "Travis", "Lisa", "Kenneth", "Bryan", "Lindsey", "Kristen", "Jose", "Alexander", "Jesse", "Katie", "Lindsay", "Shannon", "Vanessa", "Courtney", "Christine", "Alicia", "Cody", "Allison", "Bradley", "Samuel", "Shawn", "April", "Derek", "Kathryn", "Kristin", "Chad", "Jenna", "Tara", "Maria", "Krystal", "Jared", "Anna", "Edward", "Julie", "Peter", "Holly", "Marcus", "Kristina", "Natalie", "Jordan", "Victoria", "Jacqueline", "Corey", "Keith", "Monica", "Juan", "Donald", "Cassandra", "Meghan", "Joel", "Shane", "Phillip", "Patricia", "Brett", "Ronald", "Catherine", "George", "Antonio", "Cynthia", "Stacy", "Kathleen", "Raymond", "Carlos", "Brandi", "Douglas", "Nathaniel", "Ian", "Craig", "Brandy", "Alex", "Valerie", "Veronica", "Cory", "Whitney", "Gary", "Derrick", "Philip", "Luis", "Diana", "Chelsea", "Leslie", "Caitlin", "Leah", "Natasha", "Erika", "Casey", "Latoya", "Erik", "Dana", "Victor", "Brent", "Dominique", "Frank", "Brittney", "Evan", "Gabriel", "Julia", "Candice", "Karen", "Melanie", "Adrian", "Stacey", "Margaret", "Sheena", "Wesley", "Vincent", "Alexandra", "Katrina", "Bethany", "Nichole", "Larry", "Jeffery", "Curtis", "Carrie", "Todd", "Blake", "Christian", "Randy", "Dennis", "Alison", "Trevor", "Seth", "Kara", "Joanna", "Rachael", "Luke", "Felicia", "Brooke", "Austin", "Candace", "Jasmine", "Jesus", "Alan", "Susan", "Sandra", "Tracy", "Kayla", "Nancy", "Tina", "Krystle", "Russell", "Jeremiah", "Carl"}; // TESTING
        foreach (string name in nameslist) { names.Add(name.ToLower()); }                                                                                        // TESTING
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
    public void OnPointerClick(BaseEventData eventData) { GameObject.Find("[LOGIC]").transform.GetComponent<loginLogic>().next(); }

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
