using FirstGearGames.SmoothCameraShaker;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class Enemy : MonoBehaviour, IDamageable
{
    public GameObject player;
    public float speed;
    public WaveSpawner waveSpawner;
    public float enemieskilled = 0;
    private float distance;
    public ShakeData enemydeathshake;
    public int Health { get; set; }
    public int InitialHealth { get; set; }
    public Movement playerHealth;

    [SerializeField] int _damage = 1;
    [SerializeField] int health = 3;
    private WaveSpawner spawner;

    private void Start()
    {
        player = FindAnyObjectByType<Movement>().gameObject;
        spawner = FindAnyObjectByType<WaveSpawner>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.CompareTag("Player"))
        {
            player.GetComponent<Movement>().TakeDamage(1);
            Debug.Log("Hello");
        }
    }


    public void TakeDamage(int amount)
    {
        health -= amount;
        
        if(health <= 0)
        {
           enemieskilled += 1;
            spawner.EnemyKilled();
           Destroy(gameObject);
           CameraShakerHandler.Shake(enemydeathshake);
        }
        DebugHealth();
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

    void Update()
    {
        distance = Vector2.Distance(transform.position, player.transform.position);
        Vector2 direction = player.transform.position - transform.position;
        transform.position = Vector2.MoveTowards(this.transform.position, player.transform.position, speed * Time.deltaTime);
        //if (distance < 5)
        //{
        //    transform.position = Vector2.MoveTowards(this.transform.position, player.transform.position, speed * Time.deltaTime);
        //}

        //if (enemieskilled == 5)
        //{
        //    waveSpawner.WaveDone();
        //}
    }
}
