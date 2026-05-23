using UnityEngine;
using UnityEngine.Splines;

public class CharacterSplineFollower : MonoBehaviour
{
    public TrainManager trainFollower; 
    public SplineContainer splineContainer; 
    public Vector3 offset; 

    void Update()
    {

        float t = trainFollower.normalizedT;


        Vector3 positionOnSpline = splineContainer.EvaluatePosition(t);

 
        Vector3 tangent = TrainManager.Normalize(splineContainer.EvaluateTangent(t));


        Quaternion rotationOnSpline = Quaternion.LookRotation(tangent);


        transform.position = positionOnSpline + rotationOnSpline * offset;


        transform.rotation = rotationOnSpline;
    }
}
