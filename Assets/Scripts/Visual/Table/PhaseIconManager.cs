using UnityEngine;
using UnityEngine.UI;

public class PhaseIconManager : MonoBehaviour
{
    public GameObject AttackIcon;
    public GameObject DefenseIcon;
    public GameObject GutsIcon;
    public Image PhaseBG;

    public static PhaseIconManager Instance;

    void Awake()
    {
        Instance = this;
    }

    public void ShowPhaseIcon(string phase, bool isActive)
    {
        if (isActive)
        {
            Color tempColor = PhaseBG.color;
            tempColor.a = 1f;
            PhaseBG.color = tempColor;
        }
        else
        {
            Color tempColor = PhaseBG.color;
            tempColor.a = .25f;
            PhaseBG.color = tempColor;
        }
        AttackIcon.SetActive(false);
        DefenseIcon.SetActive(false);
        GutsIcon.SetActive(false);
        if (phase.Equals(nameof(Phases.ATTACK)))
            AttackIcon.SetActive(isActive);
        else if (phase.Equals(nameof(Phases.DEFENSE)))
            DefenseIcon.SetActive(isActive);
        else if (phase.Equals(nameof(Phases.GUTS)))
            GutsIcon.SetActive(isActive);
    }

}
