using System;
using ApplicationManagers;
using Characters;
using Settings;
using System.Collections.Generic;
using UnityEngine;
using Utility;

namespace CustomLogic
{
    class CustomLogicInputBuiltin: CustomLogicBaseBuiltin
    {
        public CustomLogicInputBuiltin(): base("Input")
        {
        }

        public override object CallMethod(string name, List<object> parameters)
        {
            var keyHold = name == "GetKeyHold";
            var keyDown = name == "GetKeyDown";
            var keyUp = name == "GetKeyUp";
            
            if (keyHold || keyDown || keyUp)
            {
                string key = (string)parameters[0];

                if (key.Contains('/'))
                {
                    string[] strArr = key.Split('/');
                    KeybindSetting keybind =
                        (KeybindSetting)((SaveableSettingsContainer)SettingsManager.InputSettings.Settings[strArr[0]])
                        .Settings[strArr[1]];

                    if (keyDown)
                        return keybind.GetKeyDown();
                    if (keyUp)
                        return keybind.GetKeyUp();
                    if (keyHold)
                        return keybind.GetKey();
                }
                else if (Enum.TryParse(key, true, out KeyCode keyCode))
                {
                    if (keyDown)
                        return Input.GetKeyDown(keyCode);
                    if (keyUp)
                        return Input.GetKeyUp(keyCode);
                    if (keyHold)
                        return Input.GetKey(keyCode);
                }

                return null;
            }
            if (name == "GetKeyName")
            {
                string key = (string)parameters[0];
                string[] strArr = key.Split('/');
                KeybindSetting keybind = (KeybindSetting)((SaveableSettingsContainer)SettingsManager.InputSettings.Settings[strArr[0]]).Settings[strArr[1]];
                return keybind.ToString();
            }
            if (name == "GetMouseAim")
            {
                RaycastHit hit;
                Ray ray = SceneLoader.CurrentCamera.Camera.ScreenPointToRay(Input.mousePosition);
                Vector3 target = ray.origin + ray.direction * 1000f;
                if (Physics.Raycast(ray, out hit, 1000f, Human.AimMask.value))
                    target = hit.point;
                return new CustomLogicVector3Builtin(target);
            }
            return base.CallMethod(name, parameters);
        }
    }
}
