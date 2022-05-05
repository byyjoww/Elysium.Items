using UnityEngine;

namespace Elysium.Items
{
    [System.Serializable]
    public class Capacity
    {
        [field: SerializeField] public int Default { get; set; } = 20;
        [field: SerializeField] public int Min { get; set; } = 0;
        [field: SerializeField] public int Max { get; set; } = int.MaxValue;
    }
}
