using UnityEngine;
using System.IO;
using System.Collections;

public class GameResultManager : MonoBehaviour
{
    private int clickCount = 0;

    void Start()
    {
        StartCoroutine(CompareAndWriteResult());
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            clickCount++;
        }
    }

    IEnumerator CompareAndWriteResult()
    {
        while (true)
        {
            yield return new WaitUntil(() => clickCount >= 4);

            // ���� � ������
            string touchFilePath = Path.Combine(Application.dataPath, "touch_collisions.txt");
            string randomScoreFilePath = Path.Combine(Application.dataPath, "random_score.txt");
            string resultFilePath = Path.Combine(Application.dataPath, "game_result.txt");

            // ��������� ������ �� ������
            int touchCollisions = ReadIntFromFile(touchFilePath);
            int randomScore = ReadIntFromFile(randomScoreFilePath);

            // ���������� ��������
            string result;
            if (touchCollisions > randomScore)
            {
                result = "������������ �������";
            }
            else 
            {
                result = "��������� �������";
            }
            

            // ���������� ��������� � ����� ������ � ����
            AppendResultToFile(resultFilePath, result);

            // ���������� �������
            clickCount = 0;

            // ���� 5 ������ ����� ��������� ���������
            yield return new WaitForSeconds(5f);
        }
    }

    int ReadIntFromFile(string filePath)
    {
        if (File.Exists(filePath))
        {
            string content = File.ReadAllText(filePath);
            if (int.TryParse(content, out int value))
            {
                return value;
            }
            else
            {
                Debug.LogError($"Failed to parse file content as an integer: {filePath}");
            }
        }
        else
        {
            Debug.LogError($"File not found: {filePath}");
        }

        return 0;
    }

    void AppendResultToFile(string filePath, string result)
    {
        using (StreamWriter writer = new StreamWriter(filePath, true))
        {
            writer.WriteLine(result);
        }
    }
}
