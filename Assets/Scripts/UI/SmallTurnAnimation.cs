using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallTurnAnimation : MonoBehaviour
{
    // a tiny animation for round UI buttons
    // turns clockwise, then counterclockwise back to its original rotation
    [SerializeField] float Peak_Rotation_Amplitude;
    [SerializeField] float WiggleSpeed;
    private float TimeAccumulated;
    // Update is called once per frame
    void Update()
    {
        TimeAccumulated += Time.deltaTime;
        float newRot = - Peak_Rotation_Amplitude * (Mathf.Sin(WiggleSpeed * TimeAccumulated) + 1);
        transform.rotation = Quaternion.Euler(0f,0f, newRot);
    }
}
