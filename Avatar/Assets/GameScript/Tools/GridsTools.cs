using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GridsTool
{
    /// <summary>
    /// 位置模式
    /// </summary>
    public enum Grids_SortBy
    {
        LeftAndTop,
        LeftAndDown,
        RightAndTop,
        RightAndDown,
    }

    /// <summary>
    /// 排列模式
    /// </summary>
    public enum Grids_Direction
    {
        Vertical,
        Horizontal,
    }

    /// <summary>
    /// 格子計算工具
    /// </summary>
    public class GridsTools
    {
        /// <summary>
        /// 計算網格格子位置
        /// </summary>
        /// <param name="iCount">當前格子總數</param>
        /// <param name="vCanvas">區域總長寬</param>
        /// <param name="vArea">格子長寬</param>
        /// <param name="vPoint">初始格子位置</param>
        /// <param name="iMax">每行/列個數</param>
        /// <param name="SortEM">位置模式</param>
        /// <param name="DirEM">排列模式</param>
        /// <returns></returns>
        public static Vector2 f_CalculatePos(int iCount, Vector2 vCanvas, Vector2 vArea, Vector2 vPoint, int iMax, Grids_SortBy SortEM, Grids_Direction DirEM)
        {
            Vector2 vPos = Vector2.zero;
            float iCanvasRange = 0;

            //排列模式  Vertical為橫向排列   Horizontal為垂直排列
            switch (DirEM)
            {
                case Grids_Direction.Vertical:
                    iCanvasRange = vCanvas.x;
                    vPoint = new Vector2(((iCanvasRange - vArea.x * iMax) / (iMax + 1)) + vArea.x / 2, vPoint.y);
                    vPos = f_DeployGridsVal(iCount, iMax, vArea, vPoint, SortEM);
                    break;

                case Grids_Direction.Horizontal:
                    iCanvasRange = vCanvas.y;
                    vPoint = new Vector2(vPoint.x, ((iCanvasRange - vArea.y * iMax) / (iMax + 1)) + vArea.y / 2);
                    vPos = f_DeployGridsHal(iCount, iMax, vArea, vPoint, SortEM);
                    break;
            }

            return vPos;
        }

        /// <summary>
        /// 計算單排格子位置
        /// </summary>
        /// <param name="iCount">當前格子總數</param>
        /// <param name="vArea">格子長寬</param>
        /// <param name="vPoint">初始格子位置</param>
        /// <param name="fDistance">間隔</param>
        /// <param name="DirEM">排列模式</param>
        /// <returns></returns>
        public static Vector2 f_CalculatePos(int iCount, Vector2 vArea, Vector2 vPoint, float fDistance, Grids_Direction DirEM)
        {
            Vector2 vPos = Vector2.zero;

            switch (DirEM)
            {
                case Grids_Direction.Vertical:
                    vPos = vPoint + new Vector2(0, (fDistance + vArea.y / 2) * iCount);
                    break;

                case Grids_Direction.Horizontal:
                    vPos = vPoint + new Vector2((fDistance + vArea.x / 2) * iCount, 0);
                    break;
            }

            return vPos;
        }

        /// <summary>計算橫向排列</summary>
        private static Vector2 f_DeployGridsVal(int iCount, int iMax, Vector2 vArea, Vector2 vPoint, Grids_SortBy SortEM)
        {
            Vector2 vPos = Vector2.zero;
            int iLine = iCount % iMax;
            int iRow = iCount / iMax;

            //位置模式  LeftAndTop(左上至右下排序)     LeftAndDown(左下至右上排序)    RightAndTop(右上至左下排序)    RightAndDown(右下至左上排序)
            switch (SortEM)
            {
                case Grids_SortBy.LeftAndTop:
                    vPos = new Vector2(vPoint.x + (vPoint.x + vArea.x / 2) * iLine, vPoint.y - (Mathf.Abs(vPoint.y) + vArea.y / 2) * iRow);
                    break;

                case Grids_SortBy.LeftAndDown:
                    vPos = new Vector2(vPoint.x + (vPoint.x + vArea.x / 2) * iLine, vPoint.y + (Mathf.Abs(vPoint.y) + vArea.y / 2) * iRow);
                    break;

                case Grids_SortBy.RightAndTop:
                    vPos = new Vector2(-vPoint.x - (vPoint.x + vArea.x / 2) * iLine, vPoint.y - (Mathf.Abs(vPoint.y) + vArea.y / 2) * iRow);
                    break;

                case Grids_SortBy.RightAndDown:
                    vPos = new Vector2(-vPoint.x - (vPoint.x + vArea.x / 2) * iLine, vPoint.y + (Mathf.Abs(vPoint.y) + vArea.y / 2) * iRow);
                    break;
            }

            return vPos;
        }

        /// <summary>計算垂直排列</summary>
        private static Vector2 f_DeployGridsHal(int iCount, int iMax, Vector2 vArea, Vector2 vPoint, Grids_SortBy SortEM)
        {
            Vector2 vPos = Vector2.zero;
            int iLine = iCount / iMax;
            int iRow = iCount % iMax;

            //位置模式  LeftAndTop(左上至右下排序)     LeftAndDown(左下至右上排序)    RightAndTop(右上至左下排序)    RightAndDown(右下至左上排序)
            switch (SortEM)
            {
                case Grids_SortBy.LeftAndTop:
                    vPos = new Vector2(vPoint.x + (Mathf.Abs(vPoint.x) + vArea.y / 2) * iLine, -vPoint.y - (vPoint.y + vArea.y / 2) * iRow);
                    break;

                case Grids_SortBy.LeftAndDown:
                    vPos = new Vector2(vPoint.x + (Mathf.Abs(vPoint.x) + vArea.y / 2) * iLine, vPoint.y + (vPoint.y + vArea.y / 2) * iRow);
                    break;

                case Grids_SortBy.RightAndTop:
                    vPos = new Vector2(vPoint.x - (Mathf.Abs(vPoint.x) + vArea.y / 2) * iLine, -vPoint.y - (vPoint.y + vArea.y / 2) * iRow);
                    break;

                case Grids_SortBy.RightAndDown:
                    vPos = new Vector2(vPoint.x - (Mathf.Abs(vPoint.x) + vArea.y / 2) * iLine, vPoint.y + (vPoint.y + vArea.y / 2) * iRow);
                    break;
            }

            return vPos;
        }

        /// <summary>
        /// 計算格子群組總長度
        /// </summary>
        /// <param name="iCount">當前格子總數</param>
        /// <param name="iMax">每行/列個數</param>
        /// <param name="vPoint">初始格子位置</param>
        /// <param name="vArea">格子面積</param>
        /// <param name="SortEM">位置模式</param>
        /// <param name="DirEM">排列模式</param>
        /// <returns></returns>
        public static float f_CalculateGridsLength(int iCount, int iMax, Vector2 vPoint, Vector2 vArea, Grids_SortBy SortEM, Grids_Direction DirEM)
        {
            float fGridsLength = 0;
            float fweight = 0, fheight = 0;

            switch (SortEM)
            {
                case Grids_SortBy.LeftAndTop:
                    fweight = vPoint.x + vArea.x / 2;
                    fheight = vPoint.y - vArea.y / 2;
                    break;

                case Grids_SortBy.LeftAndDown:
                    fweight = vPoint.x + vArea.x / 2;
                    fheight = vPoint.y + vArea.y / 2;
                    break;

                case Grids_SortBy.RightAndTop:
                    fweight = vPoint.x - vArea.x / 2;
                    fheight = vPoint.y - vArea.y / 2;
                    break;

                case Grids_SortBy.RightAndDown:
                    fweight = vPoint.x - vArea.x / 2;
                    fheight = vPoint.y + vArea.y / 2;
                    break;
            }

            switch (DirEM)
            {
                case Grids_Direction.Vertical:
                    fGridsLength = fheight * (iCount / iMax + 1) + (vPoint.y - vArea.y / 2);
                    break;

                case Grids_Direction.Horizontal:
                    fGridsLength = fweight * (iCount / iMax + 1) + (vPoint.x - vArea.x / 2);
                    break;
            }

            return fGridsLength;
        }

        public static bool f_GridsExceedCanvas(Vector2 vGridsPos, RectTransform rCanvas, Grids_Direction DirEM, bool bMax)
        {
            switch (DirEM)
            {
                case Grids_Direction.Vertical:
                    if (vGridsPos.y > rCanvas.localPosition.y - rCanvas.sizeDelta.y / 2 && bMax) { return true; }
                    else if (vGridsPos.y < rCanvas.localPosition.y + rCanvas.sizeDelta.y / 2 && !bMax) { return true; }
                    break;

                case Grids_Direction.Horizontal:
                    if (vGridsPos.x < rCanvas.localPosition.x + rCanvas.sizeDelta.x / 2 && bMax) { return true; }
                    else if (vGridsPos.x > rCanvas.localPosition.x - rCanvas.sizeDelta.x / 2 && !bMax) { return true; }
                    break;
            }
            return false;
        }
    }
}
