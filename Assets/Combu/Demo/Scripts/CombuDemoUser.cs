using System.Collections;
using Combu;

/*
 * This is a sample class that extends the core User class,
 * we created this to easily handle the custom properties of our accounts,
 * if we will pass this class to Authenticate methods then we can safely cast localUser to this class later
 * 
 * The examples (both scenes and scripts) are provided as-are and can be eventually modified over time to reflect major changes to API or just to be improved.
 * So we discourage the approach of editing any demo file, but you could create your own scripts and inherit their classes from these examples if you want,
 * and eventually override the methods that you want to change or add more input fields and stuff to fit your needs.
 */
public class CombuDemoUser : User
{
	string _myProperty1 = "";
	int _myProperty2 = 0;
	int _coins = 0;

	public string myProperty1
	{
		get { return _myProperty1; }
		set { _myProperty1 = value; customData["myProperty1"] = _myProperty1; }
	}
	
	public int myProperty2
	{
		get { return _myProperty2; }
		set { _myProperty2 = value; customData["myProperty2"] = _myProperty2; }
	}
	
	public int coins
	{
		get { return _coins; }
		set { _coins = value; customData["Coins"] = _coins; }
	}

	public CombuDemoUser()
	{
		myProperty1 = "";
		myProperty2 = 0;
		coins = 0;
	}

	public override void FromHashtable (Hashtable hash)
	{
		// Set User class properties
		base.FromHashtable (hash);

		// Set our own custom properties that we store in customData
		if (customData.ContainsKey("myProperty1"))
			_myProperty1 = customData["myProperty1"].ToString();
		if (customData.ContainsKey("myProperty2"))
			_myProperty2 = int.Parse(customData["myProperty2"].ToString());
		if (customData.ContainsKey("Coins"))
			_coins = int.Parse(customData["Coins"].ToString());
	}
}
