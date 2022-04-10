using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Elysium.Items.UI
{
    public class ItemTooltip : MonoBehaviour, ITooltip
    {
        [SerializeField] private TMP_Text title = default;
        [SerializeField] private TMP_Text description = default;
        [SerializeField] private Image icon = default;
        [SerializeField] private Button use = default;
        [SerializeField] private Button cancel = default;

        public event UnityAction OnOpen = delegate { };
        public event UnityAction OnClose = delegate { };

        public void Open(IItem _item, UnityAction _action)
        {
            title.text = _item.Name;
            description.text = _item.Description;
            icon.sprite = _item.Icon;

            cancel.onClick.RemoveAllListeners();
            cancel.onClick.AddListener(Close);

            use.onClick.RemoveAllListeners();
            use.onClick.AddListener(_action);
            use.onClick.AddListener(Close);

            gameObject.SetActive(true);
            OnOpen?.Invoke();
        }

        public void Close()
        {
            gameObject.SetActive(false);
            OnClose?.Invoke();
        }
    }
}