using Elysium.Utils.Attributes;
using System.Collections.Generic;
using UnityEngine;

namespace Elysium.Items
{
    public static class RandomItemSelection
    {
        public static List<ItemStack> OpenMultiChest(Dictionary<IInventoryElement, float> _data)
        {
            List<ItemStack> contents = new List<ItemStack>();

            foreach (var drop in _data)
            {
                float r = UnityEngine.Random.Range(0f, 100f);
                if (r < drop.Value)
                {
                    contents.Add(new ItemStack(drop.Key, 1));
                }
            }

            return contents;
        }

        public static List<ItemStack> OpenUniqueChest(Dictionary<IInventoryElement, float> _data)
        {
            List<ItemStack> contents = new List<ItemStack>();
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
                    contents.Add(new ItemStack(drop.Key, 1));
                    return contents;
                }

                minChance += drop.Value;
            }

            throw new System.Exception("NO VALID ITEMS FOUND!");
        }
    }
}