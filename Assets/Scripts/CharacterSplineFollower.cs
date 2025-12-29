using UnityEngine;
using UnityEngine.Splines;

public class CharacterSplineFollower : MonoBehaviour
{
    public TrainManager trainFollower; // Reference to the TrainManager script
    public SplineContainer splineContainer; // Reference to the SplineContainer
    public Vector3 offset; // Offset for character (like sitting position)

    void Update()
    {
        // Get the normalized position (t) from the train’s movement
        float t = trainFollower.normalizedT;

        // Get the position on the spline at the current normalizedT
        Vector3 positionOnSpline = splineContainer.EvaluatePosition(t);

        // Get the direction (tangent) from the spline at that normalizedT
        Vector3 tangent = TrainManager.Normalize(splineContainer.EvaluateTangent(t));

        // Calculate the character's rotation based on the spline's tangent
        Quaternion rotationOnSpline = Quaternion.LookRotation(tangent);

        // Apply the offset to the character's position relative to the train's position
        transform.position = positionOnSpline + rotationOnSpline * offset;

        // Apply the rotation based on the spline's tangent direction
        transform.rotation = rotationOnSpline;
    }
}
