//using UnityEngine;

//public class HeightLimiter : MonoBehaviour
//{
//    public float minY = 200f; // Hauteur minimale autoris�e

//    private Rigidbody rb;

//    void Start()
//    {
//        rb = GetComponent<Rigidbody>();
//    }

//    void FixedUpdate()
//    {
//        if (transform.position.y < minY)
//        {
//            Vector3 pos = transform.position;
//            pos.y = minY;
//            transform.position = pos;

//            if (rb != null)
//            {
//                rb.linearVelocity = Vector3.zero; // Arr�te la chute
//            }
//        }
//    }
//}
