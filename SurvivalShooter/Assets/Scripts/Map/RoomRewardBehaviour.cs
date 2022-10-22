using UnityEngine;
using System.Collections;

public class RoomRewardBehaviour : MonoBehaviour
{
    private GameObject vfx;
    bool _triggered;

    private void OnTriggerEnter(Collider other)
    {
        if (_triggered)
            return;

        if (other.gameObject.tag == "Player")
        {
            _triggered = true;
            vfx = Instantiate(CombatManager.instance.roomRewardVfx, other.transform.position + Vector3.up * 0.5f, Quaternion.identity, other.transform);
            StartCoroutine(ShowRoguelike());
        }
    }

    IEnumerator ShowRoguelike()
    {
        yield return new WaitForSeconds(2.8f);
        vfx.transform.SetParent(null);
        yield return new WaitForSeconds(2.5f);
        RoguelikeCombat.RoguelikeRewardSystem.instance.StartNewEvent();
    }
}
