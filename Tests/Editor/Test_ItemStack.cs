using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.TestTools;

namespace Elysium.Items.Tests
{
    public class Test_ItemStack
    {
        [SetUp]
        public void Cleanup()
        {
            EditorSceneManager.NewScene(NewSceneSetup.EmptyScene);
            PlayerPrefs.DeleteAll();
        }

        [Test]
        public void TestAddItem()
        {
            IItem item1 = new GenericItem();
            var stacks = new IItemStack[]
            {
                ItemStack.WithContents(item1, 0),
                ItemStack.WithContents(item1, 346),
            };

            foreach (var stack in stacks)
            {
                int onValueChangedTriggers = 0;
                void TriggerOnValueChanged() => onValueChangedTriggers++;
                stack.OnValueChanged += TriggerOnValueChanged;

                foreach (var number in Enumerable.Range(1, 10).ToList())
                {
                    onValueChangedTriggers = 0;
                    int prev = stack.Quantity;
                    stack.Add(number);
                    Assert.AreEqual(stack.Quantity, prev+number);
                    Assert.AreEqual(1, onValueChangedTriggers);
                }

                stack.OnValueChanged -= TriggerOnValueChanged;
            }
        }

        [Test]
        public void TestRemoveItem()
        {
            IItem item1 = new GenericItem();
            var stacks = new IItemStack[]
            {
                ItemStack.WithContents(item1, 0),
                ItemStack.WithContents(item1, 1),
                ItemStack.WithContents(item1, 5),
                ItemStack.WithContents(item1, 346),
            };

            foreach (var stack in stacks)
            {
                int onValueChangedTriggers = 0;
                void TriggerOnValueChanged() => onValueChangedTriggers++;
                stack.OnValueChanged += TriggerOnValueChanged;

                foreach (var number in Enumerable.Range(1, 10).ToList())
                {
                    onValueChangedTriggers = 0;
                    int prev = stack.Quantity;
                    stack.Remove(number);
                    int expected = Math.Max(0, prev - number);
                    Assert.AreEqual(stack.Quantity, expected);

                    if (prev != stack.Quantity) 
                    {
                        Assert.AreEqual(1, onValueChangedTriggers); 
                    }                    
                }

                stack.OnValueChanged -= TriggerOnValueChanged;
            }
        }

        [Test]
        public void TestSetItem()
        {
            IItem item1 = new GenericItem();
            var stacks = new IItemStack[]
            {
                ItemStack.New(),
                ItemStack.WithContents(new NullItem(), 1),
            };

            foreach (var stack in stacks)
            {
                int onValueChangedTriggers = 0;
                void TriggerOnValueChanged() => onValueChangedTriggers++;
                stack.OnValueChanged += TriggerOnValueChanged;

                Assert.AreNotEqual(stack.Item, item1);

                stack.Set(item1);
                Assert.AreEqual(stack.Item, item1);

                Assert.AreEqual(1, onValueChangedTriggers);
                stack.OnValueChanged -= TriggerOnValueChanged;
            }
        }

        [Test]
        public void TestSetQuantityToNonZero()
        {
            IItem item1 = new GenericItem();
            var stacks = new IItemStack[]
            {
                ItemStack.New(),
                ItemStack.WithContents(item1, 1),
                ItemStack.New(),
                ItemStack.WithContents(item1, 3463),
                ItemStack.New(),
                ItemStack.WithContents(item1, 46324),
            };

            foreach (var stack in stacks)
            {
                int onValueChangedTriggers = 0;
                void TriggerOnValueChanged() => onValueChangedTriggers++;
                stack.OnValueChanged += TriggerOnValueChanged;

                int randomItemAmount = UnityEngine.Random.Range(2, 9999);

                Assert.AreNotEqual(stack.Quantity, randomItemAmount);

                stack.Set(randomItemAmount);
                Assert.AreEqual(stack.Quantity, randomItemAmount);

                Assert.AreEqual(1, onValueChangedTriggers);
                stack.OnValueChanged -= TriggerOnValueChanged;
            }
        }

        [Test]
        public void TestSetQuantityToZero()
        {
            IItem item1 = new GenericItem();
            var stacks = new IItemStack[]
            {
                ItemStack.WithContents(item1, 1),
            };

            foreach (var stack in stacks)
            {
                int onValueChangedTriggers = 0;
                void TriggerOnValueChanged() => onValueChangedTriggers++;
                stack.OnValueChanged += TriggerOnValueChanged;

                Assert.False(stack.IsEmpty);
                Assert.AreNotEqual(stack.Item, new NullItem());
                Assert.AreNotEqual(stack.Quantity, 0);

                stack.Set(0);
                Assert.True(stack.IsEmpty);
                Assert.AreEqual(stack.Item, new NullItem());
                Assert.AreEqual(stack.Quantity, 0);

                Assert.AreEqual(1, onValueChangedTriggers);
                stack.OnValueChanged -= TriggerOnValueChanged;
            }
        }

        [Test]
        public void TestSetItemAndQuantity()
        {
            IItem item1 = new GenericItem();
            var stacks = new IItemStack[]
            {
                ItemStack.New(),
                ItemStack.WithContents(new NullItem(), 1),
                ItemStack.New(),
                ItemStack.WithContents(new NullItem(), 76547),
                ItemStack.New(),
                ItemStack.WithContents(new NullItem(), 784678),
            };

            foreach (var stack in stacks)
            {
                int onValueChangedTriggers = 0;
                void TriggerOnValueChanged() => onValueChangedTriggers++;
                stack.OnValueChanged += TriggerOnValueChanged;

                int randomItemAmount = UnityEngine.Random.Range(2, 9999);

                Assert.AreNotEqual(stack.Item, item1);
                Assert.AreNotEqual(stack.Quantity, randomItemAmount);

                stack.Set(item1, randomItemAmount);
                Assert.AreEqual(stack.Item, item1);
                Assert.AreEqual(stack.Quantity, randomItemAmount);

                Assert.AreEqual(1, onValueChangedTriggers);
                stack.OnValueChanged -= TriggerOnValueChanged;
            }
        }

        [Test]
        public void TestEmpty()
        {
            IItem item1 = new GenericItem();
            var stacks = new IItemStack[]
            {
                ItemStack.WithContents(item1, 1),
            };

            foreach (var stack in stacks)
            {
                int onValueChangedTriggers = 0;
                void TriggerOnValueChanged() => onValueChangedTriggers++;
                stack.OnValueChanged += TriggerOnValueChanged;

                Assert.False(stack.IsEmpty);
                Assert.AreNotEqual(stack.Item, new NullItem());
                Assert.AreNotEqual(stack.Quantity, 0);

                stack.Empty();
                Assert.True(stack.IsEmpty);
                Assert.AreEqual(stack.Item, new NullItem());
                Assert.AreEqual(stack.Quantity, 0);

                Assert.AreEqual(1, onValueChangedTriggers);
                stack.OnValueChanged -= TriggerOnValueChanged;
            }
        }

        [Test]
        public void TestSwap()
        {
            IItem item1 = new GenericItem(Guid.NewGuid().ToString(), Guid.NewGuid().ToString());
            IItem item2 = new GenericItem(Guid.NewGuid().ToString(), Guid.NewGuid().ToString());

            var swaps = new List<(ItemStack, ItemStack)>()
            {
                (ItemStack.WithContents(item1, 1), ItemStack.WithContents(item2, 1)),
                (ItemStack.WithContents(item1, 5), ItemStack.WithContents(item2, 263)),
                (ItemStack.WithContents(item1, 4), ItemStack.WithContents(item1, 16)),
                (ItemStack.New(), ItemStack.WithContents(item1, 16)),
            };

            foreach (var swap in swaps)
            {
                int stack1OnValueChangedTriggers = 0;
                int stack2OnValueChangedTriggers = 0;

                void TriggerStack1OnValueChanged() => stack1OnValueChangedTriggers++;
                void TriggerStack2OnValueChanged() => stack2OnValueChangedTriggers++;

                IItemStack stack1 = swap.Item1;
                IItemStack stack2 = swap.Item2;

                stack1.OnValueChanged += TriggerStack1OnValueChanged;
                stack2.OnValueChanged += TriggerStack2OnValueChanged;

                IItem itemS1 = stack1.Item;
                IItem itemS2 = stack2.Item;

                int qtyS1 = stack1.Quantity;
                int qtyS2 = stack2.Quantity;

                stack1.SwapContents(stack2);

                Assert.AreEqual(stack1.Item, itemS2);
                Assert.AreEqual(stack2.Item, itemS1);

                Assert.AreEqual(stack1.Quantity, qtyS2);
                Assert.AreEqual(stack2.Quantity, qtyS1);

                Assert.AreEqual(1, stack1OnValueChangedTriggers);
                Assert.AreEqual(1, stack2OnValueChangedTriggers);
                stack1.OnValueChanged -= TriggerStack1OnValueChanged;
                stack2.OnValueChanged -= TriggerStack2OnValueChanged;
            }
        }
    }
}