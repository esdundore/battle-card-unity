using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public enum Phases { GUTS, ATTACK, DEFENSE }

public class GameStateSync : MonoBehaviour
{
    public static GameStateSync Instance;
    public GameView gameView = new GameView();

    public static string HOST_NAME = "http://localhost:8080";
    //public static string HOST_NAME = "http://battlecard-env.eba-syzyw3ur.us-east-2.elasticbeanstalk.com";
    public static string VIEW_URL = "/get-view";
    public static string ATTACK_URL = "/attack";
    public static string ATTACK_TARGET_URL = "/attack-target";
    public static string END_ATTACK_URL = "/end-attack";
    public static string DEFEND_URL = "/defend";
    public static string DEFENSE_TARGET_URL = "/defense-target";
    public static string END_DEFENSE_URL = "/end-defense";
    public static string GUTS_URL = "/make-guts";
    public static string END_TURN_URL = "/end-turn";
    public static int BREEDER_INDEX = 3;
    public static string BREEDER_HIDDEN = "Breeder/Hidden";

    // GLOBAL SETTINGS
    [Header("Prefabs")]
    public GameObject SkillCardPrefab;
    public GameObject MonsterCardPrefab;
    public GameObject BreederPrefab;
    public GameObject DamageEffectPrefab;
    public GameObject TextBubblePrefab;
    [Header("Game Visuals")]
    public PlayerAreaVisual playerAreaVisual;
    public PlayerAreaVisual opponentAreaVisual;
    public CardAreaVisual skillAreaVisual;
    public CardAreaVisual subMenuVisual;
    public Transform environmentCard;
    [Header("Colors")]
    public Color32 gutsColor;
    public Color32 lifeColor;
    public Color32 powColor;
    public Color32 intColor;
    public Color32 dmgColor;
    public Color32 healColor;
    public Color32 modColor;
    [Header("Timing")]
    public static float CardTransitionTime = .3f;
    public static float MessageTime = 2f;
    public static int PingTime = 1;

    // Singleton
    private void Awake()
    {
        Instance = this;
    }

    // On object creation
    void Start()
    {
        StartCoroutine(GameStartRoutine(SceneParameters.SceneStartUrl, SceneParameters.PlayersRequest));
    }

    // Game flow including start, syncing visuals, and finish
    public IEnumerator GameStartRoutine(string url, PlayersRequest request)
    {
        // Start the match
        //string result = RESTTemplate.POSTRequest(HOST_NAME + url, JsonUtility.ToJson(request));
        //GameView newGameView = JsonUtility.FromJson<GameView>(result);
        CoroutineWithData postRequest = new CoroutineWithData(this, RESTTemplate.POSTRequestRoutine(HOST_NAME + url, JsonUtility.ToJson(request)));
        yield return postRequest.coroutine;
        GameView newGameView = JsonUtility.FromJson<GameView>((string)postRequest.result);

        PlayerArea playerArea = newGameView.playerArea;
        PlayerArea opponentArea = newGameView.opponentArea;

        // adjust cards in deck
        new UpdateDeckCommand(opponentAreaVisual.deckVisual, opponentArea.deck.deckSize + opponentArea.hand.Count).AddToQueue();
        new UpdateDeckCommand(playerAreaVisual.deckVisual, playerArea.deck.deckSize + playerArea.hand.Count).AddToQueue();

        // create breeder assets
        CardAreaVisual playerUsers = playerAreaVisual.usersVisual;
        CardAreaVisual opponentUsers = opponentAreaVisual.usersVisual;
        new CreateBreederCommand(opponentUsers, opponentArea.breeder, opponentArea.deck, BREEDER_INDEX).AddToQueue();
        new CreateBreederCommand(playerUsers, playerArea.breeder, playerArea.deck, BREEDER_INDEX).AddToQueue();

        // create all monster cards and flip
        for (int i = 0; i < opponentArea.monsters.Count; i++)
            new CreateMonsterCardCommand(opponentUsers, i, opponentArea.monsters[i], opponentUsers.slots.Children[i].position).AddToQueue();
        for (int i = 0; i < playerArea.monsters.Count; i++)
            new CreateMonsterCardCommand(playerUsers, i, playerArea.monsters[i], playerUsers.slots.Children[i].position).AddToQueue();
        for (int i = 0; i < opponentArea.monsters.Count; i++)
            new MoveCardCommand(opponentUsers, i, opponentUsers.slots.Children[i].position, true).AddToQueue();
        for (int i = 0; i < playerArea.monsters.Count; i++)
            new MoveCardCommand(playerUsers, i, playerUsers.slots.Children[i].position, true).AddToQueue();

        VisualSyncRoutine(newGameView);
        gameView = newGameView;

        if (!newGameView.currentPlayer.Equals(MainMenu.PLAYER))
        {
            StartCoroutine(GameSyncRoutine(VIEW_URL, request));
        }
        else
        {
            HighlightPlayableCards();
            HighlightAttacker();
            EnableContinueButton();
        }
    }

    public IEnumerator GameSyncRoutine(string url, PlayersRequest request)
    {
        // disable button
        new EnableButtonCommand(playerAreaVisual, false).AddToQueue();
        string currentPlayer = "";
        string requestJSON;
        if (request is SkillRequest skillRequest)
            requestJSON = JsonUtility.ToJson(skillRequest);
        else if (request is TargetRequest targetRequest)
            requestJSON = JsonUtility.ToJson(targetRequest);
        else
            requestJSON = JsonUtility.ToJson(request);
        while (!currentPlayer.Equals(MainMenu.PLAYER))
        {
            //string result = RESTTemplate.POSTRequest(HOST_NAME + url, requestJSON);
            //GameView newGameView = JsonUtility.FromJson<GameView>(result);
            CoroutineWithData postRequest = new CoroutineWithData(this, RESTTemplate.POSTRequestRoutine(HOST_NAME + url, JsonUtility.ToJson(request)));
            yield return postRequest.coroutine;
            GameView newGameView = JsonUtility.FromJson<GameView>((string)postRequest.result);
            url = VIEW_URL;
            PlayersRequest playersRequest = new PlayersRequest() { player1 = request.player1, player2 = request.player2 };
            requestJSON = JsonUtility.ToJson(playersRequest);

            VisualSyncRoutine(newGameView);
            gameView = newGameView;
            currentPlayer = gameView.currentPlayer;

            // TODO: why doesn't this work only certain times???
            //yield return new WaitForSeconds(PingTime);
        }

        // enable inputs
        HighlightPlayableCards();
        HighlightAttacker();
        EnableContinueButton();
    }

    // Sync game view with the server
    public void VisualSyncRoutine(GameView newGameView)
    {

        PlayerAreaVisual attackerAreaVisual = opponentAreaVisual;
        PlayerAreaVisual defenderAreaVisual = playerAreaVisual;

        if (newGameView.skillArea.attacker.Equals(MainMenu.PLAYER))
        {
            attackerAreaVisual = playerAreaVisual;
            defenderAreaVisual = opponentAreaVisual;
        }

        int currentMessageId = gameView.opponentArea.breeder.messageId;
        while (newGameView.opponentArea.breeder.messageId - currentMessageId > 1)
        {
            currentMessageId++;
            new ShowDialogueCommand(new Vector3(2, 8), currentMessageId).AddToQueue();
        }

        // move skills to the skill area
        SyncSkillArea(attackerAreaVisual, defenderAreaVisual, gameView.skillArea, newGameView.skillArea);
        // resolve attack
        ResolveAttack(attackerAreaVisual, gameView.skillArea, newGameView.skillArea);

        // resolve environment card
        SyncEnvCard(newGameView);

        // update breeders
        new UpdateBreederCommand(opponentAreaVisual.usersVisual, BREEDER_INDEX, newGameView.opponentArea.breeder, newGameView.opponentArea.deck).AddToQueue();
        new UpdateBreederCommand(playerAreaVisual.usersVisual, BREEDER_INDEX, newGameView.playerArea.breeder, newGameView.playerArea.deck).AddToQueue();

        // update monsters
        for (int i = 0; i < newGameView.opponentArea.monsters.Count; i++)
            new UpdateMonsterCommand(opponentAreaVisual.usersVisual, i, newGameView.opponentArea.monsters[i]).AddToQueue();
        for (int i = 0; i < newGameView.playerArea.monsters.Count; i++)
            new UpdateMonsterCommand(playerAreaVisual.usersVisual, i, newGameView.playerArea.monsters[i]).AddToQueue();

        // update hand
        if (newGameView.currentPlayer.Equals(MainMenu.PLAYER))
        {
            //opponent first
            SyncHandAndDeck(opponentAreaVisual, gameView.opponentArea, newGameView.opponentArea);
            SyncHandAndDeck(playerAreaVisual, gameView.playerArea, newGameView.playerArea);
        }
        else 
        {
            // player first
            SyncHandAndDeck(playerAreaVisual, gameView.playerArea, newGameView.playerArea);
            SyncHandAndDeck(opponentAreaVisual, gameView.opponentArea, newGameView.opponentArea);
        }

        if (newGameView.winner != "")
        {
            new ShowMessageCommand(newGameView.winner + " Wins!", 99999f).AddToQueue();
        }


        // update game phase and display turn message
        SyncPhase(newGameView);

        // update message and show if changed
        if (newGameView.opponentArea.breeder.messageId != gameView.opponentArea.breeder.messageId && newGameView.opponentArea.breeder.messageId != 0)
        {
            new ShowDialogueCommand(new Vector3(2, 8), newGameView.opponentArea.breeder.messageId).AddToQueue();
        }
    }

    public void ResolveAttack(PlayerAreaVisual attackerArea, SkillArea skillArea, SkillArea newSkillArea)
    {
        if (newSkillArea.attackId > 0 && newSkillArea.resolved && !skillArea.resolved)
        {
            Vector3 destination = attackerArea.usersVisual.FindSlotPosition(skillArea.attacks[0].user);
            List<int> slotIndexes = new List<int>();
            for (int i = 0; i < skillAreaVisual.slots.Children.Length; i++)
                slotIndexes.Add(i);
            new AbsorbCardsCommand(skillAreaVisual, slotIndexes, skillAreaVisual.FindSlotPosition(0)).AddToQueue();
            // attack animation
            new MoveCardCommand(attackerArea.usersVisual, skillArea.attacks[0].user, destination, false).AddToQueue();
        }
    }

    public void SyncSkillArea(PlayerAreaVisual attackerArea, PlayerAreaVisual defenderArea, SkillArea oldSkillArea, SkillArea newSkillArea)
    {
        Skill newSkill = new Skill() { skillCard = new SkillCard() };
        while (oldSkillArea.attacks.Count < newSkillArea.attacks.Count)
            oldSkillArea.attacks.Add(newSkill);
        while (oldSkillArea.defenses.Count < newSkillArea.defenses.Count)
            oldSkillArea.defenses.Add(newSkill);
        if (newSkillArea.attackId > 0)
        {
            if (!newSkillArea.resolved)
            {
                if (newSkillArea.attackId > oldSkillArea.attackId)
                {
                    // move the attacker
                    Vector3 destination = skillAreaVisual.slots.Children[0].transform.position;
                    new MoveCardCommand(attackerArea.usersVisual, newSkillArea.attacks[0].user, destination, false).AddToQueue();
                }
                for (int i = 0; i < newSkillArea.attacks.Count; i++)
                {
                    int offset = 1;
                    if (oldSkillArea.attacks[i].skillCard.id != newSkillArea.attacks[i].skillCard.id)
                    {
                        Vector3 origin = attackerArea.handVisual.FindSlotPosition(newSkillArea.attacks[i].handIndex);
                        new DestroyObjectCommand(attackerArea.handVisual, new List<int> { newSkillArea.attacks[i].handIndex }).AddToQueue();
                        if (newSkillArea.attacks[i].skillCard.name != null)
                        {
                            new CreateSkillCardCommand(skillAreaVisual, i + offset, newSkillArea.attacks[i].skillCard, origin).AddToQueue();
                            new MoveCardCommand(skillAreaVisual, i + offset, skillAreaVisual.slots.Children[i + offset].transform.position, true).AddToQueue();
                        }
                    }
                    else if (newSkillArea.attacks[i].skillCard.name != null)
                    {
                        new UpdateSkillCardCommand(skillAreaVisual, i + offset, newSkillArea.attacks[i].skillCard).AddToQueue();
                    }
                }
            }
            for (int i = 0; i < newSkillArea.defenses.Count; i++)
            {
                if (oldSkillArea.defenses[i].skillCard.id != newSkillArea.defenses[i].skillCard.id)
                {
                    Vector3 origin = defenderArea.handVisual.FindSlotPosition(newSkillArea.defenses[i].handIndex);
                    if (newSkillArea.defenses[i].skillCard.name != null)
                    {
                        new CreateSkillCardCommand(defenderArea.handVisual, -1, newSkillArea.defenses[i].skillCard, origin).AddToQueue();
                    }
                }
            }
        }
    }

    public void SyncEnvCard(GameView newGameView)
    {
        if (newGameView.environmentCard != "" && gameView.environmentCard == "")
        {
            GameObject card = GameObject.Instantiate(SkillCardPrefab, environmentCard.position, Quaternion.Euler(new Vector3(0f, 0f, 0f))) as GameObject;
            card.transform.SetParent(environmentCard);
        }
        if (gameView.environmentCard != newGameView.environmentCard)
        {
            GameObject card = environmentCard.GetChild(1).gameObject;
            SkillCardManager manager = card.GetComponent<SkillCardManager>();
            manager.SkillCardAsset = Resources.Load<SkillCardAsset>("Data/Skill Cards/" + newGameView.environmentCard) as SkillCardAsset;
            manager.ReadCardFromAsset();
        }
    }
    // Draw or discard cards from hand
    public void SyncHandAndDeck(PlayerAreaVisual playerAreaVisual, PlayerArea oldPlayerArea, PlayerArea newPlayerArea)
    {
        List<SkillCard> oldHand = oldPlayerArea.hand; 
        List<SkillCard> newHand = newPlayerArea.hand; 
        CardAreaVisual handArea = playerAreaVisual.handVisual;

        while (oldHand.Count < newHand.Count) oldHand.Add(new SkillCard());
        for (int i = 0; i < newHand.Count; i++)
        {
            if (oldHand[i].id != newHand[i].id)
            {
                //if (gameView.phase.Equals(nameof(Phases.GUTS)))
                    new AbsorbCardsCommand(handArea, new List<int> { i }, playerAreaVisual.usersVisual.FindSlotPosition(BREEDER_INDEX)).AddToQueue();
                //else if (handArea.FindCard(i) != null)
                    //handArea.DestroyCard(i);
            }
            else if (newHand[i].id != 0)
                new UpdateSkillCardCommand(handArea, i, newHand[i]).AddToQueue();
        }
        int cardsInDeck = playerAreaVisual.deckVisual.CardsInDeck;
        for (int i = 0; i < newHand.Count; i++)
        {
            if (oldHand[i].id != newHand[i].id && newHand[i].id != 0)
            {
                Vector3 origin = new Vector3(0, 0, 0);
                if (newPlayerArea.deck.deckSize < cardsInDeck)
                {
                    cardsInDeck--;
                    origin = playerAreaVisual.deckVisual.transform.position;
                    new UpdateDeckCommand(playerAreaVisual.deckVisual, cardsInDeck).AddToQueue();
                }
                new CreateSkillCardCommand(handArea, i, newHand[i], origin).AddToQueue();
                bool canSee = !newHand[i].name.Equals(BREEDER_HIDDEN);
                new MoveCardCommand(handArea, i, handArea.FindSlotPosition(i), canSee).AddToQueue();
            }
        }
        new UpdateDeckCommand(playerAreaVisual.deckVisual, newPlayerArea.deck.deckSize).AddToQueue();
    }

    public void SyncPhase(GameView newGameView)
    {
        // update game phase and display turn message
        if (!gameView.currentPlayer.Equals(newGameView.currentPlayer) || !gameView.phase.Equals(newGameView.phase))
        {
            new ShowPhaseIconCommand(playerAreaVisual, "", false).AddToQueue();
            new ShowPhaseIconCommand(opponentAreaVisual, "", false).AddToQueue();
            if (newGameView.currentPlayer.Equals(MainMenu.PLAYER))
                new ShowPhaseIconCommand(playerAreaVisual, newGameView.phase, true).AddToQueue();
            else
                new ShowPhaseIconCommand(opponentAreaVisual, newGameView.phase, true).AddToQueue();
            if (newGameView.phase.Equals(nameof(Phases.ATTACK)) && gameView.phase.Equals(nameof(Phases.GUTS)))
            {
                new ShowMessageCommand(newGameView.currentPlayer + "'s Turn", MessageTime).AddToQueue();
            }
            else if (newGameView.phase.Equals(nameof(Phases.ATTACK)) && gameView.phase.Equals(nameof(Phases.ATTACK)) && !gameView.currentPlayer.Equals(newGameView.currentPlayer))
            {
                new ShowMessageCommand(newGameView.currentPlayer + "'s Turn", MessageTime).AddToQueue();
            }
        }
    }


    public void HighlightPlayableCards()
    {
        List<int> playableCardIndexes = new List<int>();
        foreach (PlayableCard playableCard in gameView.playable.playableCards)
            playableCardIndexes.Add(playableCard.handIndex);
        new HighlightCommand(playerAreaVisual.handVisual, playableCardIndexes, playerAreaVisual.highlightColor, true).AddToQueue();
    }
    public void HighlightPlayableUsers(int cardIndex)
    {
        PlayableCard card = null;
        foreach (PlayableCard playableCard in gameView.playable.playableCards) {
            if (cardIndex == playableCard.handIndex)
            {
                card = playableCard;
                break;
            }
        }
        new HighlightCommand(playerAreaVisual.usersVisual, card.users, playerAreaVisual.highlightColor, true).AddToQueue();
    }
    public void HighlightAttacker()
    {
        if (gameView.playable.playableTargets.Count > 0)
        {
            int lastUser = 0;
            if (gameView.phase.Equals(nameof(Phases.ATTACK)))
                lastUser = gameView.skillArea.attacks[gameView.skillArea.attacks.Count - 1].user;
            else if (gameView.phase.Equals(nameof(Phases.DEFENSE)))
                lastUser = gameView.skillArea.defenses[gameView.skillArea.defenses.Count - 1].user;
            new HighlightCommand(playerAreaVisual.usersVisual, new List<int> { lastUser }, playerAreaVisual.highlightColor, true).AddToQueue();
        }
    }
    public void HighlightPlayableTargets()
    {
        Color32 color = Color.white;
        CardAreaVisual targetArea = null;
        if ("ENEMY".Equals(gameView.skillArea.targetArea) && gameView.phase.Equals(nameof(Phases.ATTACK)))
        {
            targetArea = opponentAreaVisual.usersVisual;
            color = opponentAreaVisual.highlightColor;
        }
        else if ("ALLY".Equals(gameView.skillArea.targetArea) || gameView.phase.Equals(nameof(Phases.DEFENSE)))
        {
            targetArea = playerAreaVisual.usersVisual;
            color = playerAreaVisual.highlightColor;
        }
        else if ("HAND".Equals(gameView.skillArea.targetArea))
        {
            targetArea = playerAreaVisual.handVisual;
            color = playerAreaVisual.highlightColor;
        }
        else if ("DISCARD".Equals(gameView.skillArea.targetArea))
        {
            // show discard sub menu
            subMenuVisual.CreateDiscardMenu(gameView.playerArea.discards);
            targetArea = subMenuVisual;
            color = playerAreaVisual.highlightColor;
        }
        else if ("SUBMONS".Equals(gameView.skillArea.targetArea))
        {
            // show sub monster menu
            subMenuVisual.CreateSubMonsterMenu(gameView.playerArea.subMonsters);
            targetArea = subMenuVisual;
            color = playerAreaVisual.highlightColor;
        }
        if (targetArea != null)
            new HighlightCommand(targetArea, gameView.playable.playableTargets, color, true).AddToQueue();
    }
    public void TurnOffAllHighlights()
    {
        List<int> userIndexes = new List<int>();
        for (int i = 0; i < opponentAreaVisual.usersVisual.slots.Children.Length; i++)
            userIndexes.Add(i);
        new HighlightCommand(opponentAreaVisual.usersVisual, userIndexes, Color.white, false).AddToQueue();
        List<int> handIndexes = new List<int>();
        for (int i = 0; i < opponentAreaVisual.handVisual.slots.Children.Length; i++)
            handIndexes.Add(i);
        new HighlightCommand(opponentAreaVisual.handVisual, handIndexes, Color.white, false).AddToQueue();
        PlayerAreaVisual playerArea = playerAreaVisual;
        userIndexes = new List<int>();
        for (int i = 0; i < playerArea.usersVisual.slots.Children.Length; i++)
            userIndexes.Add(i);
        new HighlightCommand(playerArea.usersVisual, userIndexes, Color.white, false).AddToQueue();
        handIndexes = new List<int>();
        for (int i = 0; i < playerArea.handVisual.slots.Children.Length; i++)
            handIndexes.Add(i);
        new HighlightCommand(playerArea.handVisual, handIndexes, Color.white, false).AddToQueue();
    }
    public void EnableContinueButton()
    {
        new EnableButtonCommand(playerAreaVisual, gameView.playable.playableContinue).AddToQueue();
    }

}
