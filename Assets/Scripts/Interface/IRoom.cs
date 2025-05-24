using UnityEngine.ProBuilder.Shapes;
using UnityEngine;

public interface IRoom
{
    string RoomId { get; }
    Transform RoomTransform { get; }
    IDoor[] Doors { get; }  // ⬅️ Интерфейсные двери
    void SetActive(bool isActive);
}
