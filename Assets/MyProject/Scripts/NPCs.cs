using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCs : MonoBehaviour
{
    [SerializeField] DialogueDataSO dialogueData;

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerInteraction player;

        if (other.TryGetComponent<PlayerInteraction>(out player)) 
        {
            Debug.Log("Player pr�ximo");
            GameEvents.Instance.StartDialogue(dialogueData);
        }
    }
}
