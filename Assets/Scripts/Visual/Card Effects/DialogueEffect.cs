using UnityEngine;
using System.Collections;
using TMPro;
using DG.Tweening;

/// <summary>
/// This class will show damage dealt to creatures or payers
/// </summary>

public class DialogueEffect : MonoBehaviour
{

    // CanvasGropup should be attached to the Canvas of this damage effect
    // It is used to fade away the alpha value of this effect
    public CanvasGroup cg;

    // The text component to show the amount of damage taken by target like: "-2"
    public TMP_Text DialogueText;

    // A Coroutine to control the fading of this damage effect
    private IEnumerator ShowTextBubble(float waitTime)
    {
        // make this effect non-transparent
        cg.transform.localScale = Vector3.zero;

        cg.transform.DOScale(.005f, GameStateSync.CardTransitionTime).SetEase(Ease.InQuint);
        yield return new WaitForSeconds(waitTime);
        cg.transform.DOScale(0f, GameStateSync.CardTransitionTime).SetEase(Ease.InQuint);
        yield return new WaitForSeconds(GameStateSync.CardTransitionTime);
        // after the effect is shown it gets destroyed.
        Command.CommandExecutionComplete();
        Destroy(this.gameObject);
    }

    public static void CreateTextBubble(Vector3 position, int messageId)
    {
        // Instantiate a DamageEffect from prefab
        GameObject newTextBubble = GameObject.Instantiate(GameStateSync.Instance.TextBubblePrefab, position, Quaternion.identity) as GameObject;
        // Get DamageEffect component in this new game object
        DialogueEffect de = newTextBubble.GetComponent<DialogueEffect>();
        de.DialogueText.text = MessageText.messageMap[messageId];
        float waitTime = de.DialogueText.text.Length / 20;
        // start a coroutine to fade away and delete this effect after a certain time
        de.StartCoroutine(de.ShowTextBubble(waitTime));
    }
}
