using UnityEngine;
using System.Collections.Generic;
using UnityEditor;

public class CSVToEqDataScriptableObject : MonoBehaviour
{
    public TextAsset csvFile; // Attach your CSV file in the inspector
    public string savePath = "Assets/data/EqDataContainer.asset"; // Path for the ScriptableObject

    public void LoadCSV()
    {
        // Read the CSV data
        string[] data = csvFile.text.Split(new char[] { '\n' });

        // Create a new instance of EqDataContainer
        EqDataContainer eqDataContainer = ScriptableObject.CreateInstance<EqDataContainer>();

        // Parse the data and add to the list
        for (int i = 1; i < data.Length; i++) // Skip the header row
        {
            if (string.IsNullOrEmpty(data[i])) continue;

            string[] row = data[i].Split(',');

            EarthquakeData eqData = new EarthquakeData
            {
                time = row[0],
                xAcc = float.Parse(row[1]),
                yAcc = float.Parse(row[2])
            };

            eqDataContainer.earthquakeDataList.Add(eqData);
        }

        // Save as a single ScriptableObject asset
        AssetDatabase.CreateAsset(eqDataContainer, savePath);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
}
