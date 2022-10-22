using UnityEngine;
using DG.Tweening;

public class StartRoomDoor : MonoBehaviour
{
    public Transform door;
    public Vector3 doorOpenRotation;

    Vector3 _doorCloseRotation;

    private void Start()
    {
        _doorCloseRotation = door.localEulerAngles;
    }

    public void OpenDoor()
    {
        door.DOLocalRotate(doorOpenRotation, 1.1f).SetEase(Ease.OutBack);
    }

    public void CloseDoor()
    {
        door.DORotate(_doorCloseRotation, 1.1f).SetEase(Ease.InCubic);
    }
}
