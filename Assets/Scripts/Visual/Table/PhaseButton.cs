using UnityEngine;

public class PhaseButton : MonoBehaviour {

    public GameObject Front;
    public GameObject Back;

    private bool wasUsed = false;
    public bool WasUsedThisTurn
    { 
        get
        {
            return wasUsed;
        } 
        set
        {
            wasUsed = value;
            if (!wasUsed)
            {
                Front.SetActive(true);
                Back.SetActive(false);
            }
            else
            {
                Front.SetActive(false);
                Back.SetActive(true);
            }
        }
    }

    public void OnClick()
    {
        GameStateSync.Instance.TurnOffAllHighlights();
        string requestUrl = GameStateSync.VIEW_URL;
        if (GameStateSync.Instance.gameView.phase.Equals(nameof(Phases.GUTS))) requestUrl = GameStateSync.END_TURN_URL;
        else if (GameStateSync.Instance.gameView.phase.Equals(nameof(Phases.ATTACK))) requestUrl = GameStateSync.END_ATTACK_URL;
        else if (GameStateSync.Instance.gameView.phase.Equals(nameof(Phases.DEFENSE))) requestUrl = GameStateSync.END_DEFENSE_URL;
        StartCoroutine(GameStateSync.Instance.GameSyncRoutine(requestUrl, SceneParameters.PlayersRequest));
    }
}
