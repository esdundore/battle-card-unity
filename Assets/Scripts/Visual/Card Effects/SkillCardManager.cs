using UnityEngine;
using UnityEngine.UI;
using TMPro;

// holds the refs to all the Text, Images on the card
public class SkillCardManager : HighlightManager {

    public SkillCardAsset SkillCardAsset;
    public SkillCardManager PreviewManager;
    [Header("Text Component References")]
    public TMP_Text TitleText;
    public TMP_Text TypeText;
    public TMP_Text DescriptionText;
    public TMP_Text GutsText;
    public TMP_Text DamageText;
    [Header("Image References")]
    public Image CardBodyImage;
    public Image CardTitleImage;
    public Image CardFrameImage;
    public Image CardGutsImage;
    public Image CardGraphicImage;
    public Image CardTypeImage;
    public Image CardDamageImage;

    void Awake()
    {
        if (SkillCardAsset != null)
            ReadCardFromAsset();
    }

    public void ReadCardFromAsset()
    {
        TitleText.text = SkillCardAsset.title;
        TypeText.text = SkillCardAsset.userAsset.title + " - " + SkillCardAsset.type;
        DescriptionText.text = SkillCardAsset.description;
        GutsText.text = SkillCardAsset.currentGutsCost.ToString();
        DamageText.text = SkillCardAsset.currentDamage.ToString();

        CardGraphicImage.sprite = SkillCardAsset.image;
        if (SkillCardAsset.userAsset != null)
        {
            CardBodyImage.color = SkillCardAsset.userAsset.cardColor;
            CardFrameImage.color = SkillCardAsset.userAsset.cardColor;
            CardTitleImage.color = SkillCardAsset.userAsset.iconColor;
            CardTypeImage.color = SkillCardAsset.userAsset.iconColor;
            CardGutsImage.color = GameStateSync.Instance.gutsColor;
            CardDamageImage.color = GameStateSync.Instance.dmgColor;
        }

        if (!SkillCardAsset.type.Equals("POW") && !SkillCardAsset.type.Equals("INT"))
        {
            CardDamageImage.enabled = false;
            DamageText.enabled = false;
        }
        else if (SkillCardAsset.type.Equals("POW"))
        {
            CardDamageImage.color = GameStateSync.Instance.powColor;
        }
        else if (SkillCardAsset.type.Equals("INT"))
        {
            CardDamageImage.color = GameStateSync.Instance.intColor;
        }

        if (SkillCardAsset.currentGutsCost != SkillCardAsset.gutsCost)
            GutsText.color = GameStateSync.Instance.modColor;
        else
            GutsText.color = Color.white;

        if (SkillCardAsset.currentDamage != SkillCardAsset.damage)
            DamageText.color = GameStateSync.Instance.modColor;
        else
            DamageText.color = Color.white;

        if (PreviewManager != null)
        {
            PreviewManager.SkillCardAsset = SkillCardAsset;
            PreviewManager.ReadCardFromAsset();
        }
    }
}
