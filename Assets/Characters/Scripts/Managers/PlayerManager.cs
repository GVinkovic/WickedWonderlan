using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public int ManaRegenerationAmount = 1;
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
        instance = this;

        if (!player) Character = PlayerCharacter.Man;
        else ReferencePlayerScripts();

        cameraScript = playerCamera.GetComponent<ThirdPersonOrbitCamBasic>();

        playerInventory = GetComponent<PlayerInventory>();

        Inventory.AllInventoriesClosed += InventoriesClosed;
        Inventory.InventoryOpen += InventoryOpened;
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
        set { instance.ExperiencePoints = value; }
    }

    public static PlayerCharacter Character
    {
        get { return playerCharacter; }
        set {
            if (instance.player) instance.player.SetActive(false);
            playerCharacter = value;
            foreach(var character in instance.players)
            {
                if(value == character.character)
                {
                    instance.player = character.player;
                    instance.player.SetActive(true);
                    instance.ReferencePlayerScripts();
                    break;
                }
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
        while (instance.playerStats.CurrentMana < 100)
        {
            AlterMana(instance.ManaRegenerationAmount);
            yield return new WaitForSeconds(instance.ManaRegenerationTime);
        }
        isManaRecovering = false;
    }

    void InventoriesClosed()
    {
        Cursor.visible = false;
        cameraScript.enabled = true;
        moveBehaviour.enabled = true;
        basicBehaviour.enabled = true;
    }
    void InventoryOpened()
    {
        if (!Cursor.visible)
        {
            Cursor.visible = true;
            cameraScript.enabled = false;
            moveBehaviour.enabled = false;
            basicBehaviour.enabled = false;
        }

    }

    public static void TakeHit()
    {
        instance.healthBar.SetProgress(instance.playerStats.CurrentHealth);
    }
    public static void Die()
    {
        instance.healthBar.SetProgress(0);

    }

    public static void ConsumeMana()
    {//TODO: uzet modifier od nekud ovisno od tipu magic-a
        AlterMana(-1);
        RecoverMana();
    }
    static void AlterMana(int amount)
    {
        instance.playerStats.CurrentMana += amount;
        instance.manaBar.SetProgress(instance.playerStats.CurrentMana);
    }
    public static void GainExperience(int amount)
    {
        int MaxExperiencePoints = instance.ExperienceLevels[instance.ExperienceLevels.Length - 1];
       
        instance.ExperiencePoints += amount;

        int currentLevel = Array.BinarySearch(instance.ExperienceLevels, instance.ExperiencePoints);
        if (currentLevel < 0) currentLevel = (currentLevel + 1) * -1;
       
        if (instance.ExperiencePoints >= MaxExperiencePoints) instance.ExperiencePoints = MaxExperiencePoints;
        currentLevel--;
        
        if(currentLevel != level)
        {
            level = currentLevel;
            LevelUp();
        }

        instance.experienceBar.SetProgress(((float)instance.ExperiencePoints / (float)MaxExperiencePoints) *100, 
            instance.ExperiencePoints + "/" + MaxExperiencePoints, currentLevel);
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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S)) { 
            SaveGame.Save("save1");
        }
        else if(Input.GetKeyDown(KeyCode.L))
        {
            SaveGame.Load("save1");
        }
    }
}
