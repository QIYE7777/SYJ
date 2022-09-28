using UnityEngine;

namespace RoguelikeCombat
{
    public class RoguelikeRewardSystem : MonoBehaviour
    {
        public static RoguelikeRewardSystem instance;
        public RoguelikeRewardConfig config;

        private void Awake()
        {
            instance = this;
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
    }
}
