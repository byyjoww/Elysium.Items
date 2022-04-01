using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using UnityEditor;

namespace Elysium.Items.Samples
{
    public class SampleCrateOpener : MonoBehaviour
    {
        [System.Serializable]
        public class RewardVisualsWrapper
        {
            public Text Text;
            public Image Icon;
        }

        [SerializeField] private RewardVisualsWrapper[] possibilitiesVisuals = new RewardVisualsWrapper[6];
        [SerializeField] private RewardVisualsWrapper[] rewardVisuals = new RewardVisualsWrapper[6];
        private Sprite defaultRewardSprite = default;

        private ICrate uniqueCrate = default;
        private ICrate multiCrate = default;

        private ICrate selected { get; set; }        

        private void Start()
        {
            defaultRewardSprite = rewardVisuals.First().Icon.sprite;

            var possibilities = new List<IItemDropData>()
            {
                new ItemDropData(new SampleItem("Item 1", "t_item1"), 0.3f),
                new ItemDropData(new SampleItem("Item 2", "t_item2"), 0.3f),
                new ItemDropData(new SampleItem("Item 3", "t_item3"), 0.3f),
                new ItemDropData(new SampleItem("Item 4", "t_item4"), 0.3f),
                new ItemDropData(new SampleItem("Item 5", "t_item5"), 0.3f),
                new ItemDropData(new SampleItem("Item 6", "t_item6"), 0.3f),
            };

            uniqueCrate = new UniqueCrate(possibilities);
            multiCrate = new MultiCrate(possibilities);

            for (int i = 0; i < possibilitiesVisuals.Length; i++)
            {
                possibilitiesVisuals[i].Icon.sprite = possibilities[i]?.Item.Icon;
                possibilitiesVisuals[i].Text.text = possibilities[i]?.Item.Name;
            }

            SelectUniqueCrate();
        }

        public void SelectUniqueCrate()
        {
            selected = uniqueCrate;
        }

        public void SelectMultiCrate()
        {
            selected = multiCrate;
        }

        public void Open()
        {
            IEnumerable<IItemStack> rewards = selected.Open();

            for (int i = 0; i < rewards.Count(); i++)
            {
                rewardVisuals[i].Icon.sprite = rewards.ElementAt(i)?.Item.Icon;
                rewardVisuals[i].Text.text = rewards.ElementAt(i)?.Item.Name;
            }

            for (int i = rewards.Count(); i < rewardVisuals.Length; i++)
            {
                rewardVisuals[i].Icon.sprite = defaultRewardSprite;
                rewardVisuals[i].Text.text = "";
            }
        }
    }
}

