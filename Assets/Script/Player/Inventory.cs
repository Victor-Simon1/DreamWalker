using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory instance = null;

    private int coinsCount;

    [SerializeField] private TextMeshProUGUI coinsText;
    [SerializeField] private GameObject goInventory;
    private List<Item> content = new List<Item>();
    private int itemSelect = 0;
    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogError("Inventory is already set ");
            return;
        }
        instance = this;
   
    }

    public void AddCoins(int count)
    {
        coinsCount += count;
        coinsText.text = coinsCount.ToString();
    }

    void RefreshInventory()
    {
        int firstItem = itemSelect - 2;
        int lastItem = itemSelect + 2;
        foreach(GameObject caseInv in goInventory.transform)
        {
            //caseInv.transform.GetChild(2).GetComponent<Image>() = ;
        }
    }
    public void ConsumeItem()
    {
        if (content.Count == 0)
            return;
        Item currentItem = content[itemSelect];
        PlayerHealth.instance.HealPlayer(currentItem.hpGiven);
        PlayerMovement.instance.AddSpeed(currentItem.speedGiven);

        content.Remove(currentItem);
    }
    public void GetNextItem()
    {
        if(itemSelect< content.Count-1)
            itemSelect++;
    }
    public void GetPreviousItem()
    {
        if(itemSelect>0)
            itemSelect--;
    }
}
