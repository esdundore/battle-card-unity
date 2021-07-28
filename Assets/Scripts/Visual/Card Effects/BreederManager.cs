using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class BreederManager : HighlightManager {

    public BreederAsset BreederAsset;
    [Header("Text Component References")]
    public TMP_Text TitleText;
    public TMP_Text DeckTitleText;
    public TMP_Text GutsText;
    [Header("Image References")]
    public Image TitleImage;
    public Image BreederImage;
    public Image GutsImage;
    public Image AttackIcon;
    public Image MeditatingIcon;
    public Image FrameImage;

    void Awake()
    {
        if (BreederAsset != null)
            ReadBreederFromAsset();
    }

    public void ReadBreederFromAsset()
    {
        TitleText.text = BreederAsset.title;
        DeckTitleText.text = BreederAsset.deckTitle;
        GutsText.text = BreederAsset.guts.ToString();
        BreederImage.sprite = BreederAsset.image;
        GutsImage.color = GameStateSync.Instance.gutsColor;

        AttackIcon.gameObject.SetActive(false);
        MeditatingIcon.gameObject.SetActive(false);

        if (BreederAsset.statuses.Contains("ATTACK"))
            AttackIcon.gameObject.SetActive(true);
        if (BreederAsset.statuses.Contains("MEDITATING"))
            MeditatingIcon.gameObject.SetActive(true);
    }

    public void CopyAsset(BreederAsset breederAsset)
    {
        BreederAsset = (BreederAsset)MonsterCardAsset.CreateInstance("BreederAsset");
        BreederAsset.deckTitle = breederAsset.deckTitle;
        BreederAsset.guts = breederAsset.guts;
        BreederAsset.image = breederAsset.image;
        BreederAsset.title = breederAsset.title;
        BreederAsset.statuses = new List<string>();
    }

}
