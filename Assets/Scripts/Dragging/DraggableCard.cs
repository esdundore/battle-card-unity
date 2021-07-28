using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;

public class DraggableCard : DraggingActions
{
    private WhereIs whereIsCard;
    private VisualStates originalState;

    void Awake()
    {
        whereIsCard = GetComponent<WhereIs>();
    }

    public override void OnStartDrag()
    {
        originalState = whereIsCard.VisualState;
        whereIsCard.VisualState = VisualStates.Dragging;
        whereIsCard.BringToFront();

        GameStateSync.Instance.HighlightPlayableUsers(whereIsCard.slot);

    }

    public override void OnDraggingInUpdate() { }

    public override void OnEndDrag()
    {
        // determine if over a valid target
        GameObject target = null;
        RaycastHit[] hits;
        hits = Physics.RaycastAll(origin: Camera.main.transform.position,
            direction: (-Camera.main.transform.position + this.transform.position).normalized,
            maxDistance: 30f);

        foreach (RaycastHit hit in hits)
        {
            GameObject gameObject = hit.transform.parent.gameObject;
            HighlightManager highlightManager = gameObject.GetComponent<HighlightManager>();
            if ((highlightManager != null && highlightManager.isHighlighted()))
            {
                target = gameObject;
                break;
            }
        }

        GameStateSync.Instance.TurnOffAllHighlights();
        if (target != null)
        {
            // target monster index 
            SkillRequest skillRequest = new SkillRequest(SceneParameters.PlayersRequest) { handIndex = whereIsCard.slot, user = target.GetComponent<WhereIs>().slot };
            string requestUrl = GameStateSync.VIEW_URL;
            if (GameStateSync.Instance.gameView.phase.Equals(nameof(Phases.GUTS)))
                requestUrl = GameStateSync.GUTS_URL;
            else if (GameStateSync.Instance.gameView.phase.Equals(nameof(Phases.ATTACK)))
                requestUrl = GameStateSync.ATTACK_URL;
            else if (GameStateSync.Instance.gameView.phase.Equals(nameof(Phases.DEFENSE)))
                requestUrl = GameStateSync.DEFEND_URL;
            StartCoroutine(GameStateSync.Instance.GameSyncRoutine(requestUrl, skillRequest));
        }
        else
        {
            // Set old sorting order 
            whereIsCard.SetHandSortingOrder();
            whereIsCard.VisualState = originalState;
            // Move this card back to its slot position
            CardAreaVisual PlayerHand = GameStateSync.Instance.playerAreaVisual.handVisual;
            Vector3 oldCardPos = PlayerHand.slots.Children[whereIsCard.slot].transform.position;
            transform.DOMove(oldCardPos, 1f);
            GameStateSync.Instance.HighlightPlayableCards();
            GameStateSync.Instance.HighlightAttacker();
        }

    }
}
