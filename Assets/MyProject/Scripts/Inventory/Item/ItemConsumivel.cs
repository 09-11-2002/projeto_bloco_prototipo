using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory.Model
{
    // Define um Item Consum�vel no jogo
    [CreateAssetMenu] // Permite criar o item pelo Unity
    public class ItemConsumivel : ItemSO, IDestroyableItem, IItemAction
    {
        // Lista de modificadores que afetam o personagem ao consumir o item
        [SerializeField]
        private List<ModifierData> modifiersData = new List<ModifierData>();

        // Nome da a��o ao usar o item (por exemplo, "Consumido")
        public string ActionName => "Consumido";

        // Som ao usar o item (caso tenha)
        public AudioClip actionSFX { get; private set; }

        // M�todo chamado quando o item � consumido
        public bool PerformAction(GameObject character, List<ItemParameter> itemState = null)
        {
            // Aplica cada modificador ao personagem
            foreach (ModifierData data in modifiersData)
            {
                data.statModifier.AffectCharacter(character, data.value);
            }
            return true; // Indica que o item foi usado com sucesso
        }
    }

    // Interface que indica que o item ser� destru�do ap�s o uso
    public interface IDestroyableItem
    {

    }

    // Interface que define a��es que um item pode realizar
    public interface IItemAction
    {
        public string ActionName { get; }
        public AudioClip actionSFX { get; }
        bool PerformAction(GameObject character, List<ItemParameter> itemState);
    }

    // Classe que define um modificador para o personagem
    [Serializable]
    public class ModifierData
    {
        public CharacterStatModifierSO statModifier; // Tipo de modifica��o (ex: aumentar vida)
        public float value; // Valor da modifica��o (ex: +50 de vida)
    }
}
