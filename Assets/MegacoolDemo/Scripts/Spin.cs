using UnityEngine;
using System.Collections;

public class Spin : MonoBehaviour
{
    float speed = 10f;

    void Update ()
    {
        transform.Rotate(Vector3.up, speed * Time.deltaTime);
    }

    public void setSpeed(float newSpeed){
        speed = newSpeed;
    }
}