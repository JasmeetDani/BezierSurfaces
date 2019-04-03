using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI.Extensions;

public class BezierCurve
{
    public class BezierCurveSegment
    {
        float x1, x2, x3, x4, y1, y2, y3, y4;
        public float[,] Params = new float[2,4];
        public BezierCurveSegment next;

        public BezierCurveSegment() { }
        public BezierCurveSegment(float px1, float px2, float px3, float px4, float py1, float py2, float py3, float py4)
        {
            x1 = px1; x2 = px2; x3 = px3; x4 = px4;
            y1 = py1; y2 = py2; y3 = py3; y4 = py4;

            SetParams();

            next = null;
        }

        void SetParams()
        {
            float[,] Coeffs = new float[2,4];
            float dt = .02f;
            float temp, temp1;
            Coeffs[0,0] = -x1 + 3 * x2 - 3 * x3 + x4;   // ax
            Coeffs[0,1] = 3 * x1 -6 * x2 + 3 * x3;      // bx
            Coeffs[0,2] = -3 * x1 + 3 * x2;             // cx
            Coeffs[0,3] = x1;                           // dx
            Coeffs[1,0] = -y1 + 3 * y2 - 3 * y3 + y4;   // ay
            Coeffs[1,1] = 3 * y1 - 6 * y2 + 3 * y3;     // by
            Coeffs[1,2] = -3 * y1 + 3 * y2;             // cy
            Coeffs[1,3] = y1;                           // dy
            temp=dt * dt;
            temp1 = temp * dt;
            Params[0,0] = Coeffs[0,3];                                                     //x
            Params[0,1] = Coeffs[0,0] * temp1 + Coeffs[0,1] * temp + Coeffs[0,2] * dt;     //d1x
            Params[0,2] = 6 * Coeffs[0,0] * temp1 + 2 * Coeffs[0,1] * temp;                //d2x
            Params[0,3] = 6 * Coeffs[0,0] * temp1;                                         //d3x
            Params[1,0] = Coeffs[1,3];                                                     //y
            Params[1,1] = Coeffs[1,0] * temp1 + Coeffs[1,1] * temp + Coeffs[1,2] * dt;     //d1y
            Params[1,2] = 6 * Coeffs[1,0] * temp1 + 2 * Coeffs[1,1] * temp;                //d2y
            Params[1,3] = 6 * Coeffs[1,0] * temp1;                                         //d3y
        }
    }

    public BezierCurveSegment first;

    public BezierCurve(float px1, float px2, float px3, float px4, float py1, float py2, float py3, float py4)
    {
        first = new BezierCurveSegment(px1, px2, px3, px4, py1, py2, py3, py4);
    }

    public void Display(UILineRenderer line)
    {
        BezierCurveSegment tmp = first;

        int nsteps = 50, i;
        float ox,oy;
        float x,d1x,d2x,d3x;
        float y,d1y,d2y,d3y;

        List<Vector2> points = new List<Vector2>();

        while (tmp != null)
        {
            x = tmp.Params[0,0]; d1x = tmp.Params[0,1]; d2x = tmp.Params[0,2]; d3x=tmp.Params[0,3];
            y = tmp.Params[1,0]; d1y = tmp.Params[1,1]; d2y = tmp.Params[1,2]; d3y=tmp.Params[1,3];

            for(i = 0; i < nsteps;i++)
            {
                ox = x;  oy = y;
                x += d1x; d1x += d2x; d2x += d3x;
                y += d1y; d1y += d2y; d2y += d3y;

                points.Add(new Vector2(ox, oy));
                points.Add(new Vector2(x, y));
            }

            tmp = tmp.next;
        }

        line.Points = points.ToArray();
    }
}