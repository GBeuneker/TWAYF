using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
    GameObject world, player;
    private float minimumSize, currentSize;
    private new Camera camera;

    // Use this for initialization
    void Start()
    {
        world = GameObject.FindWithTag("World");
        player = GameObject.FindWithTag("Player");

        camera = GetComponent<Camera>();

        minimumSize = camera.orthographicSize;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateZoom();

        camera.orthographicSize = Mathf.SmoothStep(camera.orthographicSize, currentSize, 0.1f);
    }

    void UpdateZoom()
    {
        if (!world || !player)
            return;

        float distance = Vector2.Distance(world.transform.position, player.transform.position);

        float size = distance * 1.5f;

        currentSize = Mathf.Max(minimumSize, size);
    }
}
