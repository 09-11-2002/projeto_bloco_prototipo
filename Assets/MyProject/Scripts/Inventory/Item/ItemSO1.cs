using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory.Model
{
    // Classe abstrata para todos os itens do jogo
    public abstract class ItemSO : ScriptableObject
    {
        // Define se o item pode ser empilhado no invent�rio
        [field: SerializeField]
        public bool IsStackable { get; set; }

        // ID �nico do item (gerado automaticamente)
        public int ID => GetInstanceID();

        // Define o tamanho m�ximo da pilha (1 = n�o empilh�vel)
        [field: SerializeField]
        public int MaxStackSize { get; set; } = 1;

        // Nome do item
        [field: SerializeField]
        public string Name { get; set; }

        // Descri��o do item (com suporte a m�ltiplas linhas)
        [field: SerializeField]
        [field: TextArea]
        public string Description { get; set; }

        // Imagem associada ao item
        [field: SerializeField]
        public Sprite ItemImage { get; set; }

        // Lista de par�metros padr�o do item (exemplo: ataque, defesa, velocidade)
        [field: SerializeField]
        public List<ItemParameter> DefaultParametersList { get; set; }
    }

    // Define par�metros que podem modificar atributos do personagem
    [Serializable]
    public struct ItemParameter : IEquatable<ItemParameter>
    {
        // Tipo do par�metro (exemplo: ataque, defesa, vida)
        public ItemParameterSO itemParameter;

        // Valor do par�metro (exemplo: +10 de ataque, +5 de velocidade)
        public float value;

        // M�todo para comparar dois par�metros e verificar se s�o do mesmo tipo
        public bool Equals(ItemParameter other)
        {
            return other.itemParameter == itemParameter;
        }
    }
}
