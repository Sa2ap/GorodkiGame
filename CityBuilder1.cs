using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityBuilder1 : MonoBehaviour
{
    public GameObject cityBlockPrefab;
    public GameObject cityBlockType2Prefab;
    public GameObject cityBlockType3Prefab;
    public GameObject cityBlockType4Prefab;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            BuildCity(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            BuildCity(2);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            BuildCity(3);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            BuildCity(4);
        }
    }

    void BuildCity(int cityType)
    {
        DestroyPreviousCity();

        switch (cityType)
        {
            case 1:
                CreateCity(cityBlockPrefab, new Vector3(260.68f, -0.11f, 365.886f), Quaternion.Euler(-0.198f, 4.796f, 12.603f)); // Пример координат и вращения
                break;
            case 2:
                CreateCity(cityBlockType2Prefab, new Vector3(260.62f, 0.16f, 365.89f), Quaternion.Euler(-0.198f, 4.796f, 12.603f)); // Пример координат и вращения
                break;
            case 3:
                CreateCity(cityBlockType3Prefab, new Vector3(260.62f, 0.16f, 365.89f), Quaternion.Euler(-0.198f, 1.796f, 12.603f)); // Пример координат и вращения
                break;
            case 4:
                CreateCity(cityBlockType4Prefab, new Vector3(260.62f, 0.16f, 365.89f), Quaternion.Euler(-0.198f, 4.796f, 12.603f)); // Пример координат и вращения
                break;
            default:
                Debug.LogWarning("Неверный тип городка!");
                break;
        }
    }

    void CreateCity(GameObject blockPrefab, Vector3 spawnPosition, Quaternion rotation)
    {
        Vector3 position = spawnPosition;
        GameObject cityBlock = Instantiate(blockPrefab, position, rotation);
    }

    void DestroyPreviousCity()
    {
        GameObject[] cityBlocks = GameObject.FindGameObjectsWithTag("CityBlock");
        foreach (GameObject block in cityBlocks)
        {
            Destroy(block);
        }
    }
}