using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEditor.SceneManagement;

namespace Elysium.Items.Tests
{
    public class Test_Inventory
    {
        [SetUp]
        public void Setup()
        {
            EditorSceneManager.NewScene(NewSceneSetup.EmptyScene);
        }

        [Test]
        public void TestAddItem()
        {
            IItem item1 = new Item();
            IInventory[] inventories = new IInventory[]
            {
                new UnlimitedInventory(),
                new LimitedInventory(new Capacity{ Default = 20 }),
            };

            foreach (var inventory in inventories)
            {
                Assert.AreEqual(true, inventory.Add(item1, 1));
                Assert.AreEqual(1, inventory.Quantity(item1));

                Assert.AreEqual(true, inventory.Add(item1, 6));
                Assert.AreEqual(7, inventory.Quantity(item1));
            }
        }

        [Test]
        public void TestRemoveItem()
        {
            IItem item1 = new Item();
            IInventory[] inventories = new IInventory[]
            {
                new UnlimitedInventory(),
                new LimitedInventory(new Capacity{ Default = 20 }),
            };

            foreach (var inventory in inventories)
            {
                Assert.AreEqual(true, inventory.Add(item1, 1));
                Assert.AreEqual(1, inventory.Quantity(item1));

                Assert.AreEqual(true, inventory.Remove(item1, 1));
                Assert.AreEqual(0, inventory.Quantity(item1));

                Assert.AreEqual(true, inventory.Add(item1, 8));
                Assert.AreEqual(8, inventory.Quantity(item1));

                Assert.AreEqual(true, inventory.Remove(item1, 5));
                Assert.AreEqual(3, inventory.Quantity(item1));

                Assert.AreEqual(true, inventory.Remove(item1, 3));
                Assert.AreEqual(0, inventory.Quantity(item1));
            }
        }

        [Test]
        public void TestContains()
        {
            IItem item1 = new Item();
            IInventory[] inventories = new IInventory[]
            {
                new UnlimitedInventory(),
                new LimitedInventory(new Capacity{ Default = 20 }),
            };

            foreach (var inventory in inventories)
            {
                Assert.AreEqual(true, inventory.Add(item1, 1));
                Assert.AreEqual(true, inventory.Contains(item1));
            }
        }

        [Test]
        public void TestQuantity()
        {
            IItem item1 = new Item();
            IInventory[] inventories = new IInventory[]
            {
                new UnlimitedInventory(),
                new LimitedInventory(new Capacity{ Default = 20 }),
            };

            foreach (var inventory in inventories)
            {
                Assert.AreEqual(true, inventory.Add(item1, 1));
                Assert.AreEqual(1, inventory.Quantity(item1));
                Assert.AreEqual(true, inventory.Add(item1, 3));
                Assert.AreEqual(4, inventory.Quantity(item1));
            }
        }

        [Test]
        public void TestEmpty()
        {
            IItem item1 = new Item();
            IItem item2 = new Item();
            IItem item3 = new Item();
            IInventory[] inventories = new IInventory[]
            {
                new UnlimitedInventory(),
                new LimitedInventory(new Capacity{ Default = 20 }),
            };

            foreach (var inventory in inventories)
            {
                Assert.AreEqual(true, inventory.Add(item1, 4));
                Assert.AreEqual(true, inventory.Add(item2, 2));
                Assert.AreEqual(true, inventory.Add(item3, 3));
                inventory.Empty();
                Assert.AreEqual(0, inventory.Quantity(item1));
                Assert.AreEqual(0, inventory.Quantity(item2));
                Assert.AreEqual(0, inventory.Quantity(item3));
            }
        }

        [Test]
        public void TestLimitedInventoryCapacity()
        {
            IItem item1 = new Item();

            IInventory inventory1 = new LimitedInventory(new Capacity{ Default = 10 });
            Assert.AreEqual(true, inventory1.Add(item1, 10));
            Assert.AreEqual(10, inventory1.Quantity(item1));
            Assert.AreEqual(false, inventory1.Add(item1, 1));
            Assert.AreEqual(10, inventory1.Quantity(item1));

            IInventory inventory2 = new LimitedInventory(new Capacity{ Default = 11 });
            Assert.AreEqual(true, inventory2.Add(item1, 10));
            Assert.AreEqual(10, inventory2.Quantity(item1));
            Assert.AreEqual(true, inventory2.Add(item1, 1));
            Assert.AreEqual(11, inventory2.Quantity(item1));
        }

        [Test]
        public void TestLimitedInventoryExpand()
        {
            IItem item1 = new Item();
            IItem item2 = new Item();

            LimitedInventory inventory = new LimitedInventory(new Capacity{ Default = 10 });
            Assert.AreEqual(true, inventory.Add(item1, 10));
            Assert.AreEqual(10, inventory.Quantity(item1));
            Assert.AreEqual(false, inventory.Add(item1, 1));
            Assert.AreEqual(10, inventory.Quantity(item1));

            Assert.AreEqual(true, inventory.Expand(3));
            Assert.AreEqual(10, inventory.Quantity(item1));

            Assert.AreEqual(true, inventory.Add(item2, 3));
            Assert.AreEqual(3, inventory.Quantity(item2));

            Assert.AreEqual(false, inventory.Add(item1, 1));
            Assert.AreEqual(10, inventory.Quantity(item1));

            Assert.AreEqual(true, inventory.Shrink(2, out IEnumerable<IItemStack> _excessItems));
            Assert.AreEqual(1, inventory.Quantity(item2));
            Assert.AreEqual(10, inventory.Quantity(item1));
            Assert.AreEqual(2, _excessItems.Count());
            foreach (var item in _excessItems)
            {
                Assert.AreEqual(item2, item.Item);
                Assert.AreEqual(1, item.Quantity);
            }            
        }
    }
}