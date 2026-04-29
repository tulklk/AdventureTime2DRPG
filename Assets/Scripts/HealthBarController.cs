using UnityEngine;
using UnityEngine.UI;


public class HealthBarController : MonoBehaviour
{
    [SerializeField] private Slider slider;

    // Thiết lập thanh máu
    public void SetHealth(int currentHealth, int maxHealth)
    {
        slider.value = (float)currentHealth / maxHealth;
    }
}
