using UnityEngine;

public abstract class Food : MonoBehaviour
{
    protected float Amount { get; private set; }

    [SerializeField] protected float fullAmount;

    public float Feed(float amount)
    {
        float foodTaken = Mathf.Clamp(amount, 0f, Amount);

        Amount -= amount;

        if (Amount <= 0)
        {
            Amount = 0f;
                
            OnEmpty();
        }

        return foodTaken;
    }

    protected abstract void OnEmpty();
    
    /// <summary>
    /// Reset source
    /// </summary>
    public virtual void ResetFood()
    {
        Amount = fullAmount;
    }
}