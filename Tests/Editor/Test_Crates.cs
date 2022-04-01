using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Elysium.Items.Tests
{
    public class Test_Crates
    {
        [SetUp]
        public void Cleanup()
        {
            EditorSceneManager.NewScene(NewSceneSetup.EmptyScene);
            PlayerPrefs.DeleteAll();
        }

        [Test]
        public void TestRandomItemSelectionUniqueCrateWithAvailableItems()
        {
            IItem item1 = new GenericItem();

            var rewards1 = RandomItemSelection.OpenUniqueCrate(new Dictionary<IItem, float>() { { item1, 1 } });
            Assert.NotZero(rewards1.Count());
            Assert.AreEqual(item1, rewards1.First().Item);
        }

        [Test]
        public void TestRandomItemSelectionUniqueCrateZeroItems()
        {
            var rewards2 = RandomItemSelection.OpenUniqueCrate(new Dictionary<IItem, float>());
            Assert.Zero(rewards2.Count());
        }

        [Test]
        public void TestRandomItemSelectionMultiCrateWithAvailableItems()
        {
            IItem item1 = new GenericItem();
            IItem item2 = new GenericItem();
            IItem item3 = new GenericItem();

            var rewards1 = RandomItemSelection.OpenMultiCrate(new Dictionary<IItem, float>()
            {
                { item1, 1 },
                { item2, 1 },
                { item3, 1 },
            });

            Assert.AreEqual(3, rewards1.Count());
            Assert.NotNull(rewards1.FirstOrDefault(x => x.Item == item1));
            Assert.NotNull(rewards1.FirstOrDefault(x => x.Item == item2));
            Assert.NotNull(rewards1.FirstOrDefault(x => x.Item == item3));

            var rewards2 = RandomItemSelection.OpenMultiCrate(new Dictionary<IItem, float>()
            {
                { item1, 1 },
                { item2, 1 },
                { item3, 0 },
            });

            Assert.AreEqual(2, rewards2.Count());
            Assert.NotNull(rewards2.FirstOrDefault(x => x.Item == item1));
            Assert.NotNull(rewards2.FirstOrDefault(x => x.Item == item2));
        }

        [Test]
        public void TestRandomItemSelectionMultiCrateZeroItems()
        {
            var rewards = RandomItemSelection.OpenMultiCrate(new Dictionary<IItem, float>() { });
            Assert.Zero(rewards.Count());
        }
    }
}