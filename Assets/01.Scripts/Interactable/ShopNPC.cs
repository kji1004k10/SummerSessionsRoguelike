using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;


public class ShopNPC : Interactable
{
    public GameObject ShopPanel;
    public GameObject[] ItemListUI = new GameObject[4];
    private List<Dictionary<string, object>> itemTable;
    private Sprite[] itemSprites = new Sprite[10];


    private void Start()
    {
        itemTable = CSVReader.Read("Item/ItemTable");
        ShopPanel.SetActive(false);
        for (int i = 0; i < 10; i++)
        {
            Sprite loadImage = Resources.Load<Sprite>("Item/Icon/Accessory/" + itemTable[i]["ItemIcon"]);
            itemSprites[i] = loadImage;
        }

        List<int> itemIDs = GetItemID();
        // 상점 주인 상호작용 구현
        for (int i = 0; i < 4; i++)
        {
            int gold = int.Parse(itemTable[itemIDs[i]]["Gold"].ToString().Replace("G", ""));
            string className = itemTable[itemIDs[i]]["Class"].ToString();
            ShopDisplay display = ItemListUI[i].GetComponent<ShopDisplay>();

            display.SetCanBuy(true);
            display.itemName = itemTable[itemIDs[i]]["ItemName"].ToString();
            display.itemDescription = itemTable[itemIDs[i]]["Item"].ToString() + "\n"
                + itemTable[itemIDs[i]]["Abillty"].ToString();
            display.itemIcon = itemSprites[itemIDs[i]];
            ItemListUI[i].transform.Find("Image").GetComponent<Image>().sprite = itemSprites[itemIDs[i]];
            ItemListUI[i].GetComponentInChildren<TextMeshProUGUI>().text = itemTable[itemIDs[i]]["Gold"].ToString();
            ItemListUI[i].GetComponentInChildren<Button>().onClick.AddListener(() =>
            {
                if (GameManager.instance.Gold - gold >= 0 && display.canBuy == true)
                {
                    System.Type type = System.Type.GetType(className);
                    Item item = System.Activator.CreateInstance(type) as Item;
                    Player.Instance.Inventory.AddItem(item);
                    GameManager.instance.Gold -= gold;
                    display.SetCanBuy(false);
                }
            });
        }
    }

    public override void OnInteract(Player player)
    {
        ShopPanel.SetActive(!ShopPanel.activeSelf);

    }
    
    private List<int> GetItemID()
    {
        List<int> result = new List<int>();
        
        while (result.Count < 4)
        {
            while (true)
            {
                int rand = Random.Range(0, 10);
                bool notEqual = true;
                foreach (var id in result)
                {
                    if(rand == id)
                    {
                        notEqual = false;
                        break;
                    }
                }
                if (notEqual == true)
                {
                    result.Add(rand);
                    break;
                }
            }
        }
        return result;
    }

    public void ExitShop() => ShopPanel.SetActive(false);

}
