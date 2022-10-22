using UnityEngine;
using System.Collections.Generic;

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
        }

        public void OnEventFinished()
        {
            Debug.Log("TODO door show and take you up");
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
