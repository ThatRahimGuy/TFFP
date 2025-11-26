using UnityEngine;
using UnityEngine.TextCore.Text;

public class Enemy : MonoBehaviour, IDamageable
{
    public int Health { get; set; }
    public int InitialHealth { get; set; }

    public void TakeDamage(int amount)
    {
        Health -= amount;
        DebugHealth();
    }

    public void HealDamage(int amount)
    {
        Health += amount;
        DebugHealth();
    }

    public void ResetHealth()
    {
        Health = InitialHealth;
        DebugHealth();
    }

    public void SetHealth(int amount)
    {
        Health = amount;
        DebugHealth();
    }

    void DebugHealth()
    {
        Debug.Log($"Health = {Health}");
    }
}
