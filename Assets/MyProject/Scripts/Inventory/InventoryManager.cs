using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }

    private List<ItemColectable> inventoryItems = new List<ItemColectable>();
    private bool isInventoryOpen = false;

    [SerializeField] private GameObject inventoryUI; // O painel de UI para o invent�rio

    private void Awake()
    {

        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Garante que o invent�rio come�a fechado
        if (inventoryUI != null)
        {
            inventoryUI.SetActive(false);
        }
    }

    private void Update()
    {
        // Alterna o invent�rio ao pressionar "I"
        if (Input.GetKeyDown(KeyCode.I))
        {
            ToggleInventory();
        }
    }

    public void AddItemToInventory(ItemColectable item)
    {
        inventoryItems.Add(item);
        Debug.Log($"{item.itemName} foi adicionado ao invent�rio!");
    }

    public void ToggleInventory()
    {
        isInventoryOpen = !isInventoryOpen;

        if (inventoryUI != null)
        {
            inventoryUI.SetActive(isInventoryOpen);
        }
    }

    public List<ItemColectable> GetInventoryItems()
    {
        foreach (var item in InventoryManager.Instance.GetInventoryItems())
        {
            if (item == null)
            {
                Debug.LogError("Item encontrado como null!");
                continue;
            }

            Debug.Log($"Item no invent�rio: {item.itemName}");
            // Processar o item
        }
        return inventoryItems;
    }
}
