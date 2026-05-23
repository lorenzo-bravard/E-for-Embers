using UnityEngine;
using UnityEngine.Splines;
using System;

public class TrainManager : MonoBehaviour
{
    public SplineContainer splineContainer; // Assign in Inspector
    public float speed = 5f;

    public float normalizedT { get; private set; } // Accessible from other scripts
    private float distanceTravelled = 0f;
    private float splineLength;

    void Awake()
    {
        if (splineContainer != null)
        {
            splineLength = splineContainer.CalculateLength();
            
            // The user specified the train should be at Vector3(-52.7821884,79.0999908,-201.382645)
            // This corresponds to a specific distance along the spline.
            // Based on analysis, this is approximately T = 0.7026.
            normalizedT = 0.7026022f; 
            distanceTravelled = normalizedT * splineLength;
            
            // Snap to this position immediately
            UpdateTrain(normalizedT);
        }
    }

    void Start()
    {
        // Re-calculate if needed
        if (splineLength <= 0 && splineContainer != null)
            splineLength = splineContainer.CalculateLength();
    }

    void Update()
    {
        distanceTravelled += speed * Time.deltaTime;

        // Looping
        if (distanceTravelled > splineLength)
            distanceTravelled -= splineLength;

        // Calculate normalized position on the spline
        normalizedT = distanceTravelled / splineLength;

        UpdateTrain(normalizedT);
    }

    void UpdateTrain(float t)
    {
        if (splineContainer == null) return;

        // Get position and direction from spline
        Vector3 pos = splineContainer.EvaluatePosition(t);
        Vector3 dir = Normalize(splineContainer.EvaluateTangent(t)); 

        Quaternion rot = Quaternion.LookRotation(dir);
        Quaternion correction = Quaternion.Euler(0f, 90f, 0f); 

        transform.SetPositionAndRotation(pos, rot * correction);
    }

    public static Vector3 Normalize(Vector3 vec)
    {
        float xn = vec.x * vec.x;
        float yn = vec.y * vec.y;
        float zn = vec.z * vec.z;
        return vec / (float) Math.Sqrt(xn + yn + zn);
    }

    public float GetCurrentCurvature()
    {
        // Calculate how sharp the current turn is
        float sampleDistance = 0.05f;
        Vector3 previousPos = splineContainer.EvaluatePosition(Mathf.Max(0, normalizedT - sampleDistance));
        Vector3 nextPos = splineContainer.EvaluatePosition(Mathf.Min(1, normalizedT + sampleDistance));

        Vector3 incomingVec = transform.position - previousPos;
        Vector3 outgoingVec = nextPos - transform.position;

        return Vector3.SignedAngle(incomingVec, outgoingVec, Vector3.up) / 180f;
    }
}
