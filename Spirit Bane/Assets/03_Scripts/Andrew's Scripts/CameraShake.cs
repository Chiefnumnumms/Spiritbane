using UnityEngine;
using Cinemachine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
    [SerializeField] CinemachineImpulseSource screenShake;
    [SerializeField] float powerAmount;

    public void ScreenShake(Vector3 dir)
    {
        screenShake.GenerateImpulseWithVelocity(dir);
    }

}