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

    [SerializeField] int health = 3;

    public void TakeDamage(int amount)
    {
        Health -= amount;
        
        if(Health < 0)
        {
            enemieskilled += 1;
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
        
        if(distance < 5)
        {
            transform.position = Vector2.MoveTowards(this.transform.position, player.transform.position, speed * Time.deltaTime);
        }

        if (enemieskilled == 5)
        {
            waveSpawner.WaveDone();
        }
    }
}
