using Inventory.Model;
using Inventory.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Inventory
{
    public class InventoryManager : MonoBehaviour
    {
        [Header("Inventory UI & Audio")]
        [SerializeField] private InventoryUI inventoryUI; // Refer�ncia ao script de UI do invent�rio
        [SerializeField] private AudioClip dropClip; // Som de quando um item � descartado
        [SerializeField] private AudioSource audioSource; // Componente de �udio para tocar sons do invent�rio

        [Header("Inventory Data")]
        [SerializeField] private InventorySO inventoryData; // ScriptableObject que armazena os dados do invent�rio
        [SerializeField] private int inventorySize = 5; // Tamanho m�ximo do invent�rio
        [SerializeField] private List<InventoryItem> initialItems; // Itens iniciais do invent�rio

        private void Start()
        {
            PrepareUI(); // Configura a interface do invent�rio
            PrepareInventoryData(); // Inicializa os dados do invent�rio
        }

        private void PrepareInventoryData()
        {
            inventoryData.Initialize(); // Reseta os dados do invent�rio
            inventoryData.OnInventoryUpdated += UpdateInventoryUI; // Atualiza UI sempre que o invent�rio muda

            // Adiciona os itens iniciais ao invent�rio, se existirem
            foreach (InventoryItem item in initialItems)
            {
                if (item.IsEmpty) continue; // Ignora itens vazios
                inventoryData.AddItem(item);
            }
        }

        private void UpdateInventoryUI(Dictionary<int, InventoryItem> inventoryState)
        {
            inventoryUI.ResetAllItems(); // Limpa todos os itens visuais da UI

            // Atualiza os slots com os itens e suas quantidades
            foreach (var item in inventoryState)
            {
                inventoryUI.UpdateData(item.Key, item.Value.item.ItemImage, item.Value.quantity);
            }
        }

        private void PrepareUI()
        {
            inventoryUI.IniciaUIInventory(inventoryData.Size); // Inicializa a interface do invent�rio

            // Conecta eventos da UI com as fun��es correspondentes no script
            inventoryUI.OnDescriptionRequested += HandleDescriptionRequest;
            inventoryUI.OnSwapItems += HandleSwapItems;
            inventoryUI.OnStartDragging += HandleDragging;
            inventoryUI.OnItemActionRequested += HandleItemActionRequest;
        }

        private void HandleItemActionRequest(int itemIndex)
        {
            InventoryItem inventoryItem = inventoryData.GetItemAt(itemIndex);
            if (inventoryItem.IsEmpty) return; // Se o slot est� vazio, n�o faz nada

            IItemAction itemAction = inventoryItem.item as IItemAction;
            if (itemAction != null)
            {
                inventoryUI.ShowItemAction(itemIndex); // Exibe as op��es de a��o para o item
                inventoryUI.AddAction(itemAction.ActionName, () => PerformAction(itemIndex)); // Adiciona a��o � UI
            }

            IDestroyableItem destroyableItem = inventoryItem.item as IDestroyableItem;
            if (destroyableItem != null)
            {
                inventoryUI.AddAction("Drop", () => DropItem(itemIndex, inventoryItem.quantity)); // Adiciona op��o de descarte
            }

        }

        private void DropItem(int itemIndex, int quantity)
        {
            inventoryData.RemoveItem(itemIndex, quantity); // Remove item do invent�rio
            inventoryUI.ResetSelection(); // Reseta a sele��o da UI
            audioSource.PlayOneShot(dropClip); // Toca som de descarte
        }

        public void PerformAction(int itemIndex)
        {
            InventoryItem inventoryItem = inventoryData.GetItemAt(itemIndex);
            if (inventoryItem.IsEmpty) return; // Se o slot est� vazio, n�o faz nada

            IDestroyableItem destroyableItem = inventoryItem.item as IDestroyableItem;
            if (destroyableItem != null)
            {
                inventoryData.RemoveItem(itemIndex, 1); // Remove uma unidade do item do invent�rio
            }

            IItemAction itemAction = inventoryItem.item as IItemAction;
            if (itemAction != null)
            {
                itemAction.PerformAction(gameObject, inventoryItem.itemState); // Executa a a��o do item
                audioSource.PlayOneShot(itemAction.actionSFX); // Toca som da a��o

                if (inventoryData.GetItemAt(itemIndex).IsEmpty)
                    inventoryUI.ResetSelection(); // Se o item foi consumido, reseta sele��o
            }
        }

        private void HandleDragging(int itemIndex)
        {
            InventoryItem inventoryItem = inventoryData.GetItemAt(itemIndex);
            if (inventoryItem.IsEmpty) return; // Se o slot est� vazio, n�o faz nada
            inventoryUI.CreateDraggedItem(inventoryItem.item.ItemImage, inventoryItem.quantity); // Cria o �cone do item arrastado
        }

        private void HandleSwapItems(int itemIndex_1, int itemIndex_2)
        {
            inventoryData.SwapItems(itemIndex_1, itemIndex_2); // Troca os itens de posi��o no invent�rio
        }

        private void HandleDescriptionRequest(int itemIndex)
        {
            InventoryItem inventoryItem = inventoryData.GetItemAt(itemIndex);
            if (inventoryItem.IsEmpty)
            {
                inventoryUI.ResetSelection(); // Se o slot est� vazio, reseta a sele��o na UI
                return;
            }

            ItemSO item = inventoryItem.item;
            string description = PrepareDescription(inventoryItem); // Obt�m a descri��o do item
            inventoryUI.UpdateDescription(itemIndex, item.ItemImage, item.name, description); // Atualiza UI com descri��o
        }

        private string PrepareDescription(InventoryItem inventoryItem)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(inventoryItem.item.Description);
            sb.AppendLine();
            for (int i = 0; i < inventoryItem.itemState.Count; i++)
            {
                sb.Append($"{inventoryItem.itemState[i].itemParameter.ParameterName} " +
                    $": {inventoryItem.itemState[i].value} / " +
                    $"{inventoryItem.item.DefaultParametersList[i].value}");
                sb.AppendLine();
            }
            return sb.ToString();
        }

        private void Update()
        {
            // Liga e desliga o invent�rio ao pressionar "I" ou fecha ao pressionar "Esc"
            if (Input.GetKeyDown(KeyCode.I))
            {
                ToggleInventory();
            }
            else if (Input.GetKeyDown(KeyCode.Escape) && inventoryUI.isActiveAndEnabled)
            {
                inventoryUI.Hide();
            }
        }

        private void ToggleInventory()
        {
            if (inventoryUI.isActiveAndEnabled == false)
            {
                inventoryUI.Show();
                foreach (var item in inventoryData.GetCurrentInventoryState())
                {
                    inventoryUI.UpdateData(item.Key,
                        item.Value.item.ItemImage,
                        item.Value.quantity);
                }
            }
            else
            {
                inventoryUI.Hide();
            }

        }
    }
}