using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.UI;

public class MessageManager : MonoBehaviour
{
    public TMP_Text MessageText;
    public GameObject MessagePanel;

    public static MessageManager Instance;

    void Awake()
    {
        Instance = this;
        MessagePanel.SetActive(false);
    }

    public void ShowMessage(string Text, float Duration)
    {
        StartCoroutine(ShowMessageCoroutine(Text, Duration));
    }

    IEnumerator ShowMessageCoroutine(string Text, float Duration)
    {
        MessageText.text = Text;
        MessagePanel.SetActive(true);

        yield return new WaitForSeconds(Duration);

        MessagePanel.SetActive(false);
        Command.CommandExecutionComplete();
    }

}
