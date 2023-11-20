using UnityEngine;

public class FlickeringFlame : MonoBehaviour
{
    public float minIntensity = 1.0f;
    public float maxIntensity = 2.0f;
    public float flickerSpeed = 1.0f;

    private Light pointLight;
    private float baseIntensity;

    private void Start()
    {
        pointLight = GetComponent<Light>();
        baseIntensity = pointLight.intensity;
    }

    private void Update()
    {
        // Calculate a flickering effect using Perlin noise
        float noise = Mathf.PerlinNoise(Time.time * flickerSpeed, 0.0f);

        // Map the noise value to the intensity range
        float flickerIntensity = Mathf.Lerp(minIntensity, maxIntensity, noise);

        // Apply the flicker effect to the point light
        pointLight.intensity = baseIntensity * flickerIntensity;
    }
}


