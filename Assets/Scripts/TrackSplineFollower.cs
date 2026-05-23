using UnityEngine;
using UnityEngine.Splines;

public class TrainMover : MonoBehaviour
{
    public SplineContainer splineContainer; 
    public float speed = 2f;

    private float t = 0f; 

    void Update()
    {
        if (splineContainer == null) return;

        t += speed * Time.deltaTime / splineContainer.CalculateLength();
        t %= 1f; 

        var curve = splineContainer.Spline;
        Vector3 position = curve.EvaluatePosition(t);

    }
}
