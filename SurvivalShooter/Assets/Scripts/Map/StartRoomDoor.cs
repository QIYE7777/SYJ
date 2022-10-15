using UnityEngine;
using DG.Tweening;

public class StartRoomDoor : MonoBehaviour
{
    public Transform door;
    public Vector3 doorOpenRotation;

    // Use this for initialization
    void Start()
    {
        door.DORotate(doorOpenRotation, 1.2f).SetDelay(1.3f).SetEase(Ease.OutBack);
    }
}
