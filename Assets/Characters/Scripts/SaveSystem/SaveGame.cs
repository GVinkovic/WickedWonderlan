﻿using System;
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
    static readonly string maxHealth = "maxhp";
    static readonly string mana = "mana";
    static readonly string maxMana = "maxmana";
    static readonly string strength = "str";
    static readonly string constitution = "const";
    static readonly string intelligence = "intel";
    static readonly string dexterity = "dex";
    static readonly string playerType = "plType";
    static readonly string attackType = "atckType";
    static readonly string sword = "swd";
	static readonly string statDex = "sd"; //dodano
	static readonly string statStr = "ss"; //dodano
	static readonly string statConst = "sc"; //dodano
	static readonly string statIntel = "si"; //dodano
	static readonly string statAvail = "sa"; //dodano
	static readonly string treeAvail = "ta";//dodano
	static readonly string orb1 = "o1"; //dodano
	static readonly string orb2 = "o2"; //dodano
	static readonly string orb3 = "o3"; //dodano
	static readonly string orb4 = "o4"; //dodano
	static readonly string orb5 = "o5"; //dodano
	static readonly string orb6 = "o6"; //dodano
	static readonly string orb7 = "o7"; //dodano
	static readonly string orb8 = "o8"; //dodano
	static readonly string orb9 = "o9"; //dodano
	static readonly string orb10 = "10"; //dodano





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
        SaveSystem.SetInt(profileName + maxHealth, ps.MaxHealth);
        SaveSystem.SetInt(profileName + mana, ps.CurrentMana);
        SaveSystem.SetInt(profileName + maxMana, ps.MaxMana);
        SaveSystem.SetInt(profileName + strength, ps.strength.Value);
        SaveSystem.SetInt(profileName + constitution, ps.constitution.Value);
        SaveSystem.SetInt(profileName + intelligence, ps.intelligence.Value);
        SaveSystem.SetInt(profileName + dexterity, ps.dexterity.Value);



        SaveSystem.SetInt(profileName + playerType, (int)PlayerManager.PlayerScript.Type);
        SaveSystem.SetInt(profileName + attackType, PlayerManager.PlayerScript.AttackIndex);
        SaveSystem.SetInt(profileName + sword, PlayerManager.PlayerScript.defaultSwordIndex);
		SaveSystem.SetInt (profileName + statDex, PassiveTreeScript.Dexterity);//dodano 
		SaveSystem.SetInt (profileName + statStr, PassiveTreeScript.Strength);//dodano 
		SaveSystem.SetInt (profileName + statConst, PassiveTreeScript.Constitution);//dodano 
		SaveSystem.SetInt (profileName + statIntel, PassiveTreeScript.Intelligence);//dodano 
		SaveSystem.SetInt (profileName + statAvail, PassiveTreeScript.AvailablePoints);//dodano 
		SaveSystem.SetInt (profileName + treeAvail, PassiveTreeScript.AvailablePointsT);//dodano
		SaveSystem.SetInt (profileName + orb1, PassiveTreeScript.Orb1);//dodano
		SaveSystem.SetInt (profileName + orb2, PassiveTreeScript.Orb2);//dodano
		SaveSystem.SetInt (profileName + orb3, PassiveTreeScript.Orb3);//dodano
		SaveSystem.SetInt (profileName + orb4, PassiveTreeScript.Orb4);//dodano
		SaveSystem.SetInt (profileName + orb5, PassiveTreeScript.Orb5);//dodano
		SaveSystem.SetInt (profileName + orb6, PassiveTreeScript.Orb6);//dodano
		SaveSystem.SetInt (profileName + orb7, PassiveTreeScript.Orb7);//dodano
		SaveSystem.SetInt (profileName + orb8, PassiveTreeScript.Orb8);//dodano
		SaveSystem.SetInt (profileName + orb9, PassiveTreeScript.Orb9);//dodano
		SaveSystem.SetInt (profileName + orb10, PassiveTreeScript.Orb10);//dodano


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

    public static bool Load(string profileName)
    {
        if (!GetSaveGameNames().Contains(profileName))
        {
            //    print("Save profil " + profileName + " ne postoji");
            return false;
        }

        PlayerManager.Character = (PlayerManager.PlayerCharacter) SaveSystem.GetInt(profileName + character);

        var player = PlayerManager.Player;

        player.transform.position = SaveSystem.GetVector3(profileName + position);
        player.transform.rotation = Quaternion.Euler(SaveSystem.GetVector3(profileName + rotation));

        PlayerManager.Experience = SaveSystem.GetInt(profileName + experience);
		PassiveTreeScript.Dexterity = SaveSystem.GetInt (profileName + statDex);//dodano
		PassiveTreeScript.Intelligence = SaveSystem.GetInt (profileName + statIntel);//dodano
		PassiveTreeScript.Constitution = SaveSystem.GetInt (profileName + statConst);//dodano
		PassiveTreeScript.Strength = SaveSystem.GetInt (profileName + statStr);//dodano
		PassiveTreeScript.AvailablePoints = SaveSystem.GetInt (profileName + statAvail);//dodano
		PassiveTreeScript.Orb1 = SaveSystem.GetInt (profileName + orb1);//dodano
		PassiveTreeScript.Orb2 = SaveSystem.GetInt (profileName + orb2);//dodano
		PassiveTreeScript.Orb3 = SaveSystem.GetInt (profileName + orb3);//dodano
		PassiveTreeScript.Orb4 = SaveSystem.GetInt (profileName + orb4);//dodano
		PassiveTreeScript.Orb5 = SaveSystem.GetInt (profileName + orb5);//dodano
		PassiveTreeScript.Orb6 = SaveSystem.GetInt (profileName + orb6);//dodano
		PassiveTreeScript.Orb7 = SaveSystem.GetInt (profileName + orb7);//dodano
		PassiveTreeScript.Orb8 = SaveSystem.GetInt (profileName + orb8);//dodano
		PassiveTreeScript.Orb9 = SaveSystem.GetInt (profileName + orb9);//dodano
		PassiveTreeScript.Orb10 = SaveSystem.GetInt (profileName + orb10);//dodano

        PlayerStats ps = PlayerManager.PlayerStats;
        ps.CurrentHealth = SaveSystem.GetInt(profileName + health);
        ps.MaxHealth = SaveSystem.GetInt(profileName + maxHealth);
        ps.CurrentMana = SaveSystem.GetInt(profileName + mana);
        ps.MaxMana = SaveSystem.GetInt(profileName + maxMana);
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
        return true;
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
