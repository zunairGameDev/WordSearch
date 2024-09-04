using Unity.VisualScripting;
using UnityEngine.UIElements;

/// Credit Alastair Aitchison
/// Sourced from - https://bitbucket.org/UnityUIExtensions/unity-ui-extensions/issues/123/uilinerenderer-issues-with-specifying
namespace UnityEngine.UI.Extensions
{
    [AddComponentMenu("UI/Extensions/UI Line Connector")]
    [RequireComponent(typeof(UILineRenderer))]
    [ExecuteInEditMode]
    public class UILineConnector : MonoBehaviour
    {

        // The elements between which line segments should be drawn
        public RectTransform[] transforms;
        public Vector2[] vector2;
        public bool selectingLineRenderer;
        private Vector2[] previousPositions;
        public RectTransform canvas;
        private RectTransform rt;
        private UILineRenderer lr;

        private void Awake()
        {
            canvas = GetComponentInParent<RectTransform>().GetParentCanvas().GetComponent<UIManager>().GetGridRectTransform().parent.GetComponent<RectTransform>();
            rt = GetComponent<RectTransform>();
            lr = GetComponent<UILineRenderer>();
        }

        // Update is called once per frame
        void Update()
        {
            UpToDateLine();
        }
        public void UpToDateLine()
        {
            if (selectingLineRenderer)
            {
                if (vector2 == null || vector2.Length < 1)
                {
                    return;
                }
                //Performance check to only redraw when the child transforms move
                if (previousPositions != null && previousPositions.Length == vector2.Length)
                {
                    bool updateLine = false;
                    for (int i = 0; i < vector2.Length; i++)
                    {
                        if (!updateLine && previousPositions[i] != vector2[i])
                        {
                            updateLine = true;
                        }
                    }
                    if (!updateLine) return;
                }
                float distance = Vector2.Distance(vector2[0], vector2[vector2.Length - 1]);
                Vector2 direction = new Vector2(Mathf.Cos(HighlightBehaviour.instance._selectingLineAngle * Mathf.Deg2Rad), Mathf.Sin(HighlightBehaviour.instance._selectingLineAngle * Mathf.Deg2Rad));
                Vector2 endPoint = vector2[0] + direction * distance;
                // Constrain each point to the bounds of the canvas RectTransform
                //for (int i = 0; i < vector2.Length; i++)
                //{
                //    vector2[i] = ConstrainToRect(vector2[i], canvas);
                //}
                Vector2 startPoint = ConstrainToRect(vector2[0], canvas);
                endPoint = ConstrainToRect(endPoint, canvas);
                lr.Points = new Vector2[] { startPoint, endPoint };
                lr.RelativeSize = false;
                lr.drivenExternally = true;
                previousPositions = new Vector2[vector2.Length];
                for (int i = 0; i < transforms.Length; i++)
                {
                    previousPositions[i] = vector2[i];
                }
            }
            //else
            //{
            //    if (transforms == null || transforms.Length < 1)
            //    {
            //        return;
            //    }
            //    //Performance check to only redraw when the child transforms move
            //    if (previousPositions != null && previousPositions.Length == transforms.Length)
            //    {
            //        bool updateLine = false;
            //        for (int i = 0; i < transforms.Length; i++)
            //        {
            //            if (!updateLine && previousPositions[i] != transforms[i].anchoredPosition)
            //            {
            //                updateLine = true;
            //            }
            //        }
            //        if (!updateLine) return;
            //    }
            //    //if (previousPositions == null || previousPositions.Length != transforms.Length)
            //    //{
            //    //    previousPositions = new Vector2[transforms.Length];
            //    //    for (int i = 0; i < transforms.Length; i++)
            //    //    {
            //    //        previousPositions[i] = transforms[i].anchoredPosition;
            //    //    }
            //    //}

            //    // Get the pivot points
            //    Vector2 thisPivot = rt.pivot;
            //    Vector2 canvasPivot = canvas.pivot;

            //    // Set up some arrays of coordinates in various reference systems
            //    Vector3[] worldSpaces = new Vector3[transforms.Length];
            //    Vector3[] canvasSpaces = new Vector3[transforms.Length];
            //    Vector2[] points = new Vector2[transforms.Length];

            //    // First, convert the pivot to worldspace
            //    for (int i = 0; i < transforms.Length; i++)
            //    {
            //        worldSpaces[i] = transforms[i].TransformPoint(thisPivot);
            //    }

            //    // Then, convert to canvas space
            //    for (int i = 0; i < transforms.Length; i++)
            //    {
            //        canvasSpaces[i] = canvas.InverseTransformPoint(worldSpaces[i]);
            //    }
            //    if (canvasSpaces == null || canvasSpaces.Length != transforms.Length)
            //    {
            //        Debug.LogError("canvasSpaces array is not properly initialized.");
            //        return;
            //    }
            //    Vector2 targetPosition;
            //    // Calculate delta from the canvas pivot point
            //    for (int i = 0; i < transforms.Length; i++)
            //    {
            //        //points[i] = new Vector2(canvasSpaces[i].x, canvasSpaces[i].y);

            //        targetPosition = new Vector2(canvasSpaces[i].x, canvasSpaces[i].y);

            //        // Debug output to check values
            //        //if (i >= previousPositions.Length)
            //        //{
            //        //    Debug.LogError($"Index {i} is out of bounds for previousPositions.");
            //        //    continue;
            //        //}
            //        // Use Lerp to smoothly move towards the target position
            //        points[i] = Vector2.Lerp(previousPositions[i], targetPosition, 1f); // Adjust the last parameter for speed
            //    }

            //    // And assign the converted points to the line renderer
            //    lr.Points = points;
            //    lr.RelativeSize = false;
            //    lr.drivenExternally = true;

            //    previousPositions = new Vector2[transforms.Length];
            //    for (int i = 0; i < transforms.Length; i++)
            //    {
            //        previousPositions[i] = transforms[i].anchoredPosition;
            //    }
            //}
        }
        private Vector2 ConstrainToRect(Vector2 point, RectTransform rect)
        {
            Rect rectBounds = rect.rect;
            float offSet = 100f;
            point.x = Mathf.Clamp(point.x, rectBounds.xMin + offSet, rectBounds.xMax - offSet);
            point.y = Mathf.Clamp(point.y, rectBounds.yMin + offSet, rectBounds.yMax - offSet);
            return point;
        }
    }

}