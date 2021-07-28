using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CardAreaVisual : MonoBehaviour
{
    public SameDistanceChildren slots;

    private static readonly string RESOURCE_PATH_SKILL = "Data/Skill Cards/";
    private static readonly string RESOURCE_PATH_BREEDER = "Data/Breeders/";
    private static readonly string RESOURCE_PATH_MONSTER = "Data/Monster Cards/";
    private static readonly Quaternion FACE_UP = Quaternion.Euler(new Vector3(0f, 0f, 0f));
    private static readonly Quaternion FACE_DOWN = Quaternion.Euler(new Vector3(0f, -180f, 0f));

    public GameObject FindCard(int slotIndex)
    {
        // Find a card at the indexed slot
        if (slots.Children[slotIndex].childCount > 0)
            return slots.Children[slotIndex].GetChild(0).gameObject;
        else
            return null;
    }
    public Vector3 FindSlotPosition(int slotIndex)
    {
        return slots.Children[slotIndex].transform.position;
    }

    public void HighlightCard(int slotIndex, Color32 highlightColor, bool isEnabled)
    {
        // Highlight the card at indexed slot
        GameObject card = FindCard(slotIndex);
        if (card != null)
        {
            HighlightManager highlightManager = card.GetComponent<HighlightManager>();
            if (highlightManager == null)
                highlightManager = card.GetComponentInChildren<HighlightManager>();
            highlightManager.GlowImage.color = highlightColor;
            highlightManager.setHighlighted(isEnabled);
        }
    }

    public void CreateDiscardMenu(List<string> discards)
    {
        // Create a new skill card at the origin location
        slots.Children = new Transform[discards.Count];
        int index = -1;
        foreach (string discard in discards)
        {
            index++;
            if (slots.Children[index] == null) slots.Children[index] = new GameObject().transform;
            GameObject card = GameObject.Instantiate(GameStateSync.Instance.SkillCardPrefab, slots.Children[index].transform.position, FACE_UP) as GameObject;
            SkillCardManager manager = card.GetComponent<SkillCardManager>();
            manager.SkillCardAsset = Resources.Load<SkillCardAsset>(RESOURCE_PATH_SKILL + discard) as SkillCardAsset;
            manager.ReadCardFromAsset();
            card.transform.SetParent(slots.Children[index]);
            card.GetComponent<WhereIs>().slot = index;
            card.GetComponent<WhereIs>().SetHandSortingOrder();
        }
        ((RealSameDistanceChildren)slots).Resize();
    }
    public void CreateSubMonsterMenu(List<Monster> subMonsters)
    {
        // Create a new skill card at the origin location
        slots.Children = new Transform[subMonsters.Count];
        int index = -1;
        foreach (Monster monster in subMonsters)
        {
            index++;
            if (slots.Children[index] == null) slots.Children[index] = new GameObject().transform;
            GameObject card = GameObject.Instantiate(GameStateSync.Instance.MonsterCardPrefab, slots.Children[index].transform.position, FACE_UP) as GameObject;
            MonsterCardManager manager = card.GetComponent<MonsterCardManager>();
            manager.CopyAsset(Resources.Load<MonsterCardAsset>(RESOURCE_PATH_MONSTER + monster.name) as MonsterCardAsset);
            manager.ReadCardFromAsset();
            card.transform.SetParent(slots.Children[index]);
            card.GetComponent<WhereIs>().slot = index;
            card.GetComponent<WhereIs>().SetHandSortingOrder();
        }
    ((RealSameDistanceChildren)slots).Resize();
    }
    public void DestroySubMenu()
    {
        // Create a new skill card at the origin location
        foreach (Transform slot in slots.Children)
        {
            if (slot != null)
            {
                if (slot.childCount > 0) Destroy(slot.GetChild(0).gameObject);
                Destroy(slot.gameObject);
            }
        }
    }

    public GameObject CreateSkillCard(int slotIndex, SkillCard skillCard, Vector3 origin)
    {
        // Create a new skill card at the origin location
        GameObject card = null;
        try
        {
            card = GameObject.Instantiate(GameStateSync.Instance.SkillCardPrefab, origin, FACE_DOWN) as GameObject;
        }
        catch
        {
            card = GameObject.Instantiate(LibraryLoad.Instance.SkillCardPrefab, origin, FACE_UP) as GameObject;
        }
        SkillCardManager manager = card.GetComponent<SkillCardManager>();
        manager.SkillCardAsset = Resources.Load<SkillCardAsset>(RESOURCE_PATH_SKILL + skillCard.name) as SkillCardAsset;
        if (slotIndex != -1)
        {
            card.transform.SetParent(slots.Children[slotIndex]);
            card.GetComponent<WhereIs>().slot = slotIndex;
        }
        return UpdateSkillCard(card, skillCard);
    }
    public GameObject UpdateSkillCard(int slotIndex, SkillCard skillCard)
    {
        // Update skill card details on existing card
        GameObject card = FindCard(slotIndex);
        SkillCardManager manager = card.GetComponent<SkillCardManager>();
        manager.SkillCardAsset.currentGutsCost = skillCard.gutsCost;
        manager.SkillCardAsset.currentDamage = skillCard.damage;
        manager.ReadCardFromAsset();
        if (card.transform.rotation == FACE_DOWN && !skillCard.name.Equals(GameStateSync.BREEDER_HIDDEN))
        {
            manager.SkillCardAsset = Resources.Load<SkillCardAsset>(RESOURCE_PATH_SKILL + skillCard.name) as SkillCardAsset;
            manager.SkillCardAsset.currentGutsCost = skillCard.gutsCost;
            manager.SkillCardAsset.currentDamage = skillCard.damage;
            manager.ReadCardFromAsset();
            Sequence sequence = DOTween.Sequence();
            sequence.Append(card.transform.DORotate(Vector3.zero, GameStateSync.CardTransitionTime));
        }
        return card;
    }
    public GameObject UpdateSkillCard(GameObject card, SkillCard skillCard)
    {
        // Update skill card details on existing card
        SkillCardManager manager = card.GetComponent<SkillCardManager>();
        manager.SkillCardAsset.currentGutsCost = skillCard.gutsCost;
        manager.SkillCardAsset.currentDamage = skillCard.damage;
        manager.ReadCardFromAsset();
        if (card.transform.rotation == FACE_DOWN && !skillCard.name.Equals(GameStateSync.BREEDER_HIDDEN))
        {
            manager.SkillCardAsset = Resources.Load<SkillCardAsset>(RESOURCE_PATH_SKILL + skillCard.name) as SkillCardAsset;
            manager.SkillCardAsset.currentGutsCost = skillCard.gutsCost;
            manager.SkillCardAsset.currentDamage = skillCard.damage;
            manager.ReadCardFromAsset();
            Sequence sequence = DOTween.Sequence();
            sequence.Append(card.transform.DORotate(Vector3.zero, GameStateSync.CardTransitionTime));
        }
        return card;
    }

    public GameObject CreateMonsterCard(int slotIndex, Monster monster, Vector3 origin)
    {
        // Create a new monster card at the indexed slot
        GameObject card = GameObject.Instantiate(GameStateSync.Instance.MonsterCardPrefab, origin, FACE_DOWN) as GameObject;
        card.transform.SetParent(slots.Children[slotIndex]);
        card.GetComponent<WhereIs>().slot = slotIndex;
        MonsterCardManager manager = card.GetComponent<MonsterCardManager>();
        manager.CopyAsset(Resources.Load<MonsterCardAsset>(RESOURCE_PATH_MONSTER + monster.name) as MonsterCardAsset);
        return UpdateMonsterCard(slotIndex, monster);
    }
    public GameObject UpdateMonsterCard(int slotIndex, Monster monster)
    {
        // Update monster card details on existing card
        GameObject card = FindCard(slotIndex);
        MonsterCardManager manager = card.GetComponent<MonsterCardManager>();
        if (!monster.name.Equals(manager.MonsterAsset.title))
        {
            manager.CopyAsset(Resources.Load<MonsterCardAsset>(RESOURCE_PATH_MONSTER + monster.name) as MonsterCardAsset);
            manager.ReadCardFromAsset();
        }
        int damage = manager.MonsterAsset.currentLife - monster.currentLife;
        if (damage > 0)
            DamageEffect.CreateDamageEffect(card.transform.position, damage, GameStateSync.Instance.dmgColor, true);
        else if (damage < 0)
            DamageEffect.CreateDamageEffect(card.transform.position, damage, GameStateSync.Instance.healColor, true);
        manager.MonsterAsset.currentLife = monster.currentLife;
        manager.MonsterAsset.currentType = monster.monsterType;
        manager.MonsterAsset.tempDamage = monster.tempDamage;
        manager.MonsterAsset.statuses = monster.statuses;
        if (monster.canAttack)
            manager.MonsterAsset.statuses.Add("ATTACK");
        manager.ReadCardFromAsset();
        return card;
    }

    public GameObject CreateBreeder(int slotIndex, Breeder breeder, Deck deck)
    {
        // Create a new breeder icon at the indexed slot
        GameObject card = GameObject.Instantiate(GameStateSync.Instance.BreederPrefab, slots.Children[slotIndex].position, FACE_UP) as GameObject;
        card.transform.SetParent(slots.Children[slotIndex]);
        card.GetComponent<WhereIs>().slot = slotIndex;
        BreederManager manager = card.GetComponent<BreederManager>();
        manager.CopyAsset(Resources.Load<BreederAsset>(RESOURCE_PATH_BREEDER + deck.deckAvatar) as BreederAsset);
        return UpdateBreeder(slotIndex, breeder, deck);
    }
    public GameObject UpdateBreeder(int slotIndex, Breeder breeder, Deck deck)
    {
        // Load breeder details into the card
        GameObject card = FindCard(slotIndex);
        BreederManager manager = card.GetComponent<BreederManager>();
        int damage = manager.BreederAsset.guts - breeder.guts;
        if (damage != 0)
            DamageEffect.CreateDamageEffect(manager.GutsImage.transform.position, damage, GameStateSync.Instance.gutsColor, false);
        manager.BreederAsset.title = breeder.playerName;
        manager.BreederAsset.deckTitle = deck.deckName;
        manager.BreederAsset.guts = breeder.guts;
        manager.BreederAsset.statuses = breeder.statuses;
        if (breeder.canAttack)
            manager.BreederAsset.statuses.Add("ATTACK");
        manager.ReadBreederFromAsset();
        return card;
    }

    public void SequenceMoveCard(int slotIndex, bool reveal)
    {
        SequenceMoveCard(slotIndex, FindSlotPosition(slotIndex), reveal);
    }
    public void SequenceMoveCard(int slotIndex, Vector3 position, bool reveal)
    {
        GameObject card = FindCard(slotIndex);
        WhereIs whereIsCard = card.GetComponent<WhereIs>();
        whereIsCard.BringToFront();
        card.GetComponent<AudioSource>().Play();
        Sequence sequence = DOTween.Sequence();
        //Vector3 slotPosition = slots.Children[0].transform.localPosition;
        //slotPosition.z = card.transform.position.z;
        sequence.Append(card.transform.DOMove(position, GameStateSync.CardTransitionTime));
        if (card.transform.rotation == FACE_DOWN && reveal)
            sequence.Insert(0f, card.transform.DORotate(Vector3.zero, GameStateSync.CardTransitionTime));
        sequence.OnComplete(() => CompleteSequenceMoveCard(slotIndex));
    }
    public void CompleteSequenceMoveCard(int slotIndex)
    {
        GameObject card = FindCard(slotIndex);
        WhereIs whereIsCard = card.GetComponent<WhereIs>();
        whereIsCard.SendToBack();
        Command.CommandExecutionComplete();
    }
    public void SequenceMoveCard(GameObject card, Vector3 position, bool reveal)
    {
        card.GetComponent<AudioSource>().Play();
        Sequence sequence = DOTween.Sequence();
        //Vector3 slotPosition = slots.Children[0].transform.localPosition;
        //slotPosition.z = card.transform.position.z;
        sequence.Append(card.transform.DOMove(position, GameStateSync.CardTransitionTime));
        if (card.transform.rotation == FACE_DOWN && reveal)
            sequence.Insert(0f, card.transform.DORotate(Vector3.zero, GameStateSync.CardTransitionTime));
        sequence.Append(card.transform.DOMove(position, GameStateSync.CardTransitionTime));
        sequence.OnComplete(() => CompleteSequenceMoveCard(card));
    }
    public void CompleteSequenceMoveCard(GameObject card)
    {
        Destroy(card);
        Command.CommandExecutionComplete();
    }


    public void SequenceAbsorbCards(List<int> slotIndexes, Vector3 destination)
    {
        Sequence sequence = DOTween.Sequence();
        foreach (int slotIndex in slotIndexes)
        {
            GameObject card = FindCard(slotIndex);
            if (card != null)
            {
                card.transform.parent = null;
                WhereIs whereIsCard = card.GetComponent<WhereIs>();
                whereIsCard.BringToFront();
                if (card.activeSelf)
                    card.GetComponent<AudioSource>().Play();
                sequence.Insert(0f, card.transform.DOScale(0, GameStateSync.CardTransitionTime).SetEase(Ease.InSine));
                sequence.Insert(0f, card.transform.DOMove(destination, GameStateSync.CardTransitionTime).SetEase(Ease.OutSine));
            }
        }
        sequence.OnComplete(() => CompleteSequenceAbsorbCards(slotIndexes));
    }
    public void CompleteSequenceAbsorbCards(List<int> slotIndexes)
    {
        foreach (int slotIndex in slotIndexes)
        {
            GameObject card = FindCard(slotIndex);
            if (card != null)
                Destroy(card);
        }
        Command.CommandExecutionComplete();
    }
    public void DestroyCard(int i)
    {
        GameObject card = FindCard(i);
        if (card != null)
            Destroy(card);
    }

}