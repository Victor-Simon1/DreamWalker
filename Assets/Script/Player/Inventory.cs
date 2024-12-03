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
    public void AddCoins(int count)
    {
        coinsCount += count;
        coinsText.text = coinsCount.ToString();
    }

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
    public void AddItem(Item item)
    {
        if(content.Count < maxSize)
        {
            content.Add(item);
            RefreshInventoryUI();
        }
        isFull = content.Count >= maxSize;
          
    }
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
