﻿using UnityEngine;

public enum Layers { Table, Monsters, Cards, Above }
public enum VisualStates { Transition, LowHand, TopHand, LowTable, TopTable, Dragging }

public class WhereIs : MonoBehaviour {

    // reference to a HoverPreview Component
    private HoverPreview hover;

    // reference to a canvas on this object to set sorting order
    private Canvas canvas;

    // a value for canvas sorting order when we want to show this object above everything
    private int TopSortingOrder = 500;

    // PROPERTIES
    public int slot = -1;

    private VisualStates state;
    public VisualStates VisualState
    {
        get{ return state; }  

        set
        {
            state = value;
            switch (state)
            {
                case VisualStates.LowTable:
                case VisualStates.TopTable:
                    hover.ThisPreviewEnabled = true;
                    break;
                case VisualStates.Transition:
                    hover.ThisPreviewEnabled = false;
                    break;
                case VisualStates.Dragging:
                    hover.ThisPreviewEnabled = false;
                    break;
            }
        }
    }

    void Awake()
    {
        hover = GetComponent<HoverPreview>();
        // for characters hover is attached to a child game object
        if (hover == null)
            hover = GetComponentInChildren<HoverPreview>();
        canvas = GetComponentInChildren<Canvas>();
    }

    public void BringToFront()
    {
        canvas.sortingOrder = TopSortingOrder;
        canvas.sortingLayerName = nameof(Layers.Above);
    }

    public void SendToBack()
    {
        canvas.sortingOrder = TopSortingOrder;
        canvas.sortingLayerName = nameof(Layers.Table);
    }

    // not setting sorting order inside of VisualStaes property because when the card is drawn, 
    // we want to set an index first and set the sorting order only when the card arrives to hand. 
    public void SetHandSortingOrder()
    {
        if (slot != -1)
            canvas.sortingOrder = HandSortingOrder(slot);
        canvas.sortingLayerName = nameof(Layers.Cards);
    }

    public void SetTableSortingOrder()
    {
        canvas.sortingOrder = 0;
        canvas.sortingLayerName = nameof(Layers.Monsters);
    }

    private int HandSortingOrder(int placeInHand)
    {
        return ((placeInHand + 1) * 10); 
    }


}
