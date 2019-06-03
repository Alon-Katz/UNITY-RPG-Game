using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
    public static EquipmentManager instance;

    Equipment[] currentEquipment;
    SkinnedMeshRenderer[] currentMeshes;
    InventoryManager inventory;
    public delegate void OnEquipmentChanged(Equipment newItem, Equipment oldItem);
    public OnEquipmentChanged onEquipmentChangedCallback;
    public SkinnedMeshRenderer targetMesh;
    public Equipment[] defaultItems;

    void Awake()
    {
        if (instance != null)
        {
            Debug.Log("Equipment is singleton!");
            return;
        }
        instance = this;
    }

    private void Start()
    {
        inventory = InventoryManager.instance;
        int numSlots = System.Enum.GetNames(typeof(EquipmentSlot)).Length;
        currentEquipment = new Equipment[numSlots];
        currentMeshes = new SkinnedMeshRenderer[numSlots];

        EquipDefaultItems();
    }

    public void Equip(Equipment newItem)
    {
        int slotIndex = (int)newItem.equipSlot;
        Equipment oldItem = Unequip(slotIndex);

        if (onEquipmentChangedCallback != null)
        {
            onEquipmentChangedCallback.Invoke(newItem, null);
        }
        SetEquipmentBlendShapes(newItem, 100);
        currentEquipment[slotIndex] = newItem;
        SkinnedMeshRenderer newMesh = Instantiate<SkinnedMeshRenderer>(newItem.mesh);
        newMesh.transform.parent = targetMesh.transform;

        newMesh.bones = targetMesh.bones;
        newMesh.rootBone = targetMesh.rootBone;
        currentMeshes[slotIndex] = newMesh;
    }

    public Equipment Unequip(int slotIndex)
    {
        if (currentEquipment[slotIndex] != null)
        {
            if (currentMeshes[slotIndex] != null)
            {
                Destroy(currentMeshes[slotIndex].gameObject);
            }
            Equipment oldItem = currentEquipment[slotIndex];
            SetEquipmentBlendShapes(oldItem, 0);
            inventory.Add(oldItem);
            currentEquipment[slotIndex] = null;

            if (onEquipmentChangedCallback != null)
            {
                onEquipmentChangedCallback.Invoke(null, oldItem);
            }

            return oldItem;
        }

        return null;
    }

    void SetEquipmentBlendShapes(Equipment item, int weight)
    {
        foreach (EquipmentMeshRegion blendShape in item.coverdMeshRegions)
        {
            targetMesh.SetBlendShapeWeight((int)blendShape, weight);
        }
    }

    void EquipDefaultItems()
    {
        foreach (Equipment item in defaultItems)
        {
            Equip(item);
        }
    }
}
