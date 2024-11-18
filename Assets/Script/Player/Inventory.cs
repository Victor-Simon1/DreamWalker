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
        content.Add(item);
        RefreshInventoryUI();
    }
    public void ConsumeItem(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;
        
        if (content.Count == 0)
            return;
        Debug.Log("Consume Item");
        Item currentItem = content[itemSelect];
        PlayerHealth.instance.HealPlayer(currentItem.hpGiven);
        PlayerMovement.instance.AddSpeed(currentItem.speedGiven);

        content.Remove(currentItem);
        RefreshInventoryUI();
    }
    public void GetNextItem(InputAction.CallbackContext context)
    {
        if(!context.performed) 
            return;
        Debug.Log("Next Item");
        if (itemSelect< content.Count-1)
        {
            itemSelect++;
            goItemSelect.transform.SetParent(caseInventoryList[itemSelect].transform);
        }
            
    }
    public void GetPreviousItem(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;
        Debug.Log("Previous Item");
        if (itemSelect>0)
        {
            itemSelect--;
            goItemSelect.transform.SetParent(caseInventoryList[itemSelect].transform);
        }
           
    }
}
