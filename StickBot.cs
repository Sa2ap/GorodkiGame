using UnityEngine;

public class StickBot : MonoBehaviour
{
    public GameObject stickPrefab; // ������ �����
    public Transform target; // ����, ���� ��� ����� ������� �����
    public float throwForce = 40f; // ���� ������ �����

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // ���������, ���� �� ������ ����� ������ ����
        {
            ThrowStick();
        }
    }

    void ThrowStick()
    {
        GameObject stick = Instantiate(stickPrefab, transform.position, Quaternion.identity); // ������� ������-�����
        Rigidbody stickRb = stick.GetComponent<Rigidbody>(); // �������� ��������� Rigidbody �����

        if (stickRb != null)
        {
            Vector3 throwDirection = (target.position - transform.position).normalized; // ���������� ����������� ������
            stickRb.AddForce(throwDirection * throwForce, ForceMode.Impulse); // ��������� ���� ������
        }
        else
        {
            Debug.LogError("Rigidbody component not found on the stick prefab.");
        }
    }
}
