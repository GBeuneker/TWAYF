using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockBehaviour : MonoBehaviour
{
    Rigidbody2D body;

    // Use this for initialization
    void Awake()
    {
        transform.SetParent(GameObject.FindWithTag("World").transform);
        body = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ShootDown(float force)
    {
        body.AddRelativeForce(new Vector2(0, -force), ForceMode2D.Impulse);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "World" || other.gameObject.tag == "Block")
        {
            body.bodyType = RigidbodyType2D.Static;
            Camera.main.GetComponent<CameraShake>().PlayShake();
            gameObject.tag = "Block";
            AudioManager.Play(Resources.Load<AudioClip>("Sounds/impact"), AudioManager.Channel.SFX);
            GameObject particles = Instantiate(Resources.Load<GameObject>("Particles/impact"), transform.position, Quaternion.identity);
            particles.transform.SetParent(transform.parent);
        }
    }
}
