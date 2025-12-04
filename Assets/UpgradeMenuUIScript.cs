using UnityEngine;
using TMPro;

public class UpgradeMenuUIScript : MonoBehaviour
{
    public BodyScript tank;
    public TurrentScript turret;

    // TextMeshPro fields (nastav v Inspectoru)
    public TextMeshProUGUI hpTMP;
    public TextMeshProUGUI speedTMP;
    public TextMeshProUGUI damageTMP;
    public TextMeshProUGUI reloadTMP;
    public TextMeshProUGUI shellSpeedTMP;

    void Start()
    {
        // při startu jednorázově zkopírujeme staty do TMP polí
        if (tank != null)
        {
            if (hpTMP != null) hpTMP.text = tank.maxHealth.ToString();
            if (speedTMP != null) speedTMP.text = tank.moveSpeed.ToString("0.##");
        }
        else
        {
            if (hpTMP != null) hpTMP.text = "—";
            if (speedTMP != null) speedTMP.text = "—";
        }

        // damage není ve BodyScript -> zobrazíme prostě "—"
        if (damageTMP != null) damageTMP.text = "—";

        if (turret != null)
        {
            if (reloadTMP != null) reloadTMP.text = $"{turret.reloadTime:0.00}s";
            if (shellSpeedTMP != null) shellSpeedTMP.text = turret.shellSpeed.ToString("0.##");
        }
        else
        {
            if (reloadTMP != null) reloadTMP.text = "—";
            if (shellSpeedTMP != null) shellSpeedTMP.text = "—";
        }
    }
}
