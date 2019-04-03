using UnityEngine;
using UnityEngine.UI.Extensions;

public class BezierSurfaceRenderer : MonoBehaviour
{
    private BezierSurfaceNet surface;

    [SerializeField]
    private Transform[] controlPoints;

    [SerializeField]
    private UILineRenderer[] lines;


    private BezierCurve[] bz = new BezierCurve[4];


    private UIRectangle rects;


    [SerializeField]
    private RectTransform panel;

    
    void Awake()
    {
        rects = GetComponent<UIRectangle>();    
    }


    void Start()
    {
        CreateSurface();
        
        DragController.PointMoved += () => { CreateSurface(); };
    }


    void CreateSurface()
    {
        for (int i = 0; i < 4; i++)
        {
            bz[i] = new BezierCurve(controlPoints[i * 4].position.x,
                                    controlPoints[i * 4 + 1].position.x,
                                    controlPoints[i * 4 + 2].position.x,
                                    controlPoints[i * 4 + 3].position.x,
                                    controlPoints[i * 4].position.y,
                                    controlPoints[i * 4 + 1].position.y,
                                    controlPoints[i * 4 + 2].position.y,
                                    controlPoints[i * 4 + 3].position.y);
        }

        surface = new BezierSurfaceNet(bz[0], bz[1], bz[2], bz[3]);

        surface.Display(lines, rects, panel.rect.width, panel.rect.height);
    }
}