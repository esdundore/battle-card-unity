using UnityEngine;
using System.Collections.Generic;

public class DraggableTarget : DraggingActions
{
    private SpriteRenderer sr;
    private LineRenderer lr;
    private Transform triangle;
    private SpriteRenderer triangleSR;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        lr = GetComponentInChildren<LineRenderer>();
        lr.sortingLayerName = nameof(Layers.Above);
        triangle = transform.Find("Triangle");
        triangleSR = triangle.GetComponent<SpriteRenderer>();
    }

    public override void OnStartDrag()
    {
        sr.enabled = true;
        lr.enabled = true;

        GameStateSync.Instance.TurnOffAllHighlights();
        GameStateSync.Instance.HighlightPlayableTargets();
    }

    public override void OnDraggingInUpdate()
    {
        // This code only draws the arrow
        Vector3 notNormalized = transform.position - transform.parent.position;
        Vector3 direction = notNormalized.normalized;
        float distanceToTarget = (direction*2.3f).magnitude;
        if (notNormalized.magnitude > distanceToTarget)
        {
            // draw a line between the creature and the target
            lr.SetPositions(new Vector3[]{ transform.parent.position, transform.position - direction*2.3f });
            lr.enabled = true;

            // position the end of the arrow between near the target.
            triangleSR.enabled = true;
            triangleSR.transform.position = transform.position - 1.5f*direction;

            // proper rotarion of arrow end
            float rot_z = Mathf.Atan2(notNormalized.y, notNormalized.x) * Mathf.Rad2Deg;
            triangleSR.transform.rotation = Quaternion.Euler(0f, 0f, rot_z - 90);
        }
        else
        {
            // if the target is not far enough from creature, do not show the arrow
            lr.enabled = false;
            triangleSR.enabled = false;
        }

    }

    public override void OnEndDrag()
    {
        GameObject target = null;
        RaycastHit[] hits;
        hits = Physics.RaycastAll(origin: Camera.main.transform.position,
            direction: (-Camera.main.transform.position + this.transform.position).normalized,
            maxDistance: 30f);

        foreach (RaycastHit hit in hits)
        {
            GameObject gameObject = hit.transform.parent.gameObject;
            HighlightManager highlightManager = gameObject.GetComponent<HighlightManager>();
            if (highlightManager == null) highlightManager = gameObject.GetComponentInChildren<HighlightManager>();
            if ((highlightManager != null && highlightManager.isHighlighted()))
            {
                target = gameObject;
                break;
            }
        }

        GameStateSync.Instance.TurnOffAllHighlights();
        GameStateSync.Instance.subMenuVisual.DestroySubMenu();

        if (target != null)
        {
            WhereIs whereIsCard = target.GetComponent<WhereIs>();
            if (whereIsCard == null) whereIsCard = target.GetComponentInChildren<WhereIs>();
            TargetRequest targetRequest = new TargetRequest(SceneParameters.PlayersRequest) { target = whereIsCard.slot };
            string requestUrl = GameStateSync.VIEW_URL;
            if (GameStateSync.Instance.gameView.phase.Equals(nameof(Phases.ATTACK)))
                requestUrl = GameStateSync.ATTACK_TARGET_URL;
            else if (GameStateSync.Instance.gameView.phase.Equals(nameof(Phases.DEFENSE)))
                requestUrl = GameStateSync.DEFENSE_TARGET_URL;
            StartCoroutine(GameStateSync.Instance.GameSyncRoutine(requestUrl, targetRequest));
        }
        else
        {
            GameStateSync.Instance.HighlightAttacker();
            GameStateSync.Instance.HighlightPlayableCards();
        }
        transform.localPosition = new Vector3(0f, 0f, 0.4f);
        sr.enabled = false;
        lr.enabled = false;
        triangleSR.enabled = false;

    }

}
