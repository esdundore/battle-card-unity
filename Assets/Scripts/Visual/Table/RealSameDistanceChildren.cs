using UnityEngine;

// place first and last elements in children array manually
// others will be placed automatically with equal distances between first and last elements
public class RealSameDistanceChildren : SameDistanceChildren
{

    // Use this for initialization
    void Awake()
    {
        Resize();
    }

    public void Resize()
    {
        Children[0].transform.position = new Vector3(-4.5f, Children.Length, 0);
        Children[Children.Length - 1].transform.position = new Vector3(-4.5f, -Children.Length, 0);
        Vector3 firstElementPos = Children[0].transform.position;
        Vector3 lastElementPos = Children[Children.Length - 1].transform.position;

        // dividing by Children.Length - 1 because for example: between 10 points that are 9 segments
        float XDist = (lastElementPos.x - firstElementPos.x) / (float)(Children.Length - 1);
        float YDist = (lastElementPos.y - firstElementPos.y) / (float)(Children.Length - 1);
        float ZDist = (lastElementPos.z - firstElementPos.z) / (float)(Children.Length - 1);

        Vector3 Dist = new Vector3(XDist, YDist, ZDist);

        for (int i = 1; i < Children.Length; i++)
        {
            Children[i].transform.position = Children[i - 1].transform.position + Dist;
        }
    }


}
