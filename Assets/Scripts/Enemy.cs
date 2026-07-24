using FirstGearGames.SmoothCameraShaker;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class Enemy : MonoBehaviour, IDamageable
{
    public Movement player;
    public float speed;
    public WaveSpawner waveSpawner;
    public float enemieskilled = 0;
    private float distance;
    public ShakeData enemydeathshake;
    public int Health { get; set; }
    public int InitialHealth { get; set; }
    public Movement playerHealth;
    Animator animator;
    SpriteRenderer sr;
    //LootTable
    [Header("Loot")]
    public List<LootItem> lootTable = new List<LootItem>();

    [SerializeField] int _damage = 1;
    [SerializeField] int health = 3;
    private WaveSpawner spawner;
    [SerializeField] private Rigidbody2D rb;
    public AudioClip deathAudio;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Movement>();
        spawner = FindAnyObjectByType<WaveSpawner>();
        rb =  GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.CompareTag("Player"))
        {
            player.GetComponent<Movement>().TakeDamage(1);
            //blink.Blink();
            Debug.Log("Hello");
        }
    }


    public void TakeDamage(int amount)
    {
        health -= amount;
// TakeKnockback(player.transform.position);
        //blink.Blink();

        if(health <= 0)
        {
           enemieskilled += 1;
           spawner.EnemyKilled();
            AudioSource.PlayClipAtPoint(deathAudio, transform.position);
            Destroy(gameObject);
           CameraShakerHandler.Shake(enemydeathshake);

            foreach (LootItem lootItem in lootTable)
            {
                if(Random.Range(0f,100f) <= lootItem.dropChance)
                {
                    InstantiateLoot(lootItem.itemPrefab);
                    break;
                }
            }
        }
        DebugHealth();
    }

    void InstantiateLoot(GameObject loot)
    {
        if (loot)
        {
            GameObject droppedLoot = Instantiate(loot, transform.position, Quaternion.identity);
        }
    }

    public void HealDamage(int amount)
    {
        Health += 2;
        DebugHealth();
    }

    public void ResetHealth()
    {
        Health = InitialHealth;
        DebugHealth();
    }

    public void SetHealth(int amount)
    {
        Health = 4;
        DebugHealth();
    }

    void DebugHealth()
    {
        Debug.Log($"Health = {Health}");
    }

    public void TakeKnockback(Vector2 pos)
    {
        rb.AddForceAtPosition(player.isFacingRight? player.transform.right * 6.7f : player.transform.right * (-1 * 6.7f), pos, ForceMode2D.Impulse);
    }

    void Update()
    {
        if (!player) return;
        distance = Vector2.Distance(transform.position, player.transform.position);
        Vector2 direction = player.transform.position - transform.position;
        transform.position = Vector2.MoveTowards(this.transform.position, player.transform.position, speed * Time.deltaTime);
        FacePlayer();
        //if (distance < 5)
        //{
        //    transform.position = Vector2.MoveTowards(this.transform.position, player.transform.position, speed * Time.deltaTime);
        //}

        //if (enemieskilled == 5)
        //{
        //    waveSpawner.WaveDone();
        //}
    }

    private void FacePlayer()
    {
        float xDiff = player.transform.position.x - transform.position.x;
        if (xDiff > 0) sr.flipX = false;
        else sr.flipX = true;
        
    }
}
