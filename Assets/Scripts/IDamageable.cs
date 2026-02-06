public interface IDamageable
{
    public int Health { get; set;}

    public void TakeDamage(int amount);

    public void HealDamage(int amount);

    public void ResetHealth();

    public void SetHealth(int amount);
}


