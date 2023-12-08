using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class HUDbot : MonoBehaviour
{
    public Text countText; // Ссылка на текстовый объект в UI
    public float updateInterval = 10f; // Интервал обновления данных из файла
    private int clickCount = 0;

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

    void UpdateCountFromFile()
    {
        string filename = "random_score.txt";
        string filePath = Path.Combine(Application.dataPath, filename);

        if (clickCount >= 4)
        {
            // Генерируем случайное число от 0 до 20
            int randomScore = Random.Range(0, 21);

            // Записываем в файл
            File.WriteAllText(filePath, randomScore.ToString());

            // Обновляем UI с новым числом
            UpdateCountUI(randomScore);

            // Сбрасываем счетчик
            clickCount = 0;
        }
        else
        {
            if (File.Exists(filePath))
            {
                string content = File.ReadAllText(filePath);
                Debug.Log("File Content: " + content);

                if (int.TryParse(content, out int randomScore))
                {
                    UpdateCountUI(randomScore);
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
    }

    void UpdateCountUI(int randomScore)
    {
        if (countText != null)
        {
            countText.text = "Компьютер выбил: " + randomScore.ToString();
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            clickCount++;
        }
    }
    private void OnApplicationQuit()
    {
        // Вызывается при завершении программы
        ResetCollisionCountInFile("random_score.txt");
       
    }

    // Обнуление файла
    private void ResetCollisionCountInFile(string filename)
    {
        string filePath = Path.Combine(Application.dataPath, filename);

        // Обнуление файла
        using (StreamWriter writer = new StreamWriter(filePath, false))
        {
            writer.Write("0");
        }
    }
}
