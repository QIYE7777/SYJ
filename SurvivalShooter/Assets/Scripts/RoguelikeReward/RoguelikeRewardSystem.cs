using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;
using System.Collections;

namespace RoguelikeCombat
{
    public class RoguelikeRewardSystem : MonoBehaviour
    {
        public static RoguelikeRewardSystem instance;
        public RoguelikeRewardConfig config;

        public List<RoguelikeUpgradeId> perks;

        private void Awake()
        {
            instance = this;
            perks = new List<RoguelikeUpgradeId>();
        }

        public void StartNewEvent()
        {
            var data = new RoguelikeRewardEventData();
            data.title = "Choose a Upgrade!";

            var availableRewardPool = config.roguelikeRewards;
            data.rewards.Add(availableRewardPool[0]);
            data.rewards.Add(availableRewardPool[1]);
            data.rewards.Add(availableRewardPool[2]);
            RoguelikeRewardWindowBehaviour.instance.Setup(data);
            RoguelikeRewardWindowBehaviour.instance.Show();
            com.GameTime.timeScale = 0;
            PlayerBehaviour.instance.move.anim.SetBool("IsWalking", false);
        }

        public void OnEventFinished()
        {
            StartCoroutine(EventFishCoroutine());
        }

        IEnumerator EventFishCoroutine()
        {
            com.GameTime.timeScale = 1;

            var player = PlayerBehaviour.instance;
            var playerTrans = player.move.transform;
        
            player.move.disableMove = true;
            player.shooting.enabled = false;
            player.shootSuper.enabled = false;
            yield return new WaitForSeconds(0.1f);
            //Debug.Log("TODO door show and take you up");
            var door = Instantiate(CombatManager.instance.levelStartDoor);
            var spawnPosition = playerTrans.position;
            spawnPosition.y = 8;
            spawnPosition.z += 2f;
            door.transform.position = spawnPosition;
            door.transform.DOMoveY(0, 1.0f).SetEase(Ease.InCubic);

            yield return new WaitForSeconds(1.0f);
            var doorBehaviour = door.GetComponent<StartRoomDoor>();


            yield return new WaitForSeconds(0.4f);
            doorBehaviour.OpenDoor();
            player.move.cc.enabled = true;
            player.move.simulateMoveForward = true;

            yield return new WaitForSeconds(0.8f);

            player.transform.SetParent(doorBehaviour.transform);
            player.move.disableMove = false;
            player.move.simulateMoveForward = false;
            player.move.cc.enabled = false;
            doorBehaviour.CloseDoor();
            yield return new WaitForSeconds(0.8f);
            CameraFollow.instance.SyncPos(player.transform.position);
            CameraFollow.instance.enabled = false;
            door.transform.DOMoveY(9, 1.2f).SetEase(Ease.InCubic);
            yield return new WaitForSeconds(1.3f);
            SceneSwitcher.instance.SwitchToNextRoom();
        }

        public void AddPerk(RoguelikeUpgradeId id)
        {
            perks.Add(id);
        }

        public bool HasPerk(RoguelikeUpgradeId id)
        {
            return perks.IndexOf(id) >= 0;
        }
    }
}
