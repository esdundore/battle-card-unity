using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;
using System;
using DG.Tweening;

/// <summary>
/// This class will show damage dealt to creatures or payers
/// </summary>

public class DamageEffect : MonoBehaviour {

    // an array of sprites with different blood splash graphics
    public Sprite[] Splashes;

    // a UI image to show the blood splashes
    public Image DamageImage;

    // CanvasGropup should be attached to the Canvas of this damage effect
    // It is used to fade away the alpha value of this effect
    public CanvasGroup cg;

    // The text component to show the amount of damage taken by target like: "-2"
    public TMP_Text AmountText;

    void Awake()
    {
        // pick a random image
        DamageImage.sprite = Splashes[UnityEngine.Random.Range(0, Splashes.Length)];  
    }

    // A Coroutine to control the fading of this damage effect
    private IEnumerator ShowDamageEffect()
    {
        // make this effect non-transparent
        cg.alpha = 1f;
        // wait for 1 second before fading
        yield return new WaitForSeconds(1f);
        // gradually fade the effect by changing its alpha value
        while (cg.alpha > 0)
        {
            cg.alpha -= 0.05f;
            yield return new WaitForSeconds(0.05f);
        }
        // after the effect is shown it gets destroyed.
        Destroy(this.gameObject);
    }

    private IEnumerator FloatNumberEffect()
    {
        DamageImage.gameObject.SetActive(false);
        AmountText.color = new Color32(31, 103, 163, 255);
        Sequence sequence = DOTween.Sequence();
        Vector3 destination = transform.position;
        destination.y += 1;
        sequence.Append(transform.DOMove(destination, 2f));

        // make this effect non-transparent
        cg.alpha = 1f;
        yield return new WaitForSeconds(1f);
        // gradually fade the effect by changing its alpha value
        while (cg.alpha > 0)
        {
            cg.alpha -= 0.05f;
            yield return new WaitForSeconds(0.05f);
        }
        // after the effect is shown it gets destroyed.
        Destroy(this.gameObject);
    }

    public static void CreateDamageEffect(Vector3 position, int amount, Color32 color, bool isDamage)
    {
        // Instantiate a DamageEffect from prefab
        GameObject newDamageEffect = GameObject.Instantiate(GameStateSync.Instance.DamageEffectPrefab, position, Quaternion.identity) as GameObject;
        // Get DamageEffect component in this new game object
        DamageEffect de = newDamageEffect.GetComponent<DamageEffect>();
        de.DamageImage.color = color;
        // Change the amount text to reflect the amount of damage dealt
        string symbol = "+";
        if (amount > 0) symbol = "-";
        de.AmountText.text = symbol + Math.Abs(amount).ToString();
        // start a coroutine to fade away and delete this effect after a certain time
        if (isDamage) de.StartCoroutine(de.ShowDamageEffect());
        else de.StartCoroutine(de.FloatNumberEffect());
    }
}
