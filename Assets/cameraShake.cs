using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;

public class cameraShake : MonoBehaviour
{
    public float intesity = 1f;
    public float amplitude = 1f;
    public bool earthquake = false;
    public CinemachineVirtualCamera cam;
    private CinemachineBasicMultiChannelPerlin noise;

    void Start()
    {
        noise = cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }
    private void Update()
    {
        if (!earthquake)
        {
            noise.m_AmplitudeGain = 0f;
            noise.m_FrequencyGain = 0f;

        }
        else
        {
            noise.m_AmplitudeGain = 1f;
            noise.m_FrequencyGain = 1f;
        }

    }

    public void ShakeCamera(float intensity, float duration)
    {
        StartCoroutine(Shake(intensity, duration));
    }

    private IEnumerator Shake(float intensity, float duration)
    {
        noise.m_AmplitudeGain = intensity;
        yield return new WaitForSeconds(duration);
        noise.m_AmplitudeGain = 0f;
    }
}
