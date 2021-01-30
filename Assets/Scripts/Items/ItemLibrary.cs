using UnityEngine;

namespace Scripts.Items
{
    [CreateAssetMenu(fileName = "ItemLibrary", menuName = "ScriptableObjects/ItemLibrary")]
    public class ItemLibrary : ScriptableObject
    {
        [SerializeField]
        public ItemObject[] items;

        public GameObject GetItem(Item item)
        {
            foreach(ItemObject itemObj in items)
            {
                if (itemObj.itemId == item)
                {
                    return itemObj.gameObject;
                }
            }
            return null;
        }
    }

    [System.Serializable]
    public class ItemObject
    {
        public Item itemId;
        public GameObject gameObject;
    }
}