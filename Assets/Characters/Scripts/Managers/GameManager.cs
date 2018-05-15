using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject OnEnemyDeathParticle;
    public GameObject EnemyHealthBar;
    public DialogMgr Dialog;
    public DialogMgr ButtonsDialog;

    public DialogsCollection dialogsCollection;



    public LootTableEntry[] LootTable;

    private static ItemDataBaseList inventoryItems;

    [System.Serializable]
    public class LootTableEntry
    {
        public EnemyController.EnemyType enemyType;
        public int ExperienceFromKilling;
        public LootTableEntryItems[] items;
    }

    [System.Serializable]
    public class LootTableEntryItems
    {
        public int itemId;
        public int count = 1;
    }


    #region Singleton
    public static GameManager instance;

    void Awake()
    {
		SaveGame.Load (LoadGameScript.LoadGameName);
        instance = this;
        Cursor.visible = false;

        inventoryItems = (ItemDataBaseList)Resources.Load("ItemDatabase");
    }
    #endregion

    public static DialogMgr GetDialogMgr
    {
        get { return instance.Dialog; }
    }

    public static DialogMgr GetButtonsDialogMgr
    {
        get { return instance.ButtonsDialog; }
    }
    public static DialogsCollection GetDialogsCollection
    {
        get { return instance.dialogsCollection; }
    }

    public static ItemDataBaseList ItemDatabase
    {
        get { return inventoryItems; }
    }
    public static ProgressBar GetEnemyHealthBarCopy(GameObject enemy)
    {
      
        var hb = Instantiate(instance.EnemyHealthBar, enemy.transform);
        hb.SetActive(true);
        var y = enemy.GetComponent<BoxCollider>().size.y + .2f ;
        hb.GetComponent<RectTransform>().localPosition = new Vector3(0, y, 0);
        return hb.GetComponentInChildren<ProgressBar>();
    }


    public static void NotifyPickedUpItem(Item item)
    {
        //TODO: item picked up
    }

    public static void EnemyDied(Enemy enemy)
    {
        foreach(var entry in instance.LootTable){
            if(entry.enemyType == enemy.EnemyController.enemyType)
            {
                PlayerManager.GainExperience(entry.ExperienceFromKilling);

                float offset = 0;
                foreach (var item in entry.items)
                {
                    var inventoryItem = inventoryItems.getItemByID(item.itemId);
                    var drop = Instantiate(inventoryItem.itemModel);
                    if (!drop.activeSelf) drop.SetActive(true);
                    drop.transform.position = new Vector3(  enemy.transform.position.x + offset++, 
                                                            drop.transform.position.y, 
                                                                enemy.transform.position.z);

                    var pickUpItemScript = drop.GetComponent<PickUpItem>();
                    if (!pickUpItemScript)
                        pickUpItemScript = drop.AddComponent<PickUpItem>();
                    
                    pickUpItemScript.item = inventoryItem;
                    pickUpItemScript.item.itemValue = item.count;

                    pickUpItemScript.Start();
                }


                break;
            }
        }
       
    }
}
