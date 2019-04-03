using UnityEngine;
using UnityEngine.UI.Extensions;

public class BezierSurfaceNet
{
    public class BezierSurfacePatch
    {
        public BezierCurve[] bz = new BezierCurve[4];

        public BezierSurfacePatch next;


        public BezierSurfacePatch(BezierCurve b1, BezierCurve b2, BezierCurve b3, BezierCurve b4)
        {
            bz[0] = b1;
            bz[1] = b2;
            bz[2] = b3;
            bz[3] = b4;

            next = null;
        }
    }

    BezierSurfacePatch first;

    public BezierSurfaceNet(BezierCurve b1, BezierCurve b2, BezierCurve b3, BezierCurve b4)
    {
        first = new BezierSurfacePatch(b1, b2, b3, b4);
    }

    public void Display(UILineRenderer[] line, UIRectangle rects, float w, float h)
    {
        BezierSurfacePatch tmp = first;

        float[,] Coeffs = new float[2, 4];
        float[,,] Params = new float[4, 2, 4];

        float n = 50, s, ns = 50, ds = 1.0f / (ns - 1), i, j, temp, temp1, ox, oy;
        int ia, ib, ic;

        rects.Primitives.Clear();
        rects.Invalidate();

        while (tmp != null)
        {
            for (ia = 0; ia < 4; ia++) tmp.bz[ia].Display(line[ia]);

            for (i = 0, s = 0; i < ns; i++, s += ds)
            {
                temp = s * s;
                temp1 = s * temp;

                for (ia = 0; ia < 4; ia++)
                    for (ib = 0; ib < 2; ib++)
                        for (ic = 0; ic < 4; ic++) Params[ia,ib,ic] = tmp.bz[ia].first.Params[ib,ic];

                Coeffs[0,0] = -Params[0,0,0] + 3 * Params[1,0,0] - 3 * Params[2,0,0] + Params[3,0,0];
                Coeffs[0,1] = 3 * Params[0,0,0] - 6 * Params[1,0,0] + 3 * Params[2,0,0];
                Coeffs[0,2] = -3 * Params[0,0,0] + 3 * Params[1,0,0];
                Coeffs[0,3] = Params[0,0,0];
                Coeffs[1,0] = -Params[0,1,0] + 3 * Params[1,1,0] - 3 * Params[2,1,0] + Params[3,1,0];
                Coeffs[1,1] = 3 * Params[0,1,0] - 6 * Params[1,1,0] + 3 * Params[2,1,0];
                Coeffs[1,2] = -3 * Params[0,1,0] + 3 * Params[1,1,0];
                Coeffs[1,3] = Params[0,1,0];

                for (j = 1; j < n; j++)
                {
                    ox = Coeffs[0,0] * temp1 + Coeffs[0,1] * temp + Coeffs[0,2] * s + Coeffs[0,3];
                    oy = Coeffs[1,0] * temp1 + Coeffs[1,1] * temp + Coeffs[1,2] * s + Coeffs[1,3];

                    rects.AddCellToDraw(new CustomRect()
                    {
                        anchorMin = new Vector2(ox / w, oy / h),
                        anchorMax = new Vector2((ox + 3) / w, (oy + 3) / h)
                    });
                    
                    for (ia = 0; ia < 4; ia++)
                        for (ib = 0; ib < 2; ib++)
                            for (ic = 0; ic < 3; ic++) Params[ia,ib,ic] += Params[ia,ib,ic + 1];

                    Coeffs[0,0] = -Params[0,0,0] + 3 * Params[1,0,0] - 3 * Params[2,0,0] + Params[3,0,0];
                    Coeffs[0,1] = 3 * Params[0,0,0] - 6 * Params[1,0,0] + 3 * Params[2,0,0];
                    Coeffs[0,2] = -3 * Params[0,0,0] + 3 * Params[1,0,0];
                    Coeffs[0,3] = Params[0,0,0];
                    Coeffs[1,0] = -Params[0,1,0] + 3 * Params[1,1,0] - 3 * Params[2,1,0] + Params[3,1,0];
                    Coeffs[1,1] = 3 * Params[0,1,0] - 6 * Params[1,1,0] + 3 * Params[2,1,0];
                    Coeffs[1,2] = -3 * Params[0,1,0] + 3 * Params[1,1,0];
                    Coeffs[1,3] = Params[0,1,0];
                }
            }

            for (ia = 0; ia < 4; ia++)
                for (ib = 0; ib < 2; ib++)
                    for (ic = 0; ic < 4; ic++) Params[ia,ib,ic] = tmp.bz[ia].first.Params[ib,ic];

            for (i = 0; i < n; i++)
            {
                Coeffs[0,0] = -Params[0,0,0] + 3 * Params[1,0,0] - 3 * Params[2,0,0] + Params[3,0,0];
                Coeffs[0,1] = 3 * Params[0,0,0] - 6 * Params[1,0,0] + 3 * Params[2,0,0];
                Coeffs[0,2] = -3 * Params[0,0,0] + 3 * Params[1,0,0];
                Coeffs[0,3] = Params[0,0,0];
                Coeffs[1,0] = -Params[0,1,0] + 3 * Params[1,1,0] - 3 * Params[2,1,0] + Params[3,1,0];
                Coeffs[1,1] = 3 * Params[0,1,0] - 6 * Params[1,1,0] + 3 * Params[2,1,0];
                Coeffs[1,2] = -3 * Params[0,1,0] + 3 * Params[1,1,0];
                Coeffs[1,3] = Params[0,1,0];

                for (j = 1, s = 0; j < ns; j++, s += ds)
                {
                    temp = s * s;
                    temp1 = s * temp;
                    ox = Coeffs[0,0] * temp1 + Coeffs[0,1] * temp + Coeffs[0,2] * s + Coeffs[0,3];
                    oy = Coeffs[1,0] * temp1 + Coeffs[1,1] * temp + Coeffs[1,2] * s + Coeffs[1,3];

                    rects.AddCellToDraw(new CustomRect()
                    {
                        anchorMin = new Vector2(ox / w, oy / h),
                        anchorMax = new Vector2((ox + 3) / w, (oy + 3) / h)
                    });
                }

                for (ia = 0; ia < 4; ia++)
                    for (ib = 0; ib < 2; ib++)
                        for (ic = 0; ic < 3; ic++) Params[ia,ib,ic] += Params[ia,ib,ic + 1];
            }

            tmp = tmp.next;
        }
    }
}