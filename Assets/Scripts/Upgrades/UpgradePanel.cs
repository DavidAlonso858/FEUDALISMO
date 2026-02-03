using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradePanel : MonoBehaviour
{
    [Header("Botones")]
    public Button button1;
    public Button button2;
    
    [Header("Textos")]
    public TMP_Text text1;
    public TMP_Text text2;
    public TMP_Text description1;
    public TMP_Text description2;
    
    private UpgradeGeneral upgrade1;
    private UpgradeGeneral upgrade2;
    private bool panelActive = false;

    private void Start()
    {
        // Desactivar al inicio
        gameObject.SetActive(false);
        
        // Configurar listeners
        SetupButtonListeners();
    }

    private void SetupButtonListeners()
    {
        if (button1 != null)
        {
            button1.onClick.RemoveAllListeners();
            button1.onClick.AddListener(() => SelectUpgrade(1));
        }
        
        if (button2 != null)
        {
            button2.onClick.RemoveAllListeners();
            button2.onClick.AddListener(() => SelectUpgrade(2));
        }
    }

    public void Show()
    {
        if (panelActive || gameObject.activeSelf) return;
        
        Debug.Log("=== MOSTRANDO PANEL DE UPGRADES ===");
        panelActive = true;
        
        // Pausar el juego
        Time.timeScale = 0f;
        
        // Activar el panel
        gameObject.SetActive(true);
        
        // Obtener upgrades
        LoadUpgrades();
    }

    private void LoadUpgrades()
    {
        if (UpgradeManager.instance == null)
        {
            Debug.LogError("UpgradeManager no encontrado!");
            return;
        }
        
        var upgrades = UpgradeManager.instance.GetTwoRandomUpgrades();
        
        if (upgrades.Count >= 2)
        {
            upgrade1 = upgrades[0];
            upgrade2 = upgrades[1];
            
            // Actualizar UI
            text1.text = upgrade1.upgradeName;
            text2.text = upgrade2.upgradeName;
            
            if (description1 != null) description1.text = upgrade1.description;
            if (description2 != null) description2.text = upgrade2.description;
            
            // Activar botones
            button1.interactable = true;
            button2.interactable = true;
            
            Debug.Log($"Upgrades cargados: {upgrade1.upgradeName} y {upgrade2.upgradeName}");
        }
        else if (upgrades.Count == 1)
        {
            upgrade1 = upgrades[0];
            text1.text = upgrade1.upgradeName;
            if (description1 != null) description1.text = upgrade1.description;
            button2.interactable = false;
            
            Debug.Log($"Solo 1 upgrade: {upgrade1.upgradeName}");
        }
    }

    private void SelectUpgrade(int upgradeNumber)
    {
        Debug.Log($"Botón {upgradeNumber} clickeado");
        
        if (!panelActive) return;
        
        UpgradeGeneral selectedUpgrade = upgradeNumber == 1 ? upgrade1 : upgrade2;
        
        if (selectedUpgrade != null)
        {
            Debug.Log($"Aplicando upgrade: {selectedUpgrade.upgradeName}");
            selectedUpgrade.Apply();
            Close();
        }
        else
        {
            Debug.LogError($"Upgrade {upgradeNumber} es null!");
        }
    }

    private void Close()
    {
        Debug.Log("Cerrando panel");
        
        // Reanudar juego
        Time.timeScale = 1f;
        
        // Desactivar panel
        gameObject.SetActive(false);
        panelActive = false;
        
        // Iniciar siguiente oleada inmediatamente
        StartNextWave();
    }

    private void StartNextWave()
    {
        // NO usar coroutine - llamar directamente
        if (WaveManager.instance != null)
        {
            Debug.Log("Iniciando siguiente oleada");
            WaveManager.instance.StartWave();
        }
        else
        {
            Debug.LogError("WaveManager.instance es null!");
        }
    }
}