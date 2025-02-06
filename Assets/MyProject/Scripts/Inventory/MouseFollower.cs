using Inventory.UI; 
using System.Collections; 
using System.Collections.Generic; 
using UnityEngine; 

public class MouseFollower : MonoBehaviour 
{
    [SerializeField] private Canvas canvas; // Vari�vel para armazenar o canvas onde o item ser� exibido
    [SerializeField] private UIInventoryItem item; // Vari�vel para armazenar o item da UI do invent�rio

    private void Awake() 
    {
        canvas = transform.root.GetComponent<Canvas>(); // Obt�m o componente Canvas do objeto raiz (pai) deste objeto
        Debug.Log($"Canvas encontrado: {canvas != null}"); 

        item = GetComponentInChildren<UIInventoryItem>(); // Obt�m o componente UIInventoryItem dos filhos deste objeto
        Debug.Log($"UIInventoryItem encontrado: {item != null}"); 
    }

    public void SetData(Sprite sprite, int quantity) // Fun��o para definir os dados do item (sprite e quantidade)
    {
        item.SetData(sprite, quantity); // Chama a fun��o SetData do componente UIInventoryItem para atualizar a imagem e a quantidade do item na UI
    }

    private void Update() 
    {
        Vector2 position; // Vari�vel para armazenar a posi��o do mouse no canvas

        // Converte a posi��o do mouse na tela para a posi��o local no canvas
        RectTransformUtility.ScreenPointToLocalPointInRectangle
            (
                (RectTransform)canvas.transform, // Transforma do canvas
                Input.mousePosition, // Posi��o do mouse na tela
                canvas.worldCamera, // C�mera do mundo do canvas
                out position // Vari�vel de sa�da para a posi��o local no canvas
            );

        transform.position = canvas.transform.TransformPoint(position); // Define a posi��o deste objeto (MouseFollower) para a posi��o calculada no canvas
    }

    public void Toggle(bool value) // Fun��o para ativar/desativar o objeto MouseFollower
    {
        Debug.Log($"Item alternado {value}"); 
        gameObject.SetActive(value); // Ativa ou desativa o objeto MouseFollower
    }
}