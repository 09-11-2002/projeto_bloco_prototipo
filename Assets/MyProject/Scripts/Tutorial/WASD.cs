using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class WASD : MonoBehaviour
{
    [Header("Tutorial Settings")]
    [TextArea]
    public string tutorialMessage; // Mensagem de tutorial para exibir

    [Header("UI Reference")]
    public TextMeshProUGUI tutorialText; // Refer�ncia ao tutorial
    [SerializeField] private Image interactionIcon; // �cone da tecla "E"
    [SerializeField] private Transform iconPosition; // Posi��o manual do �cone

    private bool jaAtivado = false; // Tutorial s� aparece uma vez
    private bool isPlayerNearby = false; // Verifica se o player est� perto

    private void Start()
    {
        if (interactionIcon != null)
            interactionIcon.gameObject.SetActive(false); // O �cone come�a desativado
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (!jaAtivado)
            {
                DisplayMessage();
                jaAtivado = true; // Marca que o tutorial foi ativado
            }

            ShowInteractionIcon();
            isPlayerNearby = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            HideInteractionIcon();
            isPlayerNearby = false;
        }
    }

    private void Update()
    {
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Intera��o iniciada!");
            // Aqui voc� pode chamar o di�logo ou qualquer outra a��o
        }
    }

    private void DisplayMessage()
    {
        if (tutorialText != null)
        {
            tutorialText.text = tutorialMessage;
            tutorialText.gameObject.SetActive(true);
            StartCoroutine(HideMessageAfterTime(3f)); // Oculta o tutorial ap�s 3 segundos
        }
    }

    private IEnumerator HideMessageAfterTime(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (tutorialText != null)
            tutorialText.gameObject.SetActive(false);
    }

    private void ShowInteractionIcon()
    {
        if (interactionIcon != null && iconPosition != null)
        {
            interactionIcon.gameObject.SetActive(true);
            interactionIcon.transform.position = Camera.main.WorldToScreenPoint(iconPosition.position); // Converte posi��o
        }
    }

    private void HideInteractionIcon()
    {
        if (interactionIcon != null)
        {
            interactionIcon.gameObject.SetActive(false);
        }
    }
}
