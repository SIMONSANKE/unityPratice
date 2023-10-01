using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RubtController : MonoBehaviour
{
    Rigidbody2D rigidbody2d;
    public int maxHealth = 5;
    int currentHealth;
    public int health { get { return currentHealth; } }
    float horizontal;
    float vertical;
    public float speed = 3.0f;
    private Animator _animator;
    public GameObject projecttilePerfab;

    private Vector2 lookDirection = new Vector2(1, 0);
    public float timeInvincible = 2.0f;
    private AudioSource _audioSource;
    bool isInvincible;
    public AudioClip shootClip;
    private float invinvibleTimer;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D >();
        currentHealth = maxHealth;
        _animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
    }

    public void PlaySound(AudioClip clip)
    {
        _audioSource.PlayOneShot(clip);
    }
    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        Vector2 move = new Vector2(horizontal, vertical);
        if (!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
        {
            lookDirection.Set(move.x, move.y);
            lookDirection.Normalize();
        }
        _animator.SetFloat("Look X", lookDirection.x);
        _animator.SetFloat("Look Y", lookDirection.y);
        _animator.SetFloat("Speed", move.magnitude);
        //Debug.Log(horizontal);
        if (isInvincible)
        {
            invinvibleTimer -= Time.deltaTime;
            if (invinvibleTimer < 0)
                isInvincible = false;
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            Launch();
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            RaycastHit2D hit = Physics2D.Raycast(rigidbody2d.position + Vector2.up * 0.2f, lookDirection, 1.5f,
                LayerMask.GetMask("NPC"));
            if (hit.collider != null)
            {
                NPCController c = hit.collider.GetComponent<NPCController>();
                if (c != null)
                {
                    c.DisplayDialog();
                }
            }
        }
        
    }
    void FixedUpdate()
    {
        Vector2 position = rigidbody2d.position;
        position.x = position.x + speed * horizontal * Time.deltaTime;
        position.y = position.y + speed * vertical * Time.deltaTime;
        //transform.position = position;
        rigidbody2d.MovePosition(position);
    }

    void Launch()
    {
        GameObject projecttileObject =
            Instantiate(projecttilePerfab, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);
        ProjectTile projectTile = projecttileObject.GetComponent<ProjectTile>();
        projectTile.Launch(lookDirection, 300);
        _animator.SetTrigger("Launch");
        this.PlaySound(shootClip);
    }

    public void ChangeHealth(int amount)
    {
        
        if (amount < 0)
        {
            _animator.SetTrigger("Hit");
            if (isInvincible) return;
            isInvincible = true;
            invinvibleTimer = timeInvincible;
        }
        
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        Debug.Log(currentHealth + "/" + maxHealth);
        UIHealthBar.instance.SetValue(currentHealth/(float)maxHealth);
    }
}
