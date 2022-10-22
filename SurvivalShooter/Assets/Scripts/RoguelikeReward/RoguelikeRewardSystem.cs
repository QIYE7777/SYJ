using UnityEngine;
using System.Collections.Generic;

namespace RoguelikeCombat
{
    public class RoguelikeRewardSystem : MonoBehaviour
    {
        public static RoguelikeRewardSystem instance;
        public RoguelikeRewardConfig config;

        public List<RoguelikeUpgradeId> perks;

        StartRoomDoor door;

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
        }

        public void OnEventFinished()
        {
            //Debug.Log("TODO door show and take you up");
            door.transform.DOMoveY(0, 1.2f).SetEase(Ease.InCubic).OnComplete();
            yield return new WaitForSeconds(1.0f);
            door.OpenDoor();
            yield return new WaitForSeconds(0.2f);
            door.CloseDoor();


            com.GameTime.timeScale = 1;
            SceneSwitcher.instance.SwitchToNextRoom();
        }

        public void AddPerk(RoguelikeUpgradeId id)
        {
            perks.Add(id);

            var player = PlayerBehaviour.instance;
            switch (id)
            {
                case RoguelikeUpgradeId.None:
                    break;
                case RoguelikeUpgradeId.Leech_5:
                    player.shooting.hemophagia.healPerShoot = 5;
                    break;
                case RoguelikeUpgradeId.Leech_10:
                    player.shooting.hemophagia.healPerShoot = 10;
                    break;
            }
        }

        public bool HasPerk(RoguelikeUpgradeId id)
        {
            return perks.IndexOf(id) >= 0;
        }
    }
}
