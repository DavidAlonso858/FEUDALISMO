using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager instance;
    
    private List<UpgradeGeneral> upgrades = new List<UpgradeGeneral>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            InitializeUpgrades();
        }
    }

    private void InitializeUpgrades()
    {
        upgrades.Clear();
        upgrades.Add(new UpgradeHealthBasic());
        upgrades.Add(new UpgradeHealthMedium());
        upgrades.Add(new UpgradeSpeed());
        upgrades.Add(new UpgradeCadency());
        
        Debug.Log($"Upgrades inicializados: {upgrades.Count}");
    }

    public List<UpgradeGeneral> GetTwoRandomUpgrades()
    {
        if (upgrades.Count < 2)
        {
            Debug.LogError("No hay suficientes upgrades!");
            return new List<UpgradeGeneral>();
        }
        
        List<UpgradeGeneral> result = new List<UpgradeGeneral>();
        List<UpgradeGeneral> tempList = new List<UpgradeGeneral>(upgrades);
        
        // Primer upgrade
        int index1 = Random.Range(0, tempList.Count);
        result.Add(tempList[index1]);
        tempList.RemoveAt(index1);
        
        // Segundo upgrade
        int index2 = Random.Range(0, tempList.Count);
        result.Add(tempList[index2]);
        
        return result;
    }
}