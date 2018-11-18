using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bezier : MonoBehaviour {
    public List<Vector2> points = new List<Vector2>();
    public Color c1 = Color.yellow;
    public Color c2 = Color.red;
    public GameObject lerpedPoint;
    public List<DraggablePoint> pointsToLerp;
    // Use this for initialization
    void Start () {
        CreateLineRenderer();
    }
	
    void CreateLineRenderer()
    {
        LineRenderer lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.widthMultiplier = 0.2f;
        lineRenderer.positionCount = points.Count;

        // A simple 2 color gradient with a fixed alpha of 1.0f.
        float alpha = 1.0f;
        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[] { new GradientColorKey(c1, 0.0f), new GradientColorKey(c2, 1.0f) },
            new GradientAlphaKey[] { new GradientAlphaKey(alpha, 0.0f), new GradientAlphaKey(alpha, 1.0f) }
            );
        lineRenderer.colorGradient = gradient;

    }

	// Update is called once per frame
	void Update () {
        LineRenderer lineRenderer = GetComponent<LineRenderer>();
        for(int i = 0; i < 4; i++)
        {
            points[i] = pointsToLerp[i].transform.position;
        }
        
        for (int i = 0; i < points.Count; i++)
        {
            lineRenderer.SetPosition(i, points[i]);
        }

        var t = Mathf.Abs((Mathf.Sin(Time.time)));
        lerpedPoint.transform.position = BezierCurve(points, t);
    }

    Vector2 BezierCurve(List<Vector2> curvePoints, float t)
    {
        if (curvePoints.Count > 4)
        {
            Debug.LogError("Bezier can only take 4 points");
            return Vector2.zero;
        }
        Vector2[] lerps = new Vector2[6];
        lerps[0] = Vector2.Lerp(curvePoints[0], curvePoints[1], t);
        lerps[1] = Vector2.Lerp(curvePoints[1], curvePoints[2], t);
        lerps[2] = Vector2.Lerp(curvePoints[2], curvePoints[3], t);
        lerps[3] = Vector2.Lerp(lerps[0], lerps[1], t);
        lerps[4] = Vector2.Lerp(lerps[1], lerps[2], t);
        lerps[5] = Vector2.Lerp(lerps[3], lerps[4], t);

        return lerps[5];
    }
}
