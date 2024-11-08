using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlexibleGridLayout : LayoutGroup
{
    public int rows;
    public int columns;
    public Vector2 cellSize;
    public Vector2 spacing;

    public override void CalculateLayoutInputHorizontal()
    {
        base.CalculateLayoutInputHorizontal();
        
        float sqrt = Mathf.Sqrt(rectChildren.Count);
        rows = Mathf.CeilToInt(sqrt);
        columns = Mathf.CeilToInt(sqrt);

        float parentWidth = rectTransform.rect.width;
        float parentHeight = rectTransform.rect.height;

        float cellWidth = parentWidth / (float) columns;
        float cellHeight = parentHeight / (float) rows;
        
        cellSize.x = cellWidth;
        cellSize.y = cellHeight;

        int columnCount = 0;
        int rowCount = 0;

        for (int i = 0; i < rectChildren.Count; i++)
        {
            rowCount = i / columns;
            columnCount = i % columns;

            var item = rectChildren[i];
            
            var xPos = (cellSize.x * columnCount);
            var yPos = (cellSize.y * rowCount);
            SetChildAlongAxis(item, 0, xPos, cellSize.x);
            SetChildAlongAxis(item, 1, yPos, cellSize.y);
        }
    }
    

    public override void CalculateLayoutInputVertical()
    {
        // No additional vertical input calculation needed
    }

    public override void SetLayoutHorizontal()
    {
       // SetCellsLayout();
    }

    public override void SetLayoutVertical()
    {
       // SetCellsLayout();
    }
}
    