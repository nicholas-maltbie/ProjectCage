using Mirror;
using UnityEngine;
using Scripts.Items;
using System.Collections;

namespace Scripts.Character
{
    public class HoldObject : NetworkBehaviour
    {
        public ItemLibrary itemLibrary;

        public Transform holdingTransform;

        [SyncVar(hook = nameof(OnChangeEquipment))]
        public Item heldItem;

        public void OnChangeEquipment(Item oldItem, Item newItem)
        {
            StartCoroutine(ChangeItem(newItem));
        }

        public IEnumerator ChangeItem(Item item)
        {
            while (holdingTransform.childCount > 0)
            {
                Destroy(holdingTransform.GetChild(0).gameObject);
                yield return null;
            }

            if (item != Item.None && item != Item.Player)
            {
                Instantiate(itemLibrary.GetItem(item), holdingTransform);
            }
        }

        [Command]
        public void CmdSetHeldItem(Item selectedItem)
        {
            this.heldItem = selectedItem;
        }

        public void Update()
        {
            if (!isLocalPlayer)
            {
                return;
            }

            if (Input.GetKeyDown(KeyCode.Q) && heldItem != Item.None)
                CmdSetHeldItem(Item.None);
            if (Input.GetKeyDown(KeyCode.E) && heldItem != Item.Bamboo)
                CmdSetHeldItem(Item.Bamboo);
            if (Input.GetKeyDown(KeyCode.R) && heldItem != Item.Fish)
                CmdSetHeldItem(Item.Fish);
        }
    }
}