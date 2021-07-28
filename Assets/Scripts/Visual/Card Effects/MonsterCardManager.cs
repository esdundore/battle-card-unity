using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class MonsterCardManager : HighlightManager {

    public MonsterCardAsset MonsterAsset;
    public MonsterCardManager PreviewManager;
    [Header("Text Component References")]
    public TMP_Text TitleText;
    public TMP_Text TypeText;
    public TMP_Text LifeText;
    public TMP_Text TempDamageText;
    [Header("Image References")]
    public Image CardTitleImage;
    public Image CardTypeImage;
    public Image CardImage;
    public Image CardLifeImage;
    public Image CardTempDamageImage;
    public Image CardFrameImage;
    [Header("Status Image References")]
    public Image BerserkIcon;
    public Image MultiIcon;
    public Image AttackIcon;
    public Image StunnedIcon;
    public Image DamageBoostIcon;
    public Image UntargetableIcon;
    public Image InvulnerableIcon;
    public Image TauntingIcon;
    public Image FocusIcon;
    public Image NoblockIcon;
    public Image NododgeIcon;
    public Image NogritIcon;
    public Image CocoonIcon;

    void Awake()
    {
        if (MonsterAsset != null)
            ReadCardFromAsset();
    }

    public void ReadCardFromAsset()
    {
        TitleText.text = MonsterAsset.title;
        TypeText.text = MonsterAsset.currentType.Substring(0,1);
        LifeText.text = MonsterAsset.currentLife.ToString() + "/" + MonsterAsset.life.ToString();
        TempDamageText.text = MonsterAsset.tempDamage.ToString();

        CardTitleImage.color = MonsterAsset.mainLineage.iconColor;
        CardTypeImage.color = MonsterAsset.mainLineage.iconColor;
        CardImage.sprite = MonsterAsset.image;
        CardFrameImage.color = MonsterAsset.mainLineage.cardColor;
        CardLifeImage.color = MonsterAsset.mainLineage.iconColor;

        if (MonsterAsset.tempDamage > 0)
            CardTempDamageImage.gameObject.SetActive(true);
        else
            CardTempDamageImage.gameObject.SetActive(false);

        BerserkIcon.gameObject.SetActive(false);
        MultiIcon.gameObject.SetActive(false);
        AttackIcon.gameObject.SetActive(false);
        StunnedIcon.gameObject.SetActive(false);
        DamageBoostIcon.gameObject.SetActive(false);
        UntargetableIcon.gameObject.SetActive(false);
        InvulnerableIcon.gameObject.SetActive(false);
        TauntingIcon.gameObject.SetActive(false);
        FocusIcon.gameObject.SetActive(false);
        NoblockIcon.gameObject.SetActive(false);
        NododgeIcon.gameObject.SetActive(false);
        NogritIcon.gameObject.SetActive(false);
        CocoonIcon.gameObject.SetActive(false);

        if (MonsterAsset.statuses.Contains("BERSERK"))
            BerserkIcon.gameObject.SetActive(true);
        else if (MonsterAsset.statuses.Contains("MULTI"))
            MultiIcon.gameObject.SetActive(true);
        else if (MonsterAsset.statuses.Contains("STUNNED"))
            StunnedIcon.gameObject.SetActive(true);
        else if (MonsterAsset.statuses.Contains("ATTACK"))
            AttackIcon.gameObject.SetActive(true);

        if (MonsterAsset.statuses.Contains("DMGx2") || MonsterAsset.statuses.Contains("POW2") || MonsterAsset.statuses.Contains("INTx2") || MonsterAsset.statuses.Contains("POWx3"))
            DamageBoostIcon.gameObject.SetActive(true);
        if (MonsterAsset.statuses.Contains("UNTARGETABLE"))
            UntargetableIcon.gameObject.SetActive(true);
        if (MonsterAsset.statuses.Contains("INVULNERABLE"))
            InvulnerableIcon.gameObject.SetActive(true);
        if (MonsterAsset.statuses.Contains("TAUNTING"))
            TauntingIcon.gameObject.SetActive(true);
        if (MonsterAsset.statuses.Contains("FOCUSPOWx2") || MonsterAsset.statuses.Contains("FOCUSINTx2"))
            FocusIcon.gameObject.SetActive(true);
        if (MonsterAsset.statuses.Contains("NOBLOCK"))
            NoblockIcon.gameObject.SetActive(true);
        if (MonsterAsset.statuses.Contains("NODODGE"))
            NododgeIcon.gameObject.SetActive(true);
        if (MonsterAsset.statuses.Contains("GRIT"))
            NogritIcon.gameObject.SetActive(true);
        if (MonsterAsset.statuses.Contains("COCOON"))
            CocoonIcon.gameObject.SetActive(true);

        if (MonsterAsset.currentLife == 0)
            CardImage.color = Color.gray;
        else
            CardImage.color = Color.white;
        if (MonsterAsset.currentType != MonsterAsset.type)
            TypeText.color = GameStateSync.Instance.modColor;
        else
            TypeText.color = Color.white;

        if (PreviewManager != null)
        {
            PreviewManager.MonsterAsset = MonsterAsset;
            PreviewManager.ReadCardFromAsset();
        }

    }

    public void CopyAsset(MonsterCardAsset monsterCardAsset)
    {
        MonsterAsset = (MonsterCardAsset) MonsterCardAsset.CreateInstance("MonsterCardAsset");
        MonsterAsset.image = monsterCardAsset.image;
        MonsterAsset.cardColor = monsterCardAsset.cardColor;
        MonsterAsset.iconColor = monsterCardAsset.iconColor;
        MonsterAsset.life = monsterCardAsset.life;
        MonsterAsset.currentLife = monsterCardAsset.currentLife;
        MonsterAsset.tempDamage = monsterCardAsset.tempDamage;
        MonsterAsset.title = monsterCardAsset.title;
        MonsterAsset.type = monsterCardAsset.type;
        MonsterAsset.currentType = monsterCardAsset.currentType;
        MonsterAsset.mainLineage = monsterCardAsset.mainLineage;
        MonsterAsset.subLineage = monsterCardAsset.subLineage;
        MonsterAsset.statuses = new List<string>();
    }

}
