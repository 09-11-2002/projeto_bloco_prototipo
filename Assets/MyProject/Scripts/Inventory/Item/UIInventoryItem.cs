using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Inventory.UI
{
    // Classe que representa um item na UI do invent�rio
    public class UIInventoryItem : MonoBehaviour,
        IPointerClickHandler, IBeginDragHandler, IEndDragHandler, IDropHandler, IDragHandler
    {
        [SerializeField] private Image itemImage; // Imagem do item
        [SerializeField] private TMP_Text quantityText; // Texto da quantidade do item
        [SerializeField] private Image borderImage; // Borda ao selecionar o item

        // Eventos que outros scripts podem escutar
        public event Action<UIInventoryItem> OnItemClicked,
            OnItemDroppedOn, OnItemBeginDrag, OnItemEndDrag, OnRightMouseBtnClick;

        private bool empty = true; // Define se o slot est� vazio

        private void Awake()
        {
            ResetData(); // Inicializa o item como vazio
            Deselect(); // Garante que n�o esteja selecionado
        }

        // Reseta os dados do item (deixa o slot vazio)
        public void ResetData()
        {
            itemImage.gameObject.SetActive(false);
            empty = true;
        }

        // Desseleciona o item (oculta a borda)
        public void Deselect()
        {
            borderImage.enabled = false;
        }

        // Define os dados do item (imagem e quantidade)
        public void SetData(Sprite sprite, int quantity)
        {
            itemImage.gameObject.SetActive(true); // Ativa a imagem do item
            itemImage.sprite = sprite; // Define a imagem
            quantityText.text = quantity + ""; // Define o texto da quantidade
            empty = false; // Marca o slot como preenchido
        }

        // Seleciona o item (exibe a borda)
        public void Select()
        {
            borderImage.enabled = true;
        }

        // Detecta clique no item (bot�o esquerdo ou direito)
        public void OnPointerClick(PointerEventData pointerData)
        {
            if (pointerData.button == PointerEventData.InputButton.Right)
            {
                OnRightMouseBtnClick?.Invoke(this); // Bot�o direito
            }
            else
            {
                OnItemClicked?.Invoke(this); // Bot�o esquerdo
            }
        }

        // In�cio do arrasto do item
        public void OnBeginDrag(PointerEventData eventData)
        {
            if (empty)
                return; // N�o arrasta se o slot estiver vazio
            OnItemBeginDrag?.Invoke(this);
        }

        // Fim do arrasto do item
        public void OnEndDrag(PointerEventData eventData)
        {
            OnItemEndDrag?.Invoke(this);
        }

        // Quando o item � solto em outro slot
        public void OnDrop(PointerEventData eventData)
        {
            OnItemDroppedOn?.Invoke(this);
        }

        // Durante o arrasto do item (n�o implementado)
        public void OnDrag(PointerEventData eventData)
        {
        }
    }
}
