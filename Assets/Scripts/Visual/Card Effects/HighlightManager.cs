using UnityEngine;
using UnityEngine.UI;

public class HighlightManager : MonoBehaviour
{

    public Image GlowImage;
    public int index;

    public bool isHighlighted()
    {
        return GlowImage.gameObject.activeSelf;
    }
    public void setHighlighted(bool highlighted)
    {
        GlowImage.gameObject.SetActive(highlighted);
    }

}
