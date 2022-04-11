using System;
using System.Collections.Generic;
using System.Linq;
using Elysium.Core;
using NUnit.Framework;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Elysium.Items.Tests
{
    public class Test_Inventory
    {
        UnlimitedInventorySO unltdInventoryAsset = AssetDatabase.LoadAssetAtPath<UnlimitedInventorySO>(unltdInventoryAssetPath);
        const string unltdInventoryAssetPath = "Packages/com.elysium.items/Tests/Editor/UnlimitedInventorySO_Test.asset";

        LimitedInventorySO ltdInventoryAsset => AssetDatabase.LoadAssetAtPath<LimitedInventorySO>(ltdInventoryAssetPath);
        const string ltdInventoryAssetPath = "Packages/com.elysium.items/Tests/Editor/LimitedInventorySO_Test.asset";

        [SetUp]
        public void Cleanup()
        {
            EditorSceneManager.NewScene(NewSceneSetup.EmptyScene);
            PlayerPrefs.DeleteAll();
            unltdInventoryAsset.Reset();
            ltdInventoryAsset.Reset();
        }

        #region COMMON
        [Test]
        public void TestAddItem()
        {
            IItem item1 = new GenericItem();
            IInventory[] inventories = new IInventory[]
            {
                UnlimitedInventory.New(),
                LimitedInventory.New(new Capacity{ Default = 20 }),
                UnlimitedInventorySO.New(),
                LimitedInventorySO.New(new Capacity{ Default = 20 }),
                unltdInventoryAsset,
                ltdInventoryAsset,
            };

            foreach (var inventory in inventories)
            {
                Assert.True(inventory.Add(item1, 1));
                Assert.AreEqual(1, inventory.Quantity(item1));

                Assert.True(inventory.Add(item1, 6));
                Assert.AreEqual(7, inventory.Quantity(item1));
            }
        }

        [Test]
        public void TestRemoveItem()
        {
            IItem item1 = new GenericItem();
            IInventory[] inventories = new IInventory[]
            {
                UnlimitedInventory.New(),
                LimitedInventory.New(new Capacity{ Default = 20 }),
                UnlimitedInventorySO.New(),
                LimitedInventorySO.New(new Capacity{ Default = 20 }),
                unltdInventoryAsset,
                ltdInventoryAsset,
            };

            foreach (var inventory in inventories)
            {
                Assert.True(inventory.Add(item1, 1));
                Assert.AreEqual(1, inventory.Quantity(item1));

                Assert.True(inventory.Remove(item1, 1));
                Assert.AreEqual(0, inventory.Quantity(item1));

                Assert.True(inventory.Add(item1, 8));
                Assert.AreEqual(8, inventory.Quantity(item1));

                Assert.True(inventory.Remove(item1, 5));
                Assert.AreEqual(3, inventory.Quantity(item1));

                Assert.True(inventory.Remove(item1, 3));
                Assert.AreEqual(0, inventory.Quantity(item1));
            }
        }

        [Test]
        public void TestContains()
        {
            IItem item1 = new GenericItem();
            IInventory[] inventories = new IInventory[]
            {
                UnlimitedInventory.New(),
                LimitedInventory.New(new Capacity{ Default = 20 }),
                UnlimitedInventorySO.New(),
                LimitedInventorySO.New(new Capacity{ Default = 20 }),
                unltdInventoryAsset,
                ltdInventoryAsset,
            };

            foreach (var inventory in inventories)
            {
                Assert.True(inventory.Add(item1, 1));
                Assert.True(inventory.Contains(item1));
            }
        }

        [Test]
        public void TestQuantity()
        {
            IItem item1 = new GenericItem();
            IInventory[] inventories = new IInventory[]
            {
                UnlimitedInventory.New(),
                LimitedInventory.New(new Capacity{ Default = 20 }),
                UnlimitedInventorySO.New(),
                LimitedInventorySO.New(new Capacity{ Default = 20 }),
                unltdInventoryAsset,
                ltdInventoryAsset,
            };

            foreach (var inventory in inventories)
            {
                Assert.True(inventory.Add(item1, 1));
                Assert.AreEqual(1, inventory.Quantity(item1));

                Assert.True(inventory.Add(item1, 3));
                Assert.AreEqual(4, inventory.Quantity(item1));
            }
        }

        [Test]
        public void TestEmpty()
        {
            IItem item1 = new GenericItem();
            IItem item2 = new GenericItem();
            IItem item3 = new GenericItem();
            IInventory[] inventories = new IInventory[]
            {
                UnlimitedInventory.New(),
                LimitedInventory.New(new Capacity{ Default = 20 }),
                UnlimitedInventorySO.New(),
                LimitedInventorySO.New(new Capacity{ Default = 20 }),
                unltdInventoryAsset,
                ltdInventoryAsset,
            };

            foreach (var inventory in inventories)
            {
                Assert.True(inventory.Add(item1, 4));
                Assert.True(inventory.Add(item2, 2));
                Assert.True(inventory.Add(item3, 3));

                inventory.Empty();

                Assert.AreEqual(0, inventory.Quantity(item1));
                Assert.AreEqual(0, inventory.Quantity(item2));
                Assert.AreEqual(0, inventory.Quantity(item3));
            }
        }

        [Test]
        public void TestSwap()
        {
            IItem item1 = new GenericItem();
            IItem item2 = new GenericItem();

            var unltdInv = UnlimitedInventory.New();
            unltdInv.Add(item1, 1);
            unltdInv.Add(item2, 1);

            var ltdInv = LimitedInventory.New(new Capacity { Default = 20 });
            ltdInv.Add(item1, 1);
            ltdInv.Add(item2, 1);

            var unltdInvSO = UnlimitedInventorySO.New();
            unltdInvSO.Add(item1, 1);
            unltdInvSO.Add(item2, 1);

            var ltdInvSO = LimitedInventorySO.New(new Capacity { Default = 10 });
            ltdInvSO.Add(item1, 1);
            ltdInvSO.Add(item2, 1);

            unltdInventoryAsset.Add(item1, 1);
            unltdInventoryAsset.Add(item2, 1);

            ltdInventoryAsset.Add(item1, 1);
            ltdInventoryAsset.Add(item2, 1);

            IInventory[] inventories = new IInventory[]
            {
                unltdInv,
                ltdInv,
                unltdInvSO,
                ltdInvSO,
                unltdInventoryAsset,
                ltdInventoryAsset,
            };

            foreach (var inventory in inventories)
            {
                Assert.AreEqual(2, inventory.Items.Stacks.Where(x => !x.IsEmpty).Count());

                IItemStack stack1 = inventory.Items.Stacks.ElementAt(0);
                Assert.AreEqual(item1, stack1.Item);
                Assert.AreEqual(1, stack1.Quantity);

                IItemStack stack2 = inventory.Items.Stacks.ElementAt(1);
                Assert.AreEqual(item2, stack2.Item);
                Assert.AreEqual(1, stack2.Quantity);

                stack1.SwapContents(stack2);

                IItemStack newStack1 = inventory.Items.Stacks.ElementAt(0);
                Assert.AreEqual(item2, newStack1.Item);
                Assert.AreEqual(1, newStack1.Quantity);

                IItemStack newStack2 = inventory.Items.Stacks.ElementAt(1);
                Assert.AreEqual(item1, newStack2.Item);
                Assert.AreEqual(1, newStack2.Quantity);
            }
        }

        [Test]
        public void TestEvents()
        {
            IItem item1 = new GenericItem(Guid.NewGuid().ToString(), Guid.NewGuid().ToString());
            IItem item2 = new GenericItem(Guid.NewGuid().ToString(), Guid.NewGuid().ToString());
            IInventory[] inventories = new IInventory[]
            {
                UnlimitedInventory.New(),
                LimitedInventory.New(new Capacity{ Default = 20 }),
                UnlimitedInventorySO.New(),
                LimitedInventorySO.New(new Capacity{ Default = 20 }),
                unltdInventoryAsset,
                ltdInventoryAsset,
            };

            foreach (var inventory in inventories)
            {
                var actions = new Action[]
                {
                    delegate { inventory.Add(item1, 1); },
                    delegate { inventory.Remove(item1, 1); },
                    delegate { inventory.Add(item1, 1); },
                    delegate { inventory.Add(item2, 1); },
                    delegate {
                        IItemStack stack1 = inventory.Items.Stacks.ElementAt(0);
                        IItemStack stack2 = inventory.Items.Stacks.ElementAt(1);
                        stack1.SwapContents(stack2);
                    },
                      delegate { inventory.Empty(); },
                };

                int onValueChangedTriggers = 0;
                void TriggerOnValueChanged() => onValueChangedTriggers++;
                inventory.OnValueChanged += TriggerOnValueChanged;

                foreach (var action in actions)
                {
                    onValueChangedTriggers = 0;
                    action?.Invoke();
                    Assert.Greater(onValueChangedTriggers, 0);                    
                }

                inventory.OnValueChanged -= TriggerOnValueChanged;
            }
        }

        [Test]
        public void TestPersistency()
        {
            string id = Guid.NewGuid().ToString();
            IItem item1 = new GenericItem(id, id);
            SaveSystem saveSystem = new SaveSystem();

            UnlimitedInventorySO[] unlimiteds = new UnlimitedInventorySO[]
            {                
                UnlimitedInventorySO.New(),
                // unltdInventoryAsset,
            };

            foreach (var inventory in unlimiteds)
            {
                Assert.True(inventory.Add(item1, 1));
                inventory.Save(saveSystem);

                inventory.Empty();
                Assert.AreEqual(0, inventory.Quantity(item1));

                inventory.Load(saveSystem);
                Assert.AreEqual(1, inventory.Quantity(item1));
            }

            LimitedInventorySO[] limiteds = new LimitedInventorySO[]
            {
                LimitedInventorySO.New(new Capacity { Default = 20 }),
                // ltdInventoryAsset,
            };

            foreach (var inventory in limiteds)
            {
                Assert.True(inventory.Add(item1, 1));
                Assert.True(inventory.Expand(1));
                Assert.AreEqual(21, inventory.Items.Stacks.Count());
                inventory.Save(saveSystem);

                inventory.Empty();
                Assert.AreEqual(0, inventory.Quantity(item1));
                inventory.Shrink(1, out _);
                Assert.AreEqual(20, inventory.Items.Stacks.Count());

                inventory.Load(saveSystem);
                Assert.AreEqual(1, inventory.Quantity(item1));
                Assert.AreEqual(21, inventory.Items.Stacks.Count());
            }     
        }        

        #endregion

        #region LIMITED_INVENTORY
        [Test]
        public void TestLimitedInventoryCapacity()
        {
            IItem item1 = new GenericItem();

            IInventory[] inventoriesOutOfCapacity = new IInventory[]
            {
                LimitedInventory.New(new Capacity{ Default = 10 }),
                LimitedInventorySO.New(new Capacity{ Default = 10 }),
                ltdInventoryAsset,
            };

            foreach (var inventory in inventoriesOutOfCapacity)
            {
                Assert.True(inventory.Add(item1, 10));
                Assert.AreEqual(10, inventory.Quantity(item1));

                Assert.False(inventory.Add(item1, 1));
                Assert.AreEqual(10, inventory.Quantity(item1));
            }

            ltdInventoryAsset.Reset();
            ltdInventoryAsset.Expand(1);

            IInventory[] inventoriesWithinCapacity = new IInventory[]
            {
                LimitedInventory.New(new Capacity{ Default = 11 }),
                LimitedInventorySO.New(new Capacity{ Default = 11 }),
                ltdInventoryAsset,
            };

            foreach (var inventory in inventoriesWithinCapacity)
            {
                Assert.True(inventory.Add(item1, 10));
                Assert.AreEqual(10, inventory.Quantity(item1));

                Assert.True(inventory.Add(item1, 1));
                Assert.AreEqual(11, inventory.Quantity(item1));
            }
        }

        [Test]
        public void TestLimitedInventoryExpand()
        {
            IItem item1 = new GenericItem();
            IItem item2 = new GenericItem();

            IInventory[] inventories = new IInventory[]
            {
                LimitedInventory.New(new Capacity{ Default = 10 }),
                LimitedInventorySO.New(new Capacity{ Default = 10 }),
                ltdInventoryAsset,
            };

            foreach (var inventory in inventories)
            {
                Assert.True(inventory.Add(item1, 10));
                Assert.AreEqual(10, inventory.Quantity(item1));

                Assert.False(inventory.Add(item1, 1));
                Assert.AreEqual(10, inventory.Quantity(item1));

                Assert.True((inventory as IExpandable).Expand(3));
                Assert.AreEqual(10, inventory.Quantity(item1));

                Assert.True(inventory.Add(item2, 3));
                Assert.AreEqual(3, inventory.Quantity(item2));

                Assert.False(inventory.Add(item1, 1));
                Assert.AreEqual(10, inventory.Quantity(item1));

                Assert.True((inventory as IExpandable).Shrink(2, out IEnumerable<IItemStack> _excessItems));
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
        #endregion
    }
}