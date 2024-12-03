using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ResetDeviceBinding : MonoBehaviour
{
    [SerializeField] private InputActionAsset inputAction;
    [SerializeField] private string target;

    public void ResetAllBinding()
    {
        foreach(InputActionMap map in inputAction.actionMaps)
        {
            map.RemoveAllBindingOverrides();

        }
    }

    public void ResetControlSchemeBinding()
    {
        foreach (InputActionMap map in inputAction.actionMaps)
        {
            foreach (InputAction action in map.actions)
            {
                action.RemoveBindingOverride(InputBinding.MaskByGroup(target));

            }

        }
    }
}
