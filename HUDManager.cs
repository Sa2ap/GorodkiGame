using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class HUDManager : MonoBehaviour
{
    public Text countText; // Ссылка на текстовый объект в UI
    public float updateInterval = 1f; // Интервал обновления данных из файла

    void Start()
    {
        StartCoroutine(UpdateCountRoutine());
    }

    IEnumerator UpdateCountRoutine()
    {
        while (true)
        {
            UpdateCountFromFile();
            yield return new WaitForSeconds(updateInterval);
        }
    }
    private void ResetCollisionCountInFile(string filename)
    {
        string filePath = Path.Combine(Application.dataPath, filename);

        // Обнуление файла
        using (StreamWriter writer = new StreamWriter(filePath, false))
        {
            writer.Write("0");
        }
    }

    void UpdateCountFromFile()
    {
        string filePath = Path.Combine(Application.dataPath, "touch_collisions.txt");

        if (File.Exists(filePath))
        {
            string content = File.ReadAllText(filePath);
            int count;
            if (int.TryParse(content, out count))
            {
                UpdateCountUI(count);
            }
            else
            {
                Debug.LogError("Failed to parse file content as an integer.");
            }
        }
        else
        {
            Debug.LogError("File not found: " + filePath);
        }
    }

    void UpdateCountUI(int count)
    {
        if (countText != null)
        {
            countText.text = "Выбито: " + count.ToString();
        }
    }
}