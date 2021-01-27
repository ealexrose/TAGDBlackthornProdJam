using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShakeController : MonoBehaviour
{
    // instance of this script for enemies and bullets to call.
    public static ScreenShakeController instance;

    private Vector3 TargetPosition;             // the position the camera should always be 'focused' in. The Screenshake works by 
                                                // generating noise for the camera to move around this position.
    private float ShakeTimeRemaining, ShakePower;
    private float ShakeFadeTime;                // // The magnitude of the power variable overtime
    private float ShakeRotation;

    [Tooltip("Multiplies the power to give the max angle of rotation during the screenshake.")]
    [SerializeField] float rotationMultiplier = 15f;

    private void Awake()
    {
        TargetPosition = transform.position;
    }

    private void Start()
    {
        instance = this;
    }

    private void Update()
    {
        transform.position = TargetPosition;
        /*Testing
        if (Input.GetKeyDown(KeyCode.A)) 
            ShakeScreen(0.2f, 0.3f);
            */
    }

    private void LateUpdate()                   // offset the position of the camera if the camera is shaking.
    {
        if(ShakeTimeRemaining > 0)
        {
            ShakeTimeRemaining -= Time.deltaTime;
            float x_Offset = GetRandomOffset();
            float y_Offset = GetRandomOffset();

            transform.position += new Vector3(x_Offset, y_Offset,0);

            // both power and rotation fade at a constant rate.

            ShakePower = Mathf.MoveTowards(ShakePower, 0f, ShakeFadeTime * Time.deltaTime);

            ShakeRotation = Mathf.MoveTowards(ShakeRotation, 0f, ShakeFadeTime  * rotationMultiplier * Time.deltaTime);
        }

        transform.rotation = Quaternion.Euler(0f, 0f, ShakeRotation * Random.Range(-1f,1f));

    }

    private float GetRandomOffset()
    {
        return Random.Range(-1f, 1f) * ShakePower;
    }

    public void ShakeScreen(float shakeDuration, float Power)
    {
        ShakeTimeRemaining = shakeDuration;
        ShakePower = Power;

        ShakeFadeTime = Power / shakeDuration;
        ShakeRotation = Power * rotationMultiplier;
    }
}
