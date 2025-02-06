using Inventory.Model; 
using System.Collections; 
using System.Collections.Generic; 
using UnityEngine; 

public class PickupSystem : MonoBehaviour
{
    [SerializeField] private InventorySO inventoryData; // Vari�vel para armazenar os dados do invent�rio (Scriptable Object), serializada para ser vis�vel no Inspector da Unity

    private void OnTriggerEnter2D(Collider2D collision) // Fun��o chamada quando um objeto 2D entra na �rea de colis�o do objeto com este script
    {
        Item item = collision.GetComponent<Item>(); // Tenta obter o componente Item do objeto que colidiu

        if (item != null) // Verifica se o objeto que colidiu possui um componente Item
        {
            int remainder = inventoryData.AddItem(item.InventoryItem, item.Quantity); // Chama a fun��o AddItem do invent�rio para adicionar o item e sua quantidade. remainder armazena a quantidade de itens que n�o couberam no invent�rio

            if (remainder == 0) // Verifica se todos os itens foram adicionados ao invent�rio
            {
                item.DestroyItem(); // Destr�i o item do jogo
            }
            else // Se nem todos os itens couberam no invent�rio
            {
                item.Quantity = remainder; // Atualiza a quantidade de itens no objeto com a quantidade restante
            }
        }
    }
}