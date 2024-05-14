using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    [Tooltip("Amount of health to heal when picked up")]
    [SerializeField] int healValue;

    BasePlayer player; // used to manipulate player's health

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<BasePlayer>();
    }

    public void ProcessHealthPickup()
    {
        player.Heal(healValue);
    }
}
