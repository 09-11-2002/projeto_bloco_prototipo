using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory.Model
{
    // Permite criar itens equip�veis dentro do Unity
    [CreateAssetMenu]
    public class ItemEquipavel : ItemSO, IDestroyableItem, IItemAction
    {
        // Nome da a��o ao usar o item (exemplo: "Equipado")
        public string ActionName => "Equipado";

        // Som ao equipar o item (caso tenha um efeito sonoro)
        public AudioClip actionSFX { get; private set; }

        // M�todo que tenta equipar o item no personagem
        public bool PerformAction(GameObject character, List<ItemParameter> itemState = null)
        {
            // Busca o componente AgentWeapon no personagem
            AgentWeapon weaponSystem = character.GetComponent<AgentWeapon>();

            // Se o personagem tiver um sistema de armas, equipa o item
            if (weaponSystem != null)
            {
                weaponSystem.SetWeapon(
                    this, // Passa o pr�prio item como arma equipada
                    itemState == null ? DefaultParametersList : itemState // Usa a lista padr�o se itemState for nulo
                );
                return true; // Indica que o item foi equipado com sucesso
            }
            return false; // Retorna falso se o personagem n�o puder equipar
        }
    }
}
