using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HealthSystem : MonoBehaviour
{
    [SerializeField] protected float life;
    [SerializeField] protected float maxLife;
    [SerializeField] protected float healingSpeed;
    [SerializeField] protected bool canHeal = false;
    [SerializeField] protected UnityEvent OnDie;
    float timer = 0;
    float timerLimit = 3f;

    public void SubstractLife(float damage) 
    { 
        life -= damage;
        timer = 0;
    }

    void Update()
    {
        if (canHeal)
        {
            if (timer >= timerLimit)
            {
                if (life > maxLife)
                    life += Time.deltaTime * healingSpeed;
            }
            else
                timer += Time.deltaTime;
        }
    }

    public bool CheckDie()
    {
        if (life <= 0)
        {
            return true;
        }

        return false;
    }

    public void CheckDieEvent()
    {
        if (life <= 0)
        {
            OnDie?.Invoke();
        }
    }

    public void ResetLife()
    {
        life = maxLife;
    }

    public float GetLife() { return life; }
    public float GetMaxLife() { return maxLife; }
}
