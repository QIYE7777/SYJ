using UnityEngine;
using System.Collections;

public class RoomRewardBehaviour : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("TODO show effect feedback");
            RoguelikeCombat.RoguelikeRewardSystem.instance.StartNewEvent();
        }
    }
}
