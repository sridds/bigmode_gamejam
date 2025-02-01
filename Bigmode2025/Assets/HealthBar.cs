using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    PlayerHealthScript healthScript;
    [SerializeField] Slider healthSlider;
    float maxHealth;
    void Awake()
    {
        healthScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealthScript>();
        //maxHealth = healthScript.health;
    }

    // Update is called once per frame
    void Update()
    {
        //healthSlider.value = healthScript.health/ maxHealth;
    }
}
