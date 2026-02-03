using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] public float maxHealth;
    private float currentHealth;

    [SerializeField] private Slider sliderHealth;
    [SerializeField] private float inmuneTime;

    private bool inmune = false; // para que entre en damage primero

    [SerializeField] private AudioClip clipDeath, clipHurt;
    [SerializeField] private TMP_Text healthNumberText;
    AudioSource audioS;

    private void Awake()
    {
        sliderHealth.value = sliderHealth.maxValue = currentHealth = maxHealth;
        audioS = GetComponent<AudioSource>();
        UpdateHealthText();
    }

    private void UpdateHealthText()
    {
        sliderHealth.value = currentHealth;
        healthNumberText.text = currentHealth + " / " + maxHealth;
    }
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && !inmune)
        {
            //recibe damage al chocar contra los que tengan tag Enemy
            GetDamage();
        }
    }
    public void IncreaseMaxHealth(float amount)
    {
        maxHealth += amount;
        currentHealth += amount;
        UpdateHealthText();
    }
    public void GetDamage()
    {
        inmune = true;
        currentHealth--;
        sliderHealth.value = currentHealth;
        UpdateHealthText();

        if (currentHealth <= 0)
        {
            GetComponent<PlayerMovement>().enabled = false;
            GetComponent<PlayerShooting>().enabled = false;
            GetComponent<Rigidbody>().isKinematic = true;
            GetComponent<Animator>().Play("Death");
            audioS.clip = clipDeath;
        }
        else
        {
            audioS.clip = clipHurt;
        }
        audioS.Play();

        // Invoke es una forma de llamar al método dentro de X tiempo 
        // nameof devuelve un string usando el metodo y no pasa nada si le cambio el nombre
        Invoke(nameof(InmuneOff), inmuneTime);

    }
    public void InmuneOff()
    {
        inmune = false;
    }

    public void RestartLevel()
    {
        //  GameManager.instance.GameOver();
    }

}
