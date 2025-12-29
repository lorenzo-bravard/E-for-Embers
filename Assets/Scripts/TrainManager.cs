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

    void Start()
    {
        // Calculate the total length of the spline once
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

        // Get position and direction from spline
        Vector3 pos = splineContainer.EvaluatePosition(normalizedT);
        Vector3 dir = Normalize(splineContainer.EvaluateTangent(normalizedT)); // Make sure the direction is normalized

        // Optional: apply rotation correction (like aligning to train's forward)
        Quaternion rot = Quaternion.LookRotation(dir);
        Quaternion correction = Quaternion.Euler(0f, 90f, 0f); // Adjust the correction angle as needed

        // Apply position and rotation to the train
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
