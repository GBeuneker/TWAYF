using UnityEngine;
using System.Collections;

/// <summary>
/// Can apply a camerashake effect to the camera (both in 2D and 3D)
/// </summary>
public class CameraShake : MonoBehaviour
{
    /// <summary>
    /// Whether we're dealing with a 2D or 3D camera.
    /// </summary>
    public bool twoDimensional;

    public float duration = 0.5f;
    public float speed = 1.0f;
    public float magnitude = 0.1f;
    private bool isShaking;
    private Vector3 startPosition;

    void Start()
    {
    }

    void Update()
    {
    }

    /// <summary>
    /// By default play camera shake with default settings, but allow user to use custom settings
    /// </summary>
    /// <param name="customDuration">Duration of the camera shake.</param>
    /// <param name="customSpeed">The speed at which the camera will shake.</param>
    /// <param name="customMagnitude">The amount of shake that is allowed.</param>
    public void PlayShake(float customDuration = 0, float customSpeed = 0, float customMagnitude = 0)
    {
        // Only allow one shake at a time
        if (isShaking)
            return;
        // Make sure the camera stays at its start position (Do not use for moving cameras)
        StopAllCoroutines();
        StartCoroutine(Shake(customDuration, customSpeed, customMagnitude));
    }

    /// <summary>
    /// Coroutine to shake the camera
    /// </summary>
    /// <param name="customDuration">Duration of the camera shake.</param>
    /// <param name="customSpeed">The speed at which the camera will shake.</param>
    /// <param name="customMagnitude">The amount of shake that is allowed.</param>
    /// <returns></returns>
    IEnumerator Shake(float customDuration = 0, float customSpeed = 0, float customMagnitude = 0)
    {
        isShaking = true;
        startPosition = transform.position;

        // If the user chose custom paramters, they are set 
        float currentDuration = duration;
        if (customDuration != 0)
            currentDuration = customDuration;

        float currentSpeed = speed;
        if (customSpeed != 0)
            currentSpeed = customSpeed;

        float currentMagnitude = magnitude;
        if (customMagnitude != 0)
            currentMagnitude = customMagnitude;

        // The time elapsed from the start of the shake
        float elapsed = 0.0f;
        // Produce a random shake every time
        float randomStartX = Random.Range(-1000.0f, 1000.0f);
        float randomStartY = Random.Range(-1000.0f, 1000.0f);

        startPosition = transform.localPosition;

        while (elapsed < currentDuration)
        {
            elapsed += Time.deltaTime;
            float percentageCompleted = elapsed / currentDuration;

            // Decrease amount of shake over time
            float damper = 1.0f - Mathf.Clamp(percentageCompleted, 0.0f, 1.0f);

            // Choose a random x and y offset using perlin noise
            float xOffset = Mathf.PerlinNoise(randomStartX + currentSpeed * percentageCompleted, 0.0f) * 5.0f - 2.0f;
            float yOffset = Mathf.PerlinNoise(0.0f, randomStartY + currentSpeed * percentageCompleted) * 5.0f - 2.0f;
            float zOffset = Mathf.PerlinNoise(randomStartX + currentSpeed * percentageCompleted, randomStartY + currentSpeed * percentageCompleted) * 5.0f - 2.0f;
            xOffset *= currentMagnitude * damper;
            yOffset *= currentMagnitude * damper;
            zOffset *= currentMagnitude * damper;

            // Don't move in z-direction if set to twoDimensional
            if (twoDimensional)
                zOffset = 0;

            // Move the camera by a random amount
            transform.localPosition = new Vector3(startPosition.x + xOffset, startPosition.y + yOffset, startPosition.z + zOffset);

            yield return null;
        }

        transform.localPosition = startPosition;
        isShaking = false;
    }
}
