using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public List<Door> doors;
    public List<Enemy> enemies;

    private void Awake()
    {
        if (doors == null || doors.Count == 0)
            doors = new List<Door>(GetComponentsInChildren<Door>());

        if (enemies == null || enemies.Count == 0)
            enemies = new List<Enemy>(GetComponentsInChildren<Enemy>());
    }

    public void ActivateRoom()
    {
        gameObject.SetActive(true);
        foreach (var enemy in enemies)
        {
            if (enemy != null)
                enemy.gameObject.SetActive(true);
        }
    }

    public void DeactivateRoom()
    {
        foreach (var enemy in enemies)
        {
            if (enemy != null)
                enemy.gameObject.SetActive(false);
        }
        gameObject.SetActive(false);
    }
}