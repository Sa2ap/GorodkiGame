using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class HUDbot : MonoBehaviour
{
    public Text countText; // ������ �� ��������� ������ � UI
    public float updateInterval = 10f; // �������� ���������� ������ �� �����
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
            // ���������� ��������� ����� �� 0 �� 20
            int randomScore = Random.Range(0, 21);

            // ���������� � ����
            File.WriteAllText(filePath, randomScore.ToString());

            // ��������� UI � ����� ������
            UpdateCountUI(randomScore);

            // ���������� �������
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
            countText.text = "��������� �����: " + randomScore.ToString();
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
        // ���������� ��� ���������� ���������
        ResetCollisionCountInFile("random_score.txt");
       
    }

    // ��������� �����
    private void ResetCollisionCountInFile(string filename)
    {
        string filePath = Path.Combine(Application.dataPath, filename);

        // ��������� �����
        using (StreamWriter writer = new StreamWriter(filePath, false))
        {
            writer.Write("0");
        }
    }
}
