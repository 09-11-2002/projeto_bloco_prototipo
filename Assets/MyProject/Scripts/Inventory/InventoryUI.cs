using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using Unity.VisualScripting;

namespace Inventory.UI
{
    public class InventoryUI : MonoBehaviour 
    {
        [SerializeField] private UIInventoryItem itemPrefab; // Prefab do item da UI do invent�rio
        [SerializeField] private RectTransform contentPanel; // Painel onde os itens ser�o exibidos
        [SerializeField] private DescriptionUI descriptionUI; // Componente para exibir a descri��o dos itens
        [SerializeField] private MouseFollower mouseFollower; // Componente para seguir o mouse com o item arrastado

        List<UIInventoryItem> listOfUIItems = new List<UIInventoryItem>(); // Lista para armazenar os itens da UI do invent�rio

        private int currentDraggedItemIndex = -1; // �ndice do item que est� sendo arrastado

        // Eventos para comunica��o com outros scripts
        public event Action<int> OnDescriptionRequested, // Evento quando a descri��o de um item � requisitada
            OnItemActionRequested, // Evento quando uma a��o � requisitada para um item
            OnStartDragging; // Evento quando o arrastar de um item come�a
        public event Action<int, int> OnSwapItems; // Evento quando dois itens s�o trocados

        [SerializeField] private ItemActionPanel actionPanel; // Painel de a��es do item, serializado

        private void Awake() 
        {
            Hide(); // Esconde a UI do invent�rio
            mouseFollower.Toggle(false); // Desativa o objeto que segue o mouse
            descriptionUI.ResetDescription(); // Reseta a descri��o do item
        }

        private void HandleShowItemActions(UIInventoryItem inventoryItemUI) // Lida com a exibi��o das a��es do item (clique com o bot�o direito)
        {
            int index = listOfUIItems.IndexOf(inventoryItemUI); // Obt�m o �ndice do item na lista
            if (index == -1) // Se o item n�o foi encontrado
            {
                return; // Sai da fun��o
            }
            OnItemActionRequested?.Invoke(index); // Invoca o evento OnItemActionRequested passando o �ndice do item
        }

        private void HandleEndDrag(UIInventoryItem inventoryItemUI) // Lida com o fim do arrastar do item
        {
            ResetDraggedItem(); // Reseta o item arrastado
        }

        private void HandleSwap(UIInventoryItem inventoryItemUI) // Lida com a troca de itens
        {
            int index = listOfUIItems.IndexOf(inventoryItemUI); // Obt�m o �ndice do item na lista
            if (index == -1) // Se o item n�o foi encontrado
            {
                return; // Sai da fun��o
            }
            OnSwapItems?.Invoke(currentDraggedItemIndex, index); // Invoca o evento OnSwapItems passando os �ndices dos itens a serem trocados
            HandleItemSelection(inventoryItemUI); // Seleciona o item ap�s a troca
        }

        private void ResetDraggedItem() // Reseta o item arrastado
        {
            mouseFollower.Toggle(false); // Desativa o objeto que segue o mouse
            currentDraggedItemIndex = -1; // Reseta o �ndice do item arrastado
        }

        private void HandleBeginDrag(UIInventoryItem inventoryItemUI) // Lida com o in�cio do arrastar do item
        {
            int index = listOfUIItems.IndexOf(inventoryItemUI); // Obt�m o �ndice do item na lista
            if (index == -1) // Se o item n�o foi encontrado
                return; // Sai da fun��o
            currentDraggedItemIndex = index; // Define o �ndice do item arrastado
            HandleItemSelection(inventoryItemUI); // Seleciona o item que est� sendo arrastado
            OnStartDragging?.Invoke(index); // Invoca o evento OnStartDragging passando o �ndice do item
        }

        public void CreateDraggedItem(Sprite sprite, int quantity) // Cria o item que segue o mouse durante o arrastar
        {
            mouseFollower.Toggle(true); // Ativa o objeto que segue o mouse
            mouseFollower.SetData(sprite, quantity); // Define os dados do item que segue o mouse
        }

        private void HandleItemSelection(UIInventoryItem inventoryItemUI) // Lida com a sele��o de um item
        {
            int index = listOfUIItems.IndexOf(inventoryItemUI); // Obt�m o �ndice do item na lista
            if (index == -1) // Se o item n�o foi encontrado
                return; // Sai da fun��o
            OnDescriptionRequested?.Invoke(index); // Invoca o evento OnDescriptionRequested passando o �ndice do item
        }

        public void Show() // Mostra a UI do invent�rio
        {
            gameObject.SetActive(true); // Ativa o objeto da UI
            ResetSelection(); // Reseta a sele��o dos itens
        }

        public void ResetSelection() // Reseta a sele��o dos itens
        {
            descriptionUI.ResetDescription(); // Reseta a descri��o do item
            DeselectAllItems(); // Desseleciona todos os itens
        }

        public void AddAction(string actionName, Action performAction) // Adiciona uma a��o ao painel de a��es
        {
            actionPanel.AddButton(actionName, performAction); // Adiciona um bot�o ao painel de a��es
        }

        public void ShowItemAction(int itemIndex) // Exibe o painel de a��es para um item espec�fico
        {
            actionPanel.Toggle(true); // Ativa o painel de a��es
            actionPanel.transform.position = listOfUIItems[itemIndex].transform.position; // Define a posi��o do painel de a��es pr�ximo ao item
        }

        private void DeselectAllItems() // Desseleciona todos os itens
        {
            foreach (UIInventoryItem item in listOfUIItems) // Itera sobre todos os itens da lista
            {
                item.Deselect(); // Desseleciona o item
            }
            actionPanel.Toggle(false); // Desativa o painel de a��es
        }

        public void Hide() // Esconde a UI do invent�rio
        {
            actionPanel.Toggle(false); // Desativa o painel de a��es
            gameObject.SetActive(false); // Desativa o objeto da UI
            ResetDraggedItem(); // Reseta o item arrastado
        }

        public void IniciaUIInventory(int inventorySize) // Inicializa a UI do invent�rio
        {
            for (int i = 0; i < inventorySize; i++) // Cria os itens da UI de acordo com o tamanho do invent�rio
            {
                UIInventoryItem UIitem = Instantiate(itemPrefab, Vector3.zero, Quaternion.identity); // Instancia o prefab do item
                UIitem.transform.SetParent(contentPanel); // Define o item como filho do painel de conte�do
                listOfUIItems.Add(UIitem); // Adiciona o item � lista
                // Adiciona listeners para os eventos do item
                UIitem.OnItemClicked += HandleItemSelection;
                UIitem.OnItemBeginDrag += HandleBeginDrag;
                UIitem.OnItemDroppedOn += HandleSwap;
                UIitem.OnItemEndDrag += HandleEndDrag;
                UIitem.OnRightMouseBtnClick += HandleShowItemActions;
            }
        }

        internal void ResetAllItems() // Reseta todos os itens da UI
        {
            foreach (var item in listOfUIItems) // Itera sobre todos os itens
            {
                item.ResetData(); // Reseta os dados do item
                item.Deselect(); // Desseleciona o item
            }
        }

        internal void UpdateDescription(int itemIndex, Sprite itemImage, string name, string description) // Atualiza a descri��o do item
        {
            descriptionUI.SetDescription(itemImage, name, description); // Define a descri��o no componente DescriptionUI
            DeselectAllItems(); // Desseleciona todos os itens
            listOfUIItems[itemIndex].Select(); // Seleciona o item cuja descri��o foi atualizada
        }

        public void UpdateData(int itemIndex, Sprite itemImage, int itemQuantity) // Atualiza os dados de um item espec�fico na UI do invent�rio
        {
            if (listOfUIItems.Count > itemIndex) // Verifica se o �ndice do item � v�lido (se existe um item nesse �ndice na lista)
            {
                listOfUIItems[itemIndex].SetData(itemImage, itemQuantity); // Chama a fun��o SetData do item da UI para atualizar sua imagem (Sprite) e quantidade
            }
        }
    }
}