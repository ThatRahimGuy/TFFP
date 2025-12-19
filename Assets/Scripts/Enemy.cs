using UnityEngine;
using UnityEngine.TextCore.Text;

public class Enemy : MonoBehaviour, IDamageable
{
    public GameObject player;
    public float speed;
    private float distance;
    public int Health { get; set; }
    public int InitialHealth { get; set; }

    public void TakeDamage(int amount)
    {
        Health -= amount;
        
        if(Health < 0)
        {
           Destroy(gameObject);
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
    }
}
