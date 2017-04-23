using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float initialJumpStrength = 4.0f;
    public float shootStrength = 6.0f;
    public float maxJumpHeight = 30.0f;


    private Rigidbody2D body;
    private new Collider2D collider;
    private bool isGrounded, isJumping;
    private int shotsFired = 0, maxShots = 1;
    private BlockSpawner spawner;
    private float jumpTimer;

    // Use this for initialization
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
        spawner = GameObject.FindWithTag("Spawner").GetComponent<BlockSpawner>();
    }

    // Update is called once per frame
    void Update()
    {
        GroundCheck();
        if (SceneLoader.Instance.isPlaying)
            HandleInput();
    }

    private void FixedUpdate()
    {
        BlockCollideCheck();
        if (isJumping)
            IncreaseJump();
    }

    private void HandleInput()
    {
        if (Input.GetButtonDown("Jump"))
        {
            if (isGrounded)
                Jump();
            else if (shotsFired < maxShots)
                Shoot();
        }
        if (Input.GetButtonUp("Jump") && isJumping)
            isJumping = false;

        if (Input.GetButtonDown("Pause"))
            SceneLoader.Instance.PauseGame();

        if (Input.GetKeyDown(KeyCode.F))
            Die();
    }

    private void Jump()
    {
        body.velocity = Vector2.zero;
        body.AddRelativeForce(new Vector2(0, initialJumpStrength), ForceMode2D.Impulse);
        shotsFired = 0;
        jumpTimer = 0;

        isJumping = true;
        isGrounded = false;

        AudioManager.Play(Resources.Load<AudioClip>("Sounds/jump"), AudioManager.Channel.SFX);
    }

    private void IncreaseJump()
    {
        // Increase the jump timer. The entire jump should last 0.1 seconds.
        jumpTimer += Time.deltaTime * 5;
        float delay = 0.1f;
        if (jumpTimer >= Mathf.PI / 2)
        {
            isJumping = false;
            return;
        }
        if (jumpTimer > delay)
        {
            // Use a cosine wave for adding jumpforce.
            // This makes sure we add a lot initially and gradually lower the amount of force added.
            body.AddRelativeForce(new Vector2(0, Mathf.Cos(jumpTimer - delay) * maxJumpHeight));
        }
    }

    private void Shoot()
    {
        isJumping = false;
        GameObject prefab = spawner.GetRandomBlock();

        GameObject block = Instantiate(prefab, transform.position - new Vector3(0, collider.bounds.extents.y), Quaternion.identity);
        BlockBehaviour blockBehaviour = block.GetComponent<BlockBehaviour>();
        blockBehaviour.ShootDown(10);

        body.velocity = Vector2.zero;
        body.AddRelativeForce(new Vector2(0, shootStrength), ForceMode2D.Impulse);

        shotsFired++;
        AudioManager.Play(Resources.Load<AudioClip>("Sounds/shoot"), AudioManager.Channel.SFX);
        Instantiate(Resources.Load<GameObject>("Particles/jump"), transform.position - new Vector3(0, collider.bounds.extents.y), Quaternion.identity);
    }

    private void Die()
    {
        Camera.main.GetComponent<CameraShake>().PlayShake(0.5f, 1, 0.5f);
        SceneLoader.Instance.ShowScore();
        AudioManager.Play(Resources.Load<AudioClip>("Sounds/die"), AudioManager.Channel.SFX);
        Destroy(gameObject);
    }

    private void GroundCheck()
    {
        // Shoot a single ray
        RaycastHit2D hit = Physics2D.Raycast(transform.position - new Vector3(0, collider.bounds.extents.y + 0.001f), Vector2.down, 0.1f);
        isGrounded = hit.collider != null && (hit.collider.tag == "World" || hit.collider.tag == "Block");

        if (!isGrounded)
        {
            // Make an area raycast
            Vector2 upLeft = transform.position - new Vector3(collider.bounds.extents.x, collider.bounds.extents.y + 0.001f);
            Vector2 downRight = transform.position - new Vector3(-collider.bounds.extents.x, collider.bounds.extents.y + 0.2f);

            Collider2D hitCollider = Physics2D.OverlapArea(upLeft, downRight);

            isGrounded = hitCollider != null && (hitCollider.tag == "World" || hitCollider.tag == "Block");
        }

        GetComponent<Animator>().SetBool("isGrounded", isGrounded);

    }

    private void BlockCollideCheck()
    {
        // Make an area raycast
        Vector2 upLeft = transform.position + new Vector3(collider.bounds.extents.x, collider.bounds.extents.y + 0.001f);
        Vector2 downRight = transform.position + new Vector3(collider.bounds.extents.x + 0.1f, 0);

        Collider2D hitCollider = Physics2D.OverlapArea(upLeft, downRight);

        bool isHit = hitCollider != null && hitCollider.tag == "Block";
        if (isHit)
            Die();
    }
}
