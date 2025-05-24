using UnityEngine;

public class Door : MonoBehaviour
{
    public Room connectedRoom;
    public Transform playerSpawnPoint;

    private void OnDrawGizmos()
    {
        if (connectedRoom != null && playerSpawnPoint != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, connectedRoom.transform.position);
            Gizmos.DrawWireSphere(playerSpawnPoint.position, 0.3f);
        }
    }
}