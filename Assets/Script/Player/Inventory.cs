using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public static Inventory instance = null;

    [Header("Coins")]
    private int coinsCount;
    [SerializeField] private TextMeshProUGUI coinsText;
    [Header("Gameobject")]
    [SerializeField] private GameObject goInventory;
    [SerializeField] private List<GameObject> caseInventoryList = new List<GameObject>();
    [Header("Item List")]
    [SerializeField] private List<Item> content = new List<Item>();
    private int maxSize = 5;
    [SerializeField] public bool isFull = false;
    [Header("Item select")]
    private int itemSelect = 0;
    [SerializeField] private GameObject goItemSelect;
    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogError("Inventory is already set ");
            return;
        }
        instance = this;
   
    }
    private void Update()
    {
        if (UserInput.instance.ConsumeItemInput)
            ConsumeItem();
        if (UserInput.instance.NextCaseInput)
            GetNextItem();
        if (UserInput.instance.PreviousCaseInput)
            GetPreviousItem();
    }
    /// <summary>
    /// Add coins to the inventory
    /// </summary>
    /// <param name="count"></param>
    public void AddCoins(int count)
    {
        coinsCount += count;
        coinsText.text = coinsCount.ToString();
    }

    /// <summary>
    /// Refresh the UI of the inventory,call after something add or delete from the inventory
    /// </summary>
    void RefreshInventoryUI()
    {
        int iCase = 0;
        foreach(GameObject caseGo in caseInventoryList)
        {
            var img = caseGo.transform.GetChild(2).GetComponent<Image>();
            if(iCase<content.Count)
            {
                img.sprite = content[iCase].icon;
                img.enabled = true;
            }
            else
                img.enabled = false;
            iCase++;
        }
    }

    /// <summary>
    /// Add en item if the inventory is not full
    /// </summary>
    /// <param name="item"></param>
    public void AddItem(Item item)
    {
        if(content.Count < maxSize)
        {
            content.Add(item);
            RefreshInventoryUI();
        }
        isFull = content.Count >= maxSize;
          
    }
    /// <summary>
    /// Consume the item where the selection is set,nothing append if no item 
    /// </summary>
    public void ConsumeItem()
    {
        if (content.Count == 0)
            return;
        Debug.Log("Consume Item");
        if(itemSelect <= content.Count)
        {
            Item currentItem = content[itemSelect];
            if (currentItem)
            {
                PlayerHealth.instance.HealPlayer(currentItem.hpGiven);
                PlayerMovement.instance.AddSpeed(currentItem.speedGiven);

                content.Remove(currentItem);
                isFull = content.Count >= maxSize;
                RefreshInventoryUI();
            }
            else
                Debug.Log("No Item in case " + itemSelect);
        }
        else
            Debug.Log("No Item in case " + itemSelect);
    }

    /// <summary>
    /// Go to the right and stop when there is no more case
    /// </summary>
    public void GetNextItem()
    {
        Debug.Log("Next Item");
        if (itemSelect< maxSize-1)
        {
            itemSelect++;
            goItemSelect.transform.SetParent(caseInventoryList[itemSelect].transform);
            goItemSelect.transform.localPosition = Vector3.zero;
        }
            
    }
    /// <summary>
    /// Go to the left and stop when there is no more case
    /// </summary>
    public void GetPreviousItem()
    {
        Debug.Log("Previous Item");
        if (itemSelect>0)
        {
            itemSelect--;
            goItemSelect.transform.SetParent(caseInventoryList[itemSelect].transform);
            goItemSelect.transform.localPosition = Vector3.zero;
        }
           
    }
}
