using UnityEngine;
using UnityEngine.Splines;

public class TrainMover : MonoBehaviour
{
    public SplineContainer splineContainer; // Drag the track spline here
    public float speed = 2f;

    private float t = 0f; // Normalized time along the spline

    void Update()
    {
        if (splineContainer == null) return;

        // Move forward on the spline
        t += speed * Time.deltaTime / splineContainer.CalculateLength();
        t %= 1f; // Loop the train when it reaches the end

        // Get the position and rotation
        var curve = splineContainer.Spline;
        Vector3 position = curve.EvaluatePosition(t);
        //Quaternion rotation = curve.EvaluateRotation(t);

        //transform.SetPositionAndRotation(position, rotation);
    }
}
