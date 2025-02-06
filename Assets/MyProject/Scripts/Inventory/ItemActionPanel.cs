using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Inventory.UI
{
    public class ItemActionPanel : MonoBehaviour
    {
        // Prefab do bot�o que ser� instanciado no painel
        [SerializeField]
        private GameObject buttonPrefab;

        // Adiciona um novo bot�o ao painel com um nome e uma a��o ao clicar
        public void AddButton(string name, Action onClickAction)
        {
            // Cria um novo bot�o a partir do prefab
            GameObject button = Instantiate(buttonPrefab, transform);

            // Adiciona a a��o de clique ao bot�o
            button.GetComponent<Button>().onClick.AddListener(() => onClickAction());

            // Define o texto do bot�o com o nome passado como par�metro
            button.GetComponentInChildren<TMPro.TMP_Text>().text = name;
        }

        // Alterna a visibilidade do painel
        internal void Toggle(bool val)
        {
            // Se estiver ativando o painel, remove bot�es antigos para evitar ac�mulo
            if (val == true)
                RemoveOldButtons();

            // Ativa ou desativa o painel
            gameObject.SetActive(val);
        }

        // Remove todos os bot�es antigos antes de adicionar novos
        public void RemoveOldButtons()
        {
            // Percorre todos os elementos filhos (bot�es) e os destr�i
            foreach (Transform transformChildObjects in transform)
            {
                Destroy(transformChildObjects.gameObject);
            }
        }
    }
}