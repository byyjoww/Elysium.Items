using System.Collections.Generic;

namespace Elysium.Items
{
    public static class RandomItemSelection
    {
        public static IEnumerable<IItemStack> OpenMultiCrate(Dictionary<IItem, float> _data)
        {
            List<IItemStack> contents = new List<IItemStack>();
            foreach (var drop in _data)
            {
                float r = UnityEngine.Random.Range(0f, 1f);
                if (r < drop.Value)
                {
                    contents.Add(ItemStack.WithContents(drop.Key, 1));
                }
            }

            return contents;
        }

        public static IEnumerable<IItemStack> OpenUniqueCrate(Dictionary<IItem, float> _data)
        {
            List<IItemStack> contents = new List<IItemStack>();
            float totalProbability = 0;

            foreach (var drop in _data)
            {
                totalProbability += drop.Value;
            }

            float random = UnityEngine.Random.Range(0f, totalProbability);
            float minChance = 0;

            foreach (var drop in _data)
            {
                if (drop.Value == 0) { continue; }

                if (random >= minChance && random < minChance + drop.Value)
                {
                    contents.Add(ItemStack.WithContents(drop.Key, 1));
                    return contents;
                }

                minChance += drop.Value;
            }

            return contents;
        }
    }
}