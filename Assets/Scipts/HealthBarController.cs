using UnityEngine;
using UnityEngine.UI;

public class HealthBarController : MonoBehaviour
{
    [Tooltip("Drag the Image component used for the fill (usually the green square)")]
    public Image fillImage;

    public void SetHealth(float currentHealth, float maxHealth)
    {
        if (fillImage == null)
        {
            Debug.LogWarning("HealthBarController: fillImage is not assigned.");
            return;
        }

        if (maxHealth <= 0f)
        {
            Debug.LogWarning("HealthBarController: maxHealth must be > 0.");
            fillImage.fillAmount = 0f;
            return;
        }

        float t = Mathf.Clamp01(currentHealth / maxHealth);
        fillImage.fillAmount = t;
    }
}
