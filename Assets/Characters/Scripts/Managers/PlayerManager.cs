using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour {

	public enum PlayerCharacter
	{
		Man = 0,
		Woman = 1
	}


	[System.Serializable]
	public class PlayerAttackSpeed
	{
		public Player.PlayerType playerType;
		public float AttackSpeed;
	}

	[System.Serializable]
	public class CharacterGameObject
	{
		public PlayerCharacter character;
		public GameObject player;
	}


	public PlayerAttackSpeed[] playerAttackSpeed;

	public CharacterGameObject[] players;

	public GameObject playerCamera;
	public ProgressBar healthBar;
	public ProgressBar manaBar;
	public ProgressBar experienceBar;
	public Text playerLevelText;

	
    public float ManaRegenerationTime = 1;
    public float HealthRegenerationTime = 1;

    public int[] ExperienceLevels;

	private int ExperiencePoints = 0;

	private static int level = 0;


	private Player playerScript;
	private MoveBehaviour moveBehaviour;
	private BasicBehaviour basicBehaviour;
	private PlayerStats playerStats;
	private GameObject player;


	private PlayerInventory playerInventory;
	private GameObject gameOverMenu;

	private ThirdPersonOrbitCamBasic cameraScript;

    private RhinoController rhino;

    private List<Enemy> enemyAttacking = new List<Enemy>();



	private static bool isManaRecovering = false;
    private static bool isHealthRecovering = false;

    private static PlayerCharacter playerCharacter ;

	#region Singleton
	public static PlayerManager instance;

	void Awake()
	{
		openedWindows = 0;
        level = 0;
		instance = this;
		int playerIndex =  PlayerPrefs.GetInt("CharacterSelected", 0);

		Character = (PlayerCharacter) playerIndex;

		cameraScript = playerCamera.GetComponent<ThirdPersonOrbitCamBasic>();
		playerInventory = GetComponent<PlayerInventory>();

		Inventory.AllInventoriesClosed += InventoriesClosed;
		Inventory.InventoryOpen += InventoryOpened;
	}
	void OnDestroy()
	{
		Inventory.AllInventoriesClosed -= InventoriesClosed;
		Inventory.InventoryOpen -= InventoryOpened;
	}
	void ReferencePlayerScripts()
	{
		playerScript = player.GetComponent<Player>();
		moveBehaviour = player.GetComponent<MoveBehaviour>();
		basicBehaviour = player.GetComponent<BasicBehaviour>();
		playerStats = player.GetComponent<PlayerStats>();
	}
	#endregion
	public static GameObject Player
	{
		get { return instance.player; }
	}
	public static GameObject PlayerCamera
	{
		get { return instance.playerCamera; }
	}
	public static Player PlayerScript
	{
		get { return instance.playerScript; }
	}
	public static PlayerStats PlayerStats
	{
		get { return instance.playerStats; }
	}
	public static int Experience
	{
		get { return instance.ExperiencePoints; }
		set {

			instance.ExperiencePoints = value;

			int MaxExperiencePoints = instance.ExperienceLevels[instance.ExperienceLevels.Length - 1];
			instance.experienceBar.SetProgress(((float)instance.ExperiencePoints / (float)MaxExperiencePoints) *100, 
				instance.ExperiencePoints + "/" + MaxExperiencePoints, Level);


			int currentLevel = Array.BinarySearch(instance.ExperienceLevels, value);
			if (currentLevel < 0) currentLevel = (currentLevel + 1) * -1;

			// currentLevel--;

			if (currentLevel != level)
			{
                var prevLevel = Level;
                Level = currentLevel;
                LevelUp(prevLevel);
            }
		}
	}

	public static int Level
	{
		get { return level;}
		set {

			level = value;
			instance.playerLevelText.text = value.ToString();
		}
	}

	public static PlayerCharacter Character
	{
		get { return playerCharacter; }
		set {
			// if (instance.player) instance.player.SetActive(false);
			playerCharacter = value;
			foreach(var character in instance.players)
			{
				if (value == character.character)
				{
					instance.player = character.player;
					instance.player.SetActive(true);
					instance.ReferencePlayerScripts();

				}
				else character.player.SetActive(false);
			}

		}
	}

	public static PlayerInventory PlayerInventory
	{
		get { return instance.playerInventory; }
	}

	static void RecoverMana()
	{
		if(!isManaRecovering)
		{
			instance.StartCoroutine(ManaRecovery());
			isManaRecovering = true;
		}
	}

   static void RecoverHealth()
    {
        if (!isHealthRecovering)
        {
            instance.StartCoroutine(HealthRecovery());
            isHealthRecovering = true;
        }
    }


	static IEnumerator ManaRecovery()
	{
		while (instance.playerStats.CurrentMana < instance.playerStats.MaxMana)
		{
			AlterMana(PlayerStats.intelligence.Value, false);
			yield return new WaitForSeconds(instance.ManaRegenerationTime);
		}
		isManaRecovering = false;
	}

    static IEnumerator HealthRecovery()
    {
        while (instance.playerStats.CurrentHealth<instance.playerStats.MaxHealth)
        {
            AlterHealth(PlayerStats.constitution.Value,false);
            yield return new WaitForSeconds(instance.HealthRegenerationTime);
        }
        isHealthRecovering = false;
    }

	public static void StopPlayerMovement()
	{
		instance.moveBehaviour.enabled = false;
		instance.basicBehaviour.enabled = false;
	}
	public static void ResumePlayerMovement()
	{
		instance.moveBehaviour.enabled = true;
		instance.basicBehaviour.enabled = true;
	}

	void InventoriesClosed()
	{
		if (openedWindows > 0) return;

		Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
		cameraScript.enabled = true;
		moveBehaviour.enabled = true;
		basicBehaviour.enabled = true;
		Time.timeScale = 1f;
	}
	void InventoryOpened()
	{
		if (!Cursor.visible)
		{
			Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            cameraScript.enabled = false;
			moveBehaviour.enabled = false;
			basicBehaviour.enabled = false;
			Time.timeScale = 0f;
		}

	}
	private static int openedWindows = 0;
	public void WindowOpened()
	{
		openedWindows++;
		InventoryOpened();
	}

	public void WindowClosed()
	{
		openedWindows--;
        // ako ima još otvorenih prozora ne radi nista
        if (openedWindows > 0) return;

		//provjerava dali su svi inventory zatvoreni
		// ako jesu invoka event, koji onda poziva metodu InventoriesClosed();
		Inventory.checkIfAllInventoryClosed();
	}


    public static bool CollectItem(int itemId, int quantity)
    {
        var item = GameManager.ItemDatabase.getItemByID(itemId);
        var inventory = PlayerInventory.MainInventory;
        bool check = inventory.checkIfItemAllreadyExist(item.itemID, quantity);

        if (check)
        {
            GameManager.NotifyPickedUpItem(item);
            return true;
        }
        else if (inventory.ItemsInInventory.Count < (inventory.width * inventory.height))
        {
            inventory.addItemToInventory(item.itemID, quantity);
            inventory.updateIconSize();
            inventory.updateItemList();
            inventory.stackableSettings();
            GameManager.NotifyPickedUpItem(item);
            return true;
        }
        return false;

    }


    public static void TakeHit()
	{
		RefreshHealthUI();
        RecoverHealth();
	}
	public static void Die()
	{
		instance.healthBar.SetProgress(0);
        StopPlayerMovement();
        PlayerScript.Die();
        instance.Invoke("ShowGameOver", 5);
	}
    private void ShowGameOver()
    {
		gameOverMenu = GameObject.Find ("GameOverMenu");
		GameOverMenuScript govms = gameOverMenu.GetComponent<GameOverMenuScript> ();
		govms.loadMenu();

        Hide();
        GameManager.Hide();
        QuestManager.Hide();

    }
    public static void Hide()
    {
        PlayerInventory.HideAll();

        var pauseMenu = FindObjectOfType<PauseMenuScript>();
        if (pauseMenu) pauseMenu.enabled = false;
      
    }

   

	public static void ConsumeMana()
	{
		AlterMana(PlayerStats.intelligence.Value * 5, false);
		RecoverMana();
	}
    public static void AlterMana(int amount, bool increaseMax = true)
    {
        //povećaj i max manu ako se povećava i maximum
        if (increaseMax) PlayerStats.MaxMana += amount;

        instance.playerStats.CurrentMana = Mathf.Clamp(PlayerStats.CurrentMana + amount, 0, PlayerStats.MaxMana);
        RefreshManaUI();
    }

    public static void AlterHealth(int amount, bool increaseMax = true)
    {
        // ako se povećava i maximum povećaj maxhealth variablu
        if (increaseMax) PlayerStats.MaxHealth += amount;

        instance.playerStats.CurrentHealth = Mathf.Clamp(PlayerStats.CurrentHealth + amount, 0, PlayerStats.MaxHealth);
        RefreshHealthUI();
    }



	// osvjezava sve statove prikazane na ekranu
	public static void RefreshStats()
	{
		RefreshHealthUI();
		RefreshManaUI();

	}

	public static void RefreshHealthUI()
	{
		instance.healthBar.SetProgress((float)instance.playerStats.CurrentHealth / (float)instance.playerStats.MaxHealth * 100,
			instance.playerStats.CurrentHealth + "/" + instance.playerStats.MaxHealth);
	}

	public static void RefreshManaUI()
	{
		instance.manaBar.SetProgress((float)instance.playerStats.CurrentMana / (float)instance.playerStats.MaxMana * 100,
			instance.playerStats.CurrentMana + "/" + instance.playerStats.MaxMana);
	}

	public static void GainExperience(int amount)
	{
		int MaxExperiencePoints = instance.ExperienceLevels[instance.ExperienceLevels.Length - 1];

		int xp = Experience + amount;

		if (xp >= MaxExperiencePoints) xp = MaxExperiencePoints;


		Experience = xp;


	}

	static void LevelUp(int prevLevel)
	{
		//TODO: animacija level-upanja i pozivanja rapoređivanja onih bodova
		print("Level up: " + level);
        if (prevLevel == 0)
        {
            //nakon prvog level up-a prikazi upute za rasporeÄ‘ivanje dobivenih bodova
            GameManager.GetDialogMgr.BeginDialog(GameManager.GetDialogsCollection.getDialogList("TutorialOnLevelUp"));
        }

    }

	public static float GetAttackSpeed(Player.PlayerType playerType)
	{
		foreach(var playerAttackSpeed in instance.playerAttackSpeed)
		{
			if (playerAttackSpeed.playerType == playerType) return playerAttackSpeed.AttackSpeed;
		}
		return 1;
	}

	public static void AlterIntelligence(int amount)
	{
		PlayerStats.AlterIntelligence(amount);
		// osvježi ui prikaza mane
		RefreshManaUI();
        // povecaj manu do maximuma
        RecoverMana();
    }


	public static void AlterDexterity(int amount)
	{
		PlayerStats.AlterDexterity(amount);
	}

	public static void AlterConstitution(int amount)
	{
		PlayerStats.AlterConstitution(amount);
		//osvježi ui prikaza health-a
		RefreshHealthUI();
        // povecaj health do maximuma
        RecoverHealth();
    }

	public static void AlterStrength(int amount)
	{
		PlayerStats.AlterStrength(amount);
	}

    public static void RegisterRhino(RhinoController rhino)
    {
        instance.rhino = rhino;
    }
    public static Vector3 RhinoPosition
    {
        get
        {
            return instance.rhino ? instance.rhino.transform.position : Vector3.zero;
        }
        set
        {
            if(value != Vector3.zero)
            {
                if (instance.rhino)
                {
                    instance.rhino.SetPosition(value);
                }
            }
        }
    }

    public static void UnderEnemyAttack(Enemy enemy)
    {
        if (instance.enemyAttacking.Contains(enemy)) return;

        instance.enemyAttacking.Add(enemy);

        RhinoAttackEnemy();

    }

    public static void EnemyStoppedAttack(Enemy enemy)
    {
        instance.enemyAttacking.Remove(enemy);

        RhinoAttackEnemy();
       
    }

    static void RhinoAttackEnemy()
    {

        if (!instance.rhino) return;


        if (!instance.rhino.Attacks && instance.enemyAttacking.Count > 1)
        {
            foreach(var enemy in UnderAttackFromEnemys)
            {
                if(enemy.enemyType != Enemy.Type.Dragon)
                {
                    instance.rhino.SetTarget(enemy.gameObject, true);
                    enemy.SetTarget(instance.rhino.gameObject, false);
                    return;
                }
            }
        }
        else if(instance.enemyAttacking.Count == 0)
        {
            instance.rhino.SetTarget(Player, false);
        }

    }

    public static List<Enemy> UnderAttackFromEnemys
    {
        get
        {
            return instance.enemyAttacking;
        }
    }

}
