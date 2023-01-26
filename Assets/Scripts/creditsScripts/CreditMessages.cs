using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class CreditMessages : MonoBehaviour
{
    public TextMeshProUGUI[] dialogueText;
    public GameObject button;
    public float letterDelay = 0.1f;
    private string[] fullText;
    private string[] currentText;
    private bool isTyping = false;

    void Start()
    {
        fullText = new string[dialogueText.Length];
        currentText = new string[dialogueText.Length];
        for (int i = 0; i < dialogueText.Length; i++)
        {
            fullText[i] = dialogueText[i].text;
            dialogueText[i].text = "";
        }
        StartTyping();
    }

    public void StartTyping()
    {
        StartCoroutine(TypeText());
    }

    IEnumerator TypeText()
    {
        isTyping = true;
        for (int i = 0; i < fullText.Length; i++)
        {
            if (i == 2)
            {
                letterDelay *= 0.2f;
            }
            for (int j = 0; j < fullText[i].Length; j++)
            {
                currentText[i] += fullText[i][j];
                dialogueText[i].text = currentText[i];
                yield return new WaitForSeconds(letterDelay);
            }
        }
        isTyping = false;
        button.SetActive(true);
    }

}
