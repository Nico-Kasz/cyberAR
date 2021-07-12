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
    private List<User> users = new List<User>();
    private IDictionary<string, HashSet<string>> crnLabs;
    #endregion


    // Start is called before the first frame update
    void Start()
    {
        input = transform.GetChild(0).GetComponent<Text>();
        currText = input.text;
        // TESTING
        //string[] nameslist = new string[] { "jim", "sam", "chad", "cronie", "crestin", "crestin2", "crestion", "crestolemu", "crester", "crestunker", "Michael", "Christopher", "Jessica", "Matthew", "Ashley", "Jennifer", "Joshua", "Amanda", "Daniel", "David", "James", "Robert", "John", "Joseph", "Andrew", "Ryan", "Brandon", "Jason", "Justin", "Sarah", "William", "Jonathan", "Stephanie", "Brian", "Nicole", "Nicholas", "Anthony", "Heather", "Eric", "Elizabeth", "Adam", "Megan", "Melissa", "Kevin", "Steven", "Thomas", "Timothy", "Christina", "Kyle", "Rachel", "Laura", "Lauren", "Amber", "Brittany", "Danielle", "Richard", "Kimberly", "Jeffrey", "Amy", "Crystal", "Michelle", "Tiffany", "Jeremy", "Benjamin", "Mark", "Emily", "Aaron", "Charles", "Rebecca", "Jacob", "Stephen", "Patrick", "Sean", "Erin", "Zachary", "Jamie", "Kelly", "Samantha", "Nathan", "Sara", "Dustin", "Paul", "Angela", "Tyler", "Scott", "Katherine", "Andrea", "Gregory", "Erica", "Mary", "Travis", "Lisa", "Kenneth", "Bryan", "Lindsey", "Kristen", "Jose", "Alexander", "Jesse", "Katie", "Lindsay", "Shannon", "Vanessa", "Courtney", "Christine", "Alicia", "Cody", "Allison", "Bradley", "Samuel", "Shawn", "April", "Derek", "Kathryn", "Kristin", "Chad", "Jenna", "Tara", "Maria", "Krystal", "Jared", "Anna", "Edward", "Julie", "Peter", "Holly", "Marcus", "Kristina", "Natalie", "Jordan", "Victoria", "Jacqueline", "Corey", "Keith", "Monica", "Juan", "Donald", "Cassandra", "Meghan", "Joel", "Shane", "Phillip", "Patricia", "Brett", "Ronald", "Catherine", "George", "Antonio", "Cynthia", "Stacy", "Kathleen", "Raymond", "Carlos", "Brandi", "Douglas", "Nathaniel", "Ian", "Craig", "Brandy", "Alex", "Valerie", "Veronica", "Cory", "Whitney", "Gary", "Derrick", "Philip", "Luis", "Diana", "Chelsea", "Leslie", "Caitlin", "Leah", "Natasha", "Erika", "Casey", "Latoya", "Erik", "Dana", "Victor", "Brent", "Dominique", "Frank", "Brittney", "Evan", "Gabriel", "Julia", "Candice", "Karen", "Melanie", "Adrian", "Stacey", "Margaret", "Sheena", "Wesley", "Vincent", "Alexandra", "Katrina", "Bethany", "Nichole", "Larry", "Jeffery", "Curtis", "Carrie", "Todd", "Blake", "Christian", "Randy", "Dennis", "Alison", "Trevor", "Seth", "Kara", "Joanna", "Rachael", "Luke", "Felicia", "Brooke", "Austin", "Candace", "Jasmine", "Jesus", "Alan", "Susan", "Sandra", "Tracy", "Kayla", "Nancy", "Tina", "Krystle", "Russell", "Jeremiah", "Carl"}; // TESTING
        //foreach (string name in nameslist) { names.Add(name.ToLower()); }        // TESTING
        
        crnLabs = pullCSV(names);
        dropdown = GetComponent<Dropdown>();
        sort(names);
        // printNames();
        // foreach (string crn in crnLabs.Keys) { string outtie = ""; foreach (string str in getLabs(crn)) { outtie += str + " "; } print(outtie);  }
        foreach (User usr in users) { print(usr.ToString()); }
        users.Add(new User("guest", "guest", "0000000"));
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
            print("Username updated to: " + currText);     // for debugging

            updateDropdown();
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
        return true; // Leap test; DELETE WHEN YOU SEE THIS 
        try {
            Debug.Log("Username: " + usr + ", Password: " + pas + "\n\t    Authenticated: " + (users.Find(x => x.usr.Equals(usr)).pas.Equals(pas)));
            return users.Find(x => x.usr.Equals(usr)).pas.Equals(pas);
        } catch  {
            print("Authentication Failed: Invalid Username");
            return false;
        }
    }


    public void updateDropdown()
    {
        dropdown.Hide(); // Helps refresh dropdown menu
        // send Qury and retrieve new names list
        // foreach (string name in csv) { if (name.contains(name) ) List.Add(name);}
        // names = new names :)
        if (input.text.Length >= len)
        {
            dropdown.options.Clear();   // Ensures list is empty to start filling
            int count = 0;

            // Cycle names list and pick ones that contain provided string
            foreach (string name in names)
            {
                if (name.Contains(input.text) && count < optionLimit)
                {
                    dropdown.options.Add(new Dropdown.OptionData(name));
                    count++;
                }
            } 
            showing = count;    // Ensures that if no options appear, a blank dropdown won't appear
        }

    }

    // Returns a sorted String Array of all labs associated with a CRN 
    public string[] getLabs(string usr)
    {
        string crn = "0000000";  // Guest crn: seven 0s
        try { crn = users.Find(x => x.usr.Equals(usr)).crn; }
        catch { print("using guest button or invalid Username"); }
        string[] labs = new string[crnLabs[crn].Count];

        // Copy HashSet to String[]
        crnLabs[crn].CopyTo(labs);

        // Sort and return new Array
        Array.Sort(labs, (s1, s2) => String.Compare(s1, s2));
        return labs;
    }
    #endregion

    #region Private Methods 
    /* Loads names into the system while creating a list of User objects and pushes labs into crn dictionary.
     * @param List<string> names: List that this method stores usernames in.
     * @return: Dictionary of crns with set of lab IDs.
     */
    private IDictionary<string, HashSet<string>> pullCSV(List<string> names)
    {
        var result = new Dictionary<string, HashSet<string>>();

        // Set path for where to pull data from
        string crnPath = "Assets/Login Scene/csv bank/crn_to_labs.csv";
        string  namesPath = "Assets/Login Scene/csv bank/test_names.csv";
        string[] lines = System.IO.File.ReadAllLines(namesPath);

        for (int i = 1; i < lines.Length; i++)
        {
            string[] columns = lines[i].Split(',');
            names.Add(columns[0]);
            users.Add(new User(columns[0], columns[4], columns[2]));
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
    #endregion
}

/* Used to store Student Usernames, Passwords, and CRNs as of now 
 * All objects are added to a list where they can be searched
*/
class User 
{
    public string usr;
    public string pas;
    public string crn;

    public User(string usr, string pas, string crn)
    {
        this.usr = usr;
        this.pas = pas;
        this.crn = crn; 
    }

    public string ToString() { return ("Username: " + usr + "\tPassword: " + pas + "\t\tCRN: " + crn); }
}
