using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveGame : MonoBehaviour {

    private static readonly string SaveFile = "save.bin";

    private static List<string> saveGameNamesList;



    static readonly string SaveGameNames = "saveGameNames";
    static readonly char delimiter = ',';
    static readonly char itemValueDelimiter = '-';
    static readonly string time = "time";


    static readonly string character = "ch";

    static readonly string position = "pos";
    static readonly string rotation = "rot";
    static readonly string experience = "xp";
    static readonly string health = "hp";
    static readonly string mana = "mana";
    static readonly string strength = "str";
    static readonly string constitution = "const";
    static readonly string intelligence = "intel";
    static readonly string dexterity = "dex";
    static readonly string playerType = "plType";
    static readonly string attackType = "atckType";
    static readonly string sword = "swd";

    static readonly string mainInventoryItems = "mii";
    static readonly string characterSystemInventoryItems = "csii";
    static readonly string hotbarInventoryItems = "hubii";

    static void EnsureInit()
    {
        SaveSystem.Initialize(SaveFile);
    }
   

    public static void Save(string profileName)
    {
        if (!GetSaveGameNames().Contains(profileName))
        {
            AddSaveGameName(profileName);
        }
        SaveSystem.SetInt(profileName + character, (int)PlayerManager.Character);

        var player = PlayerManager.Player;
        SaveSystem.SetVector3(profileName + position, player.transform.position);
        SaveSystem.SetVector3(profileName + rotation, player.transform.rotation.eulerAngles);
        SaveSystem.SetInt(profileName + experience, PlayerManager.Experience);
        
        PlayerStats ps = PlayerManager.PlayerStats;
        SaveSystem.SetInt(profileName + health, ps.CurrentHealth);
        SaveSystem.SetInt(profileName + mana, ps.CurrentMana);
        SaveSystem.SetInt(profileName + strength, ps.strength.Value);
        SaveSystem.SetInt(profileName + constitution, ps.constitution.Value);
        SaveSystem.SetInt(profileName + intelligence, ps.intelligence.Value);
        SaveSystem.SetInt(profileName + dexterity, ps.dexterity.Value);

        SaveSystem.SetInt(profileName + playerType, (int)PlayerManager.PlayerScript.Type);
        SaveSystem.SetInt(profileName + attackType, PlayerManager.PlayerScript.AttackIndex);
        SaveSystem.SetInt(profileName + sword, PlayerManager.PlayerScript.defaultSwordIndex);

        SaveInventoryItems(PlayerManager.PlayerInventory.MainInventory, mainInventoryItems);
        SaveInventoryItems(PlayerManager.PlayerInventory.CharacterSystemInventory, characterSystemInventoryItems);
        SaveInventoryItems(PlayerManager.PlayerInventory.HotbarInventory, hotbarInventoryItems);

        SaveSystem.SetLong(profileName + time, DateTime.Now.ToBinary());
        SaveSystem.SaveToDisk();
    }

    static void SaveInventoryItems(Inventory inventory, string key)
    {
        string items = "";
        string d = itemValueDelimiter + "";

        foreach (var item in inventory.getItemList())
        {
            items += item.itemID + d + item.itemValue + d + inventory.getPositionOfItem(item)+delimiter;
        }
        if(!string.IsNullOrEmpty(items))items = items.Remove(items.Length - 1);

        SaveSystem.SetString(key, items);
    }

    public static void Load(string profileName)
    {
        if (!GetSaveGameNames().Contains(profileName))
        {
            print("Save profil " + profileName + " ne postoji");
            return;
        }

        PlayerManager.Character = (PlayerManager.PlayerCharacter) SaveSystem.GetInt(profileName + character);

        var player = PlayerManager.Player;

        player.transform.position = SaveSystem.GetVector3(profileName + position);
        player.transform.rotation = Quaternion.Euler(SaveSystem.GetVector3(profileName + rotation));

        PlayerManager.Experience = SaveSystem.GetInt(profileName + experience);

        PlayerStats ps = PlayerManager.PlayerStats;
        ps.CurrentHealth = SaveSystem.GetInt(profileName + health);
        ps.CurrentMana = SaveSystem.GetInt(profileName + mana);
        ps.strength.Value = SaveSystem.GetInt(profileName + strength);
        ps.constitution.Value = SaveSystem.GetInt(profileName + constitution);
        ps.intelligence.Value = SaveSystem.GetInt(profileName + intelligence);
        ps.dexterity.Value = SaveSystem.GetInt(profileName + dexterity);

        PlayerManager.PlayerScript.Type = (Player.PlayerType)SaveSystem.GetInt(profileName + playerType);
        PlayerManager.PlayerScript.AttackIndex = SaveSystem.GetInt(profileName + attackType);
        PlayerManager.PlayerScript.defaultSwordIndex = SaveSystem.GetInt(profileName + sword);

        LoadInventoryItems(PlayerManager.PlayerInventory.MainInventory, mainInventoryItems);
        LoadInventoryItems(PlayerManager.PlayerInventory.CharacterSystemInventory, characterSystemInventoryItems);
        LoadInventoryItems(PlayerManager.PlayerInventory.HotbarInventory, hotbarInventoryItems);

        ConnectInventoryItems(PlayerManager.PlayerInventory.CharacterSystemInventory, PlayerManager.PlayerInventory.HotbarInventory);

    }

    static void LoadInventoryItems(Inventory inventory, string key)
    {
        GameManager.instance.StartCoroutine(DeleteAndAddInventoryItems(inventory, key));

    }
    static IEnumerator DeleteAndAddInventoryItems(Inventory inventory, string key)
    {
        inventory.deleteAllItems();
        inventory.updateItemList();

        yield return new WaitForFixedUpdate();

        string items = SaveSystem.GetString(key);

        if (!string.IsNullOrEmpty(items))
        {
            foreach (var itemEntry in items.Split(delimiter))
            {
                var itemValues = itemEntry.Split(itemValueDelimiter);

                var id = (int.Parse(itemValues[0]));
                var value = int.Parse(itemValues[1]);
                int position = int.Parse(itemValues[2]);

                inventory.addItemToPosition(id, value, position);
            }

            inventory.updateItemList();
            inventory.updateIconSize();
            inventory.stackableSettings();
        }
    }

    static void ConnectInventoryItems(Inventory inv1, Inventory inv2)
    {
        inv1.gameObject.SetActive(true);
        inv2.gameObject.SetActive(true);
        GameManager.instance.StartCoroutine(ConnectInventoryItemsRoutine(inv1, inv2));
    }
    static IEnumerator ConnectInventoryItemsRoutine(Inventory inv1, Inventory inv2)
    {
        yield return new WaitForFixedUpdate();

        yield return new WaitForFixedUpdate();

        var itemsOnObject1 = inv1.gameObject.GetComponentsInChildren<ConsumeItem>();
        var itemsOnObject2 = inv2.gameObject.GetComponentsInChildren<ConsumeItem>();
        

        if (itemsOnObject1 != null)
        {
            foreach (var itemOnObject1 in itemsOnObject1)
            {
                foreach (var itemOnObject2 in itemsOnObject2)
                {

                    if (itemOnObject1.item.itemID == itemOnObject2.item.itemID)
                    {
                        itemOnObject1.duplication = itemOnObject2.gameObject;
                        itemOnObject2.duplication = itemOnObject1.gameObject;
                        break;
                    }
                }
            }
        }
        inv1.gameObject.SetActive(false);
     //   inv2.gameObject.SetActive(false);
    }
    public static List<string> GetSaveGameNames()
    {
        if(saveGameNamesList == null)
        {
            EnsureInit();
            var val = SaveSystem.GetString(SaveGameNames);
            if (string.IsNullOrEmpty(val)) saveGameNamesList = new List<string>();
            saveGameNamesList = new List<string>(val.Split(delimiter));
        }
        return saveGameNamesList;
    } 
    public static DateTime getSaveTime(string profileName)
    {
       var timeBinary =  SaveSystem.GetLong(profileName + time);
       return DateTime.FromBinary(timeBinary);
    }
    private static void AddSaveGameName(string name)
    {
        GetSaveGameNames();
        saveGameNamesList.Add(name);
        string names = "";
        saveGameNamesList.ForEach((n) => names += n + delimiter);

        if (!string.IsNullOrEmpty(names)) names = names.Remove(names.Length - 1);


        SaveSystem.SetString(SaveGameNames,names);
        SaveSystem.SaveToDisk();

    }

}
