﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

public class UIRectangle : UIPrimitiveBase
{
    private List<CustomRect> cellsToDraw = new List<CustomRect>();

    public List<CustomRect> Primitives
    {
        get { return cellsToDraw; }

        private set { }
    }

    Vector2 uv0 = new Vector2(0, 0);
    Vector2 uv1 = new Vector2(0, 1);
    Vector2 uv2 = new Vector2(1, 1);
    Vector2 uv3 = new Vector2(1, 0);
    Vector2 uv4 = new Vector2(0, 0);


    public void AddCellToDraw(CustomRect rc)
    {
        cellsToDraw.Add(rc);
    }


    public void Invalidate()
    {
        SetAllDirty();
    }


    protected override void OnPopulateMesh(VertexHelper vh)
    {
        vh.Clear();

        foreach (CustomRect rc in cellsToDraw)
        {
            DrawRect(vh, rc.anchorMin, rc.anchorMax, Color.white);
        }
    }

    private void DrawRect(VertexHelper vh, Vector2 min, Vector2 max, Color color)
    {
        // We assume that pivot is bottom left for us, to ease our calculations

        RectTransform rc = this.transform as RectTransform;

        Vector2 start = new Vector2(min.x * rc.rect.width, min.y * rc.rect.height);
        Vector2 end = new Vector2(max.x * rc.rect.width, max.y * rc.rect.height);

        Vector2 pos0 = new Vector2(start.x, start.y);
        Vector2 pos1 = new Vector2(start.x, end.y);
        Vector2 pos2 = new Vector2(end.x, end.y);
        Vector2 pos3 = new Vector2(end.x, start.y);
        Vector2 pos4 = new Vector2(start.x, start.y);


        // Draw the rectangle

        vh.AddUIVertexQuad(SetVbo(new[] { pos0, pos1, pos2, pos3, pos4 }, new[] { uv0, uv1, uv2, uv3, uv4 }, 5, color));
    }
}