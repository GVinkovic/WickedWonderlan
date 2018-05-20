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

    //public int ManaRegenerationAmount = 1;
    public float ManaRegenerationTime = 1;

    public int[] ExperienceLevels;

    private int ExperiencePoints = 0;

    private static int level = 0;


    private Player playerScript;
    private MoveBehaviour moveBehaviour;
    private BasicBehaviour basicBehaviour;
    private PlayerStats playerStats;
    private GameObject player;


    private PlayerInventory playerInventory;

    private ThirdPersonOrbitCamBasic cameraScript;



    private static bool isManaRecovering = false;

    private static PlayerCharacter playerCharacter ;

    #region Singleton
    public static PlayerManager instance;

    void Awake()
    {
		openedWindows = 0;
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
                Level = currentLevel;
                LevelUp();
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
    static IEnumerator ManaRecovery()
    {
        while (instance.playerStats.CurrentMana < instance.playerStats.MaxMana)
        {
            AlterMana(PlayerStats.intelligence.Value);
            yield return new WaitForSeconds(instance.ManaRegenerationTime);
        }
        isManaRecovering = false;
    }

    void InventoriesClosed()
    {
        if (openedWindows > 0) return;

        Cursor.visible = false;
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

        //provjerava dali su svi inventory zatvoreni
        // ako jesu invoka event, koji onda poziva metodu InventoriesClosed();
        Inventory.checkIfAllInventoryClosed();
    }
    public static void TakeHit()
    {
        RefreshHealthUI();
    }
    public static void Die()
    {
        instance.healthBar.SetProgress(0);
    }

    public static void ConsumeMana()
    {
        AlterMana(PlayerStats.intelligence.Value * 5);
        RecoverMana();
    }
    public static void AlterMana(int amount)
    {
        instance.playerStats.CurrentMana += amount;
        RefreshManaUI();
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

    static void LevelUp()
    {
        //TODO: animacija level-upanja i pozivanja rapoređivanja onih bodova
        print("Level up: " + level);
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
    }

    public static void AlterStrength(int amount)
    {
        PlayerStats.AlterStrength(amount);
    }

}
