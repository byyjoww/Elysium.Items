using Elysium.Utils;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Elysium.Items
{
    public static class InventoryFilters
    {
        // ADD HERE
        private static Dictionary<string, BaseFilter> AvailableFilters = new Dictionary<string, BaseFilter>()
        {
            { "ALL", new NullFilter() },
            { "CONSUMABLES", new TypeFilter<ConsumableSO>() },
            { "MISC", new TypeFilter<MiscItemSO>() },
        };

        public static BaseFilter GetEmptyFilter() => new NullFilter();

        public static BaseFilter GetFilterByKey(string _key) => AvailableFilters[_key];

        public static BaseFilter GetFilterAtPosition(int _position) => AvailableFilters.ElementAt(_position).Value;

        public static List<string> GetAllFilterNames() => AvailableFilters.Keys.ToList();

        // ----------------------------------------------- FILTERS ----------------------------------------------- //

        public abstract class BaseFilter
        {
            public abstract bool Evaluate(IInventoryElement _item);
        }

        public class NullFilter : BaseFilter
        {
            public override bool Evaluate(IInventoryElement _item)
            {
                return true;
            }
        }

        public class TypeFilter<T> : BaseFilter
        {
            public override bool Evaluate(IInventoryElement _item)
            {
                if (_item.GetType().IsSameOrSubclass(typeof(T)))
                {
                    return true;
                }

                return false;
            }
        }
    }    
}