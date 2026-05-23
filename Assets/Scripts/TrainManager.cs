using UnityEngine;
using UnityEngine.Splines;
using System;

public class TrainManager : MonoBehaviour
{
    public SplineContainer splineContainer;
    public float speed = 5f;

    public float normalizedT { get; private set; } 
    private float distanceTravelled = 0f;
    private float splineLength;

    void Awake()
    {
        if (splineContainer != null)
        {
            splineLength = splineContainer.CalculateLength();
            

            normalizedT = 0.7026022f; 
            distanceTravelled = normalizedT * splineLength;
            
            UpdateTrain(normalizedT);
        }
    }

    void Start()
    {
        if (splineLength <= 0 && splineContainer != null)
            splineLength = splineContainer.CalculateLength();
    }

    void Update()
    {
        distanceTravelled += speed * Time.deltaTime;

        if (distanceTravelled > splineLength)
            distanceTravelled -= splineLength;

        normalizedT = distanceTravelled / splineLength;

        UpdateTrain(normalizedT);
    }

    void UpdateTrain(float t)
    {
        if (splineContainer == null) return;

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
        float sampleDistance = 0.05f;
        Vector3 previousPos = splineContainer.EvaluatePosition(Mathf.Max(0, normalizedT - sampleDistance));
        Vector3 nextPos = splineContainer.EvaluatePosition(Mathf.Min(1, normalizedT + sampleDistance));

        Vector3 incomingVec = transform.position - previousPos;
        Vector3 outgoingVec = nextPos - transform.position;

        return Vector3.SignedAngle(incomingVec, outgoingVec, Vector3.up) / 180f;
    }
}
