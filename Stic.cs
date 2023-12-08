using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Stic : MonoBehaviour
{
    public GameObject objectPrefab; // Ссылка на префаб объекта
    public float throwForce = 50f; // Сила броска
    public Transform throwPoint;
    private int stickCollisionCount = 0;
    private int touchCollisionCount = 0;
    private bool stickCollisionProcessed = false;
    private bool touchCollisionProcessed = false;

    void Start()
    {
        // Инициализация начальных значений
        stickCollisionCount = 0;
        touchCollisionCount = 0;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Проверяем, была ли нажата левая кнопка мыши
        {
            ThrowNewObject();
        }
    }

    void ThrowNewObject()
    {
        // Создаем объект из префаба
        GameObject newObject = Instantiate(objectPrefab, transform.position, Quaternion.identity);
        newObject.transform.position = throwPoint.position;
        // Получаем компонент Rigidbody объекта
        Rigidbody rb = newObject.GetComponent<Rigidbody>();
        newObject.transform.Rotate(Vector3.forward, 90f);
        Vector3 cameraForward = Camera.main.transform.forward;
        // Придаем объекту силу вперед
        rb.AddForce(cameraForward * throwForce, ForceMode.Impulse);
        rb.AddTorque(Vector3.down * 10f, ForceMode.Impulse);
        // Красим объект в красный цвет
        Renderer renderer = newObject.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material.color = Color.red;
        }

        // Добавляем коллайдеры для обработки столкновений
        newObject.AddComponent<StickCollisionHandler>();
        newObject.AddComponent<TouchCollisionHandler>();

        // Удаляем объект через 20 секунд
        StartCoroutine(DestroyAfterDelay(newObject, 20f));
    }

    IEnumerator DestroyAfterDelay(GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(obj);
    }

    // Обработчик столкновений с объектами с тегом "stick"
    public void HandleStickCollision()

    {
        if (!touchCollisionProcessed)
        {
            stickCollisionCount++;
            WriteCollisionCountToFile("stick_collisions.txt", stickCollisionCount);
        }
    }

    // Обработчик столкновений с объектами с тегом "touch"
    public void HandleTouchCollision()
    {
        if (!touchCollisionProcessed)
        {
            touchCollisionCount++;
            WriteCollisionCountToFile("touch_collisions.txt", touchCollisionCount);
        }
    }

    // Запись количества столкновений в файл
    private void WriteCollisionCountToFile(string filename, int collisionCount)
    {
        string filePath = Path.Combine(Application.dataPath, filename);
        File.WriteAllText(filePath, collisionCount.ToString());

    }
}

public class StickCollisionHandler : MonoBehaviour
{
    private Stic stic;

    void Start()
    {
        stic = FindObjectOfType<Stic>();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Stick"))
        {
            stic.HandleStickCollision();
        }
    }
}

public class TouchCollisionHandler : MonoBehaviour
{
    private Stic stic;

    void Start()
    {
        stic = FindObjectOfType<Stic>();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("touch"))
        {
            stic.HandleTouchCollision();
        }
    }


    private void OnApplicationQuit()
    {
        // Вызывается при завершении программы
        ResetCollisionCountInFile("stick_collisions.txt");
        ResetCollisionCountInFile("touch_collisions.txt");
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

