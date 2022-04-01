using NUnit.Framework;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Elysium.Items.Tests
{
    public class Test_Currency
    {
        [SetUp]
        public void Cleanup()
        {
            EditorSceneManager.NewScene(NewSceneSetup.EmptyScene);
            PlayerPrefs.DeleteAll();
        }

        [Test]
        public void TestCreateCurrency()
        {
            var testCases = new (Currency, string, long)[]
            {
                (new Currency("coins", 0), "coins", 0),
                (new Currency("", 0), "", 0),
                (new Currency("coins", 50), "coins", 50),
                (new Currency("df8h4fdh51", -1), "df8h4fdh51", -1),
            };

            foreach (var testCase in testCases)
            {
                Assert.AreEqual(testCase.Item2, testCase.Item1.Name);
                Assert.AreEqual(testCase.Item3, testCase.Item1.Amount);
            }
        }

        [Test]
        public void TestGainCurrencyValidInput()
        {
            var testCases = new (Currency, long, long)[]
            {
                (new Currency("coins", 0), 0, 0),
                (new Currency("coins", 0), 20, 20),
                (new Currency("coins", 0), long.MaxValue, long.MaxValue),
                (new Currency("coins", -20), 100, 80),
            };

            foreach (var testCase in testCases)
            {
                testCase.Item1.Gain(testCase.Item2);
                Assert.AreEqual(testCase.Item3, testCase.Item1.Amount);
            }
        }

        [Test]
        public void TestGainCurrencyInvalidInput()
        {
            var testCases = new (Currency, long)[]
            {
                (new Currency("coins", -20), -1),
                (new Currency("coins", 100), -456456),
                (new Currency("coins", 0), -876),
            };

            foreach (var testCase in testCases)
            {
                Assert.Catch<System.ArgumentException>(() => testCase.Item1.Gain(testCase.Item2));
            }
        }

        [Test]
        public void TestDeductCurrencyValidInput()
        {
            var testCases = new (Currency, long, long)[]
            {
                (new Currency("coins", 0), 0, 0),
                (new Currency("coins", 0), 20, -20),
                (new Currency("coins", long.MaxValue), long.MaxValue, 0),
                (new Currency("coins", 2000), 100, 1900),
            };

            foreach (var testCase in testCases)
            {
                testCase.Item1.Deduct(testCase.Item2);
                Assert.AreEqual(testCase.Item3, testCase.Item1.Amount);
            }
        }

        [Test]
        public void TestRemoveCurrencyValidInput()
        {
            var testCases = new (Currency, long)[]
            {
                (new Currency("coins", -20), -1),
                (new Currency("coins", 100), -456456),
                (new Currency("coins", 0), -876),
            };

            foreach (var testCase in testCases)
            {
                Assert.Catch<System.ArgumentException>(() => testCase.Item1.Deduct(testCase.Item2));
            }
        }
    }
}