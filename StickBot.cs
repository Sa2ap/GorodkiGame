using UnityEngine;

public class StickBot : MonoBehaviour
{
    public GameObject stickPrefab; // Префаб палки
    public Transform target; // Цель, куда бот будет бросать палку
    public float throwForce = 40f; // Сила броска палки

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Проверяем, была ли нажата левая кнопка мыши
        {
            ThrowStick();
        }
    }

    void ThrowStick()
    {
        GameObject stick = Instantiate(stickPrefab, transform.position, Quaternion.identity); // Создаем префаб-палку
        Rigidbody stickRb = stick.GetComponent<Rigidbody>(); // Получаем компонент Rigidbody палки

        if (stickRb != null)
        {
            Vector3 throwDirection = (target.position - transform.position).normalized; // Определяем направление броска
            stickRb.AddForce(throwDirection * throwForce, ForceMode.Impulse); // Применяем силу броска
        }
        else
        {
            Debug.LogError("Rigidbody component not found on the stick prefab.");
        }
    }
}
