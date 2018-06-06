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
        instance = this;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        inventoryItems = (ItemDataBaseList)Resources.Load("ItemDatabase");
    }
    #endregion

    private void Start()
    {
        Invoke("Load", 0);
    }
    void Load()
    {
        var loaded = SaveGame.Load(LoadGameScript.LoadGameName);

       
        // refresh stats 
        PlayerManager.RefreshStats();

        // ako je ovo nova igra prikazi intro dialoge
        if(!loaded)
            StartIntroDialog();
        

    }
    void StartIntroDialog()
    {
        DialogMgr.OnDialogEnd += OnDialogEnd;
        PlayerManager.StopPlayerMovement();
        GetButtonsDialogMgr.BeginDialog(GetDialogsCollection.getDialogList("Intro"));
    }

    private void OnDialogEnd()
    {
        PlayerManager.ResumePlayerMovement();

        DialogMgr.OnDialogEnd -= OnDialogEnd;

        GetDialogMgr.BeginDialog(GetDialogsCollection.getDialogList("TutorialIntro"));

        DialogMgr.OnDialogEnd += OnTutorialDialogEnd;
    }
    private void OnTutorialDialogEnd()
    {
        DialogMgr.OnDialogEnd -= OnTutorialDialogEnd;
        QuestManager.AcceptQuest("Village");
    }

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
        var y = enemy.GetComponentInChildren<BoxCollider>().size.y + .2f ;
        hb.GetComponent<RectTransform>().localPosition = new Vector3(0, y, 0);
        return hb.GetComponentInChildren<ProgressBar>();
    }


    public static void NotifyPickedUpItem(Item item)
    {
        QuestManager.ItemCollected(item.itemID);
        //TODO: item picked up
    }

    public static void EnemyDied(Enemy enemy)
    {
        QuestManager.EnemyKilled(enemy.enemyType);

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


                    drop.transform.position = new Vector3(enemy.DyingPosition.x + offset++,
                                                          enemy.DyingPosition.y + drop.transform.position.y + 0.1f,
                                                          enemy.DyingPosition.z);

                    drop.transform.rotation = Quaternion.LookRotation(Vector3.zero);

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
