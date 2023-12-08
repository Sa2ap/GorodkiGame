using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Stic : MonoBehaviour
{
    public GameObject objectPrefab; // ������ �� ������ �������
    public float throwForce = 50f; // ���� ������
    public Transform throwPoint;
    private int stickCollisionCount = 0;
    private int touchCollisionCount = 0;
    private bool stickCollisionProcessed = false;
    private bool touchCollisionProcessed = false;

    void Start()
    {
        // ������������� ��������� ��������
        stickCollisionCount = 0;
        touchCollisionCount = 0;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // ���������, ���� �� ������ ����� ������ ����
        {
            ThrowNewObject();
        }
    }

    void ThrowNewObject()
    {
        // ������� ������ �� �������
        GameObject newObject = Instantiate(objectPrefab, transform.position, Quaternion.identity);
        newObject.transform.position = throwPoint.position;
        // �������� ��������� Rigidbody �������
        Rigidbody rb = newObject.GetComponent<Rigidbody>();
        newObject.transform.Rotate(Vector3.forward, 90f);
        Vector3 cameraForward = Camera.main.transform.forward;
        // ������� ������� ���� ������
        rb.AddForce(cameraForward * throwForce, ForceMode.Impulse);
        rb.AddTorque(Vector3.down * 10f, ForceMode.Impulse);
        // ������ ������ � ������� ����
        Renderer renderer = newObject.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material.color = Color.red;
        }

        // ��������� ���������� ��� ��������� ������������
        newObject.AddComponent<StickCollisionHandler>();
        newObject.AddComponent<TouchCollisionHandler>();

        // ������� ������ ����� 20 ������
        StartCoroutine(DestroyAfterDelay(newObject, 20f));
    }

    IEnumerator DestroyAfterDelay(GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(obj);
    }

    // ���������� ������������ � ��������� � ����� "stick"
    public void HandleStickCollision()

    {
        if (!touchCollisionProcessed)
        {
            stickCollisionCount++;
            WriteCollisionCountToFile("stick_collisions.txt", stickCollisionCount);
        }
    }

    // ���������� ������������ � ��������� � ����� "touch"
    public void HandleTouchCollision()
    {
        if (!touchCollisionProcessed)
        {
            touchCollisionCount++;
            WriteCollisionCountToFile("touch_collisions.txt", touchCollisionCount);
        }
    }

    // ������ ���������� ������������ � ����
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
        // ���������� ��� ���������� ���������
        ResetCollisionCountInFile("stick_collisions.txt");
        ResetCollisionCountInFile("touch_collisions.txt");
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

