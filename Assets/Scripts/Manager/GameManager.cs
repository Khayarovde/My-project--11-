using System.Collections; // <-- обязательно
using TheDeveloperTrain.SciFiGuns;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public PlayerMovement player;
    public Room startingRoom;
    public ScreenFader screenFader;
    private Door nearbyDoor;
    private bool isTransitioning = false;

    private void Start()
    {
        startingRoom.ActivateRoom();
        player.currentRoom = startingRoom;
    }

    private void Update()
    {
        if (isTransitioning) return;

        DetectNearbyDoor();

        if (nearbyDoor != null && Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine(TransitionRoom(nearbyDoor));
        }

        if (Input.GetKey(KeyCode.Mouse0))
        {
            Gun[] guns = GameObject.FindObjectsByType<Gun>(FindObjectsSortMode.InstanceID);
            foreach (var gun in guns)
            {
                gun.Shoot();
            }
        }
    }

    private void DetectNearbyDoor()
    {
        nearbyDoor = null;
        float interactDistance = 2f;
        Collider[] hits = Physics.OverlapSphere(player.transform.position, interactDistance);

        foreach (var hit in hits)
        {
            Door door = hit.GetComponent<Door>();
            if (door != null && door.connectedRoom != null)
            {
                nearbyDoor = door;
                break;
            }
        }
    }

    private IEnumerator TransitionRoom(Door door)
    {
        yield return StartCoroutine(screenFader.FadeToBlack());
        isTransitioning = true;

        player.enabled = false;
        player.controller.enabled = false;

        player.currentRoom.DeactivateRoom();

        player.transform.position = door.playerSpawnPoint.position;
        player.currentRoom = door.connectedRoom;

        door.connectedRoom.ActivateRoom();

        yield return new WaitForSeconds(0.3f); // <-- если это нужно, оставьте

        player.controller.enabled = true;
        player.enabled = true;

        isTransitioning = false;
        yield return StartCoroutine(screenFader.FadeFromBlack());
    }
}


