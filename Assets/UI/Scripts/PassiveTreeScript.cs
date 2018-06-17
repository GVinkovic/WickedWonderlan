using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PassiveTreeScript : MonoBehaviour {

	public static bool TreeIsActive = false;
	public GameObject PassiveTreeUI;
	public GameObject StatsUI;
	public GameObject ToolTipUI;
	public InputManager InputManagerDatabase;
	private int StatPoints = 0;
	private int TreePoints = 0;
	private int PassivePoints = 0;
	private int PointsConstNum = 0;
	private int PointsIntelNum = 0;
	private int PointsDexNum = 0;
	private int PointsStrNum = 0; 
	private int CurrentLevelNum = 0;
	private int Initial = 0;
	private int Counter = 0;
	private Button BtnAddDex, BtnAddStr, BtnAddIntel, BtnAddConst;
	private Text PointsAvail, PointsDex, PointsConst, PointsStr, PointsIntel, PointsAvailT, CurrentLevel, ToolTipText;
	private Button o1, o2, o3, o4, o5, o6, o7, o8, o9, o10;
	private int bo1, bo2, bo3, bo4, bo5, bo6, bo7, bo8, bo9, bo10 = 0;



	public static PassiveTreeScript instance;

	void Awake(){

		instance = this;
	}
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(InputManagerDatabase.PassiveSkillTreeCode)) {

			if (TreeIsActive) 
			{
				Resume ();
			} else
			{
				Pause ();	 
			}
		}
	}

	public void Resume()
	{

		TreeIsActive = false;
		PlayerManager.instance.WindowClosed();
		PassiveTreeUI.SetActive (false);
		StatsUI.SetActive (false);
		ToolTipUI.SetActive (false);

		//	Cursor.visible = false;

	}

	void Pause()
	{
		PlayerManager.instance.WindowOpened();
		PassiveTreeUI.SetActive (true);
		StatsUI.SetActive (true);
		TreeIsActive = true;
		CurrentLevel = GameObject.Find ("Text_CurrentLevel").GetComponent<Text> ();
		CurrentLevel.text = PlayerManager.Level.ToString();
		CurrentLevelNum = PlayerManager.Level;
		BtnAddDex = GameObject.Find ("Btn_AddDex").GetComponent<Button> ();
		BtnAddStr = GameObject.Find ("Btn_AddStr").GetComponent<Button> ();
		BtnAddIntel = GameObject.Find ("Btn_AddIntel").GetComponent<Button> ();
		BtnAddConst = GameObject.Find ("Btn_AddConst").GetComponent<Button> ();
		PointsAvail = GameObject.Find ("Points_Avail").GetComponent<Text> ();
		PointsAvailT = GameObject.Find ("Points_AvailT").GetComponent<Text> ();
		PointsDex = GameObject.Find ("Points_Dex").GetComponent<Text> ();
		PointsStr = GameObject.Find ("Points_Str").GetComponent<Text> ();
		PointsConst = GameObject.Find ("Points_Const").GetComponent<Text> ();
		PointsIntel = GameObject.Find ("Points_Intel").GetComponent<Text> ();

		o1 = GameObject.Find ("Orb1").GetComponent<Button> ();
		o2 = GameObject.Find ("Orb2").GetComponent<Button> ();
		o3 = GameObject.Find ("Orb3").GetComponent<Button> ();
		o4 = GameObject.Find ("Orb4").GetComponent<Button> ();
		o5 = GameObject.Find ("Orb5").GetComponent<Button> ();
		o6 = GameObject.Find ("Orb6").GetComponent<Button> ();
		o7 = GameObject.Find ("Orb7").GetComponent<Button> ();
		o8 = GameObject.Find ("Orb8").GetComponent<Button> ();
		o9 = GameObject.Find ("Orb9").GetComponent<Button> ();
		o10 = GameObject.Find ("Orb10").GetComponent<Button> ();
		StatsManager ();

		UpdateOrbs ();
	
		//	Cursor.visible = true;
	}

	void StatsManager(){
		while (CurrentLevelNum > Initial) {
			StatPoints += 2;
			TreePoints++;
			Initial++;
		}

		if (StatPoints > 0) {
			BtnAddConst.enabled = true;	
			BtnAddDex.enabled = true;	
			BtnAddIntel.enabled = true;	
			BtnAddStr.enabled = true;	
		} else {
			BtnAddConst.enabled= false;	
			BtnAddDex.enabled = false;	
			BtnAddIntel.enabled = false;	
			BtnAddStr.enabled = false;
		}


		PointsAvail.text = StatPoints.ToString ();
		PointsAvailT.text = TreePoints.ToString ();
		PointsIntel.text = PointsIntelNum.ToString ();
		PointsDex.text = PointsDexNum.ToString ();
		PointsStr.text = PointsStrNum.ToString ();
		PointsConst.text = PointsConstNum.ToString ();
	}

	private void UpdateOrbs(){
		if (bo1 == 1) {
			o1.enabled = false;
			setInteraction (o3);
			//o3.interactable = true;
		}
		if (bo2 == 1) {
			o2.enabled = false;
			setInteraction (o4);
			//o4.interactable = true;
		}
		if (bo3 == 1) {
			o3.enabled = false;
			setInteraction (o5);
			//o5.interactable = true;
		}
		if (bo4 == 1) {
			o4.enabled = false;
			setInteraction (o7);
			//o7.interactable = true;
			if (!o3.enabled) {
				setInteraction (o6);
				//o6.interactable = true;
			}
		}
		if (bo5 == 1) {
			o5.enabled = false;
			//o8.interactable = true;
			setInteraction (o8);
		}
		if (bo6 == 1) {
			o6.enabled = false;
			//o9.interactable = true;
			setInteraction (o9);
		}
		if (bo7 == 1) {
			o7.enabled = false;
			//o10.interactable = true;
			setInteraction (o10);
		}
		if (bo8 == 1 ) {
			o8.enabled = false;

		}
		if (bo9 == 1) {
			o9.enabled = false;

		}
		if (bo10 == 1) {
			o10.enabled = false;
		}
	}

	void DecreaseAvailPoints(){
		if (StatPoints > 0) {
			StatPoints--;
		}
		StatsManager ();
	}

	void DecreaseAvailPointsT(){
		if (TreePoints > 0) {
			TreePoints--;
		}
		StatsManager ();
	}

	public void AddStatPoints(){

			if (EventSystem.current.currentSelectedGameObject.name == "Btn_AddIntel" ) {
				PointsIntelNum++;
				StatsManager ();
				DecreaseAvailPoints ();
				PlayerManager.AlterIntelligence(1);
			    setSelectedButton ();
			} else if (EventSystem.current.currentSelectedGameObject.name == "Btn_AddDex") {
				PointsDexNum++;
				StatsManager ();
				DecreaseAvailPoints ();
				PlayerManager.AlterDexterity(1);
			    setSelectedButton ();
			} else if (EventSystem.current.currentSelectedGameObject.name == "Btn_AddConst") {
				PointsConstNum++;
				StatsManager ();
				DecreaseAvailPoints ();
				PlayerManager.AlterConstitution(1);
			    setSelectedButton ();
			} else if (EventSystem.current.currentSelectedGameObject.name == "Btn_AddStr") {
				PointsStrNum++;
				StatsManager ();
				DecreaseAvailPoints ();
				PlayerManager.AlterStrength(1);
			    setSelectedButton ();
		}
	}
	public void AddTreePoints(){
		if (TreePoints > 0) {
			if (EventSystem.current.currentSelectedGameObject.name == "Orb1") {
				o1.enabled = false;
				//o3.interactable = true;
				bo1 = 1;
				DecreaseAvailPointsT ();
				setInteraction (o3);
				PlayerManager.AlterHealth (20);
			} else if (EventSystem.current.currentSelectedGameObject.name == "Orb2") {
				o2.enabled = false;
				//o4.interactable = true;
				bo2 = 1;
				DecreaseAvailPointsT ();
				setInteraction (o4);
				PlayerManager.AlterMana (10);

			} else if (EventSystem.current.currentSelectedGameObject.name == "Orb3") {
				o3.enabled = false;
				//o5.interactable = true;
				bo3 = 1;
				DecreaseAvailPointsT ();
			     if (!o4.enabled) {
					setInteraction (o6);
					//o6.interactable = true;
				}
				setInteraction (o5);
				PlayerManager.AlterStrength (2);
				PointsStrNum += 2;
				StatsManager ();

			} else if (EventSystem.current.currentSelectedGameObject.name == "Orb4") {
				o4.enabled = false;
				//o7.interactable = true;
				DecreaseAvailPointsT ();
				setInteraction (o7);
				bo4 = 1;
				if (!o3.enabled) {
					//o6.interactable = true;
					setInteraction (o6);
				}

				PlayerManager.AlterIntelligence (2);
				PointsIntelNum += 2;
				StatsManager ();

			} else if (EventSystem.current.currentSelectedGameObject.name == "Orb5") {
				o5.enabled = false;
				DecreaseAvailPointsT ();
				setInteraction (o8);
				//o8.interactable = true;
				bo5 = 1;
				PlayerManager.AlterHealth (30);
				PlayerManager.AlterStrength (3);
				PlayerManager.AlterDexterity (2);
				PointsStrNum += 3;
				PointsDexNum += 2;
				StatsManager ();

			} else if (EventSystem.current.currentSelectedGameObject.name == "Orb6") {
				o6.enabled = false;
				DecreaseAvailPointsT ();
				setInteraction (o9);
				//o9.interactable = true;
				bo6 = 1;
				PlayerManager.AlterHealth (20);
				PlayerManager.AlterStrength (2);
				PlayerManager.AlterDexterity (2);

				PointsStrNum += 2;
				PointsDexNum += 2;
				StatsManager ();
			} else if (EventSystem.current.currentSelectedGameObject.name == "Orb7") {
				o7.enabled = false;
				DecreaseAvailPointsT ();
				setInteraction (o10);
				//o10.interactable = true;
				bo7 = 1;
				PlayerManager.AlterMana (30);
				PlayerManager.AlterIntelligence (3);
				PointsIntelNum += 3;
				StatsManager ();
			} else if (EventSystem.current.currentSelectedGameObject.name == "Orb8") {
				o8.enabled = false;
				DecreaseAvailPointsT ();
				bo8 = 1;
				PlayerManager.AlterConstitution (3);
				PlayerManager.AlterStrength(2);
				PointsStrNum += 2;
				PointsConstNum += 3;
				StatsManager ();
			} else if (EventSystem.current.currentSelectedGameObject.name == "Orb9") {
				o9.enabled = false;
				DecreaseAvailPointsT ();
				bo9 = 1;
				PlayerManager.AlterIntelligence (3);
				PlayerManager.AlterStrength (2);
				PlayerManager.AlterDexterity (2);

				PointsStrNum += 2;
				PointsDexNum += 2;
				PointsIntelNum += 3;
				StatsManager ();
			} else if (EventSystem.current.currentSelectedGameObject.name == "Orb10") {
				o10.enabled = false;
				DecreaseAvailPointsT ();
				bo10 = 1;
				PlayerManager.AlterIntelligence (2);
				PlayerManager.AlterMana (20);
				PointsIntelNum += 2;
				StatsManager ();
			} 
		}
	}

	private void setSelectedButton(){
		UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject (null);
	}

	public void setToolTip(string t){
		
		ToolTipUI.SetActive (true);
		ToolTipText = GameObject.Find ("ToolTipText").GetComponent<Text> ();
		ToolTipText.text = t;
	}

	private void setInteraction(Button b){
		if (TreePoints <= 0) {
			b.interactable = false;
		} else {
			b.interactable = true;
		}
	}

	public static int Dexterity
	{
		get{return instance.PointsDexNum; }
		set{
			instance.PointsDexNum = value;
		}
	}

	public static int Strength
	{
		get{return instance.PointsStrNum; }
		set{
			instance.PointsStrNum = value;
		}
	}
	public static int Constitution
	{
		get{return instance.PointsConstNum; }
		set{
			instance.PointsConstNum = value;
		}
	}
	public static int Intelligence
	{
		get{return instance.PointsIntelNum; }
		set{
			instance.PointsIntelNum = value;
		}
	}
	public static int AvailablePoints
	{
		get{return instance.StatPoints; }
		set{
			instance.StatPoints = value;
			instance.Initial = PlayerManager.Level;

		}
	}

	public static int AvailablePointsT
	{
		get{return instance.TreePoints; }
		set{
			instance.TreePoints = value;
		}
	}

	public static int Orb1
	{
		get{return instance.bo1; }
		set{
			instance.bo1 = value;
			Debug.Log (instance.bo1);
		}
	}

	public static int Orb2
	{
		get{return instance.bo2; }
		set{
			instance.bo2 = value;

		}
	}
	public static int Orb3
	{
		get{return instance.bo3; }
		set{
			instance.bo3 = value;

		}
	}
	public static int Orb4
	{
		get{return instance.bo4; }
		set{
			instance.bo4 = value;

		}
	}
	public static int Orb5
	{
		get{return instance.bo5; }
		set{
			instance.bo5 = value;

		}
	}
	public static int Orb6
	{
		get{return instance.bo6; }
		set{
			instance.bo6 = value;

		}
	}
	public static int Orb7
	{
		get{return instance.bo7; }
		set{
			instance.bo7 = value;

		}
	}
	public static int Orb8
	{
		get{return instance.bo8; }
		set{
			instance.bo8 = value;

		}
	}
	public static int Orb9
	{
		get{return instance.bo9; }
		set{
			instance.bo9 = value;

		}
	}
	public static int Orb10
	{
		get{return instance.bo10; }
		set{
			instance.bo10 = value;

		}
	}
		
}
