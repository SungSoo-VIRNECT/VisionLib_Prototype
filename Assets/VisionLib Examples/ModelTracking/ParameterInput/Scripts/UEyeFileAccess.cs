using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Visometry.VisionLib.SDK.Core;
using Visometry.VisionLib.SDK.Core.API;
using Visometry.VisionLib.SDK.Core.Details;

namespace Visometry.VisionLib.SDK.Examples
{
    public class UEyeFileAccess : MonoBehaviour
    {
        public InputField inputField;
        private void Awake()
        {
            if (this.inputField == null)
            {
                this.inputField = GetComponent<InputField>();
            }
        }

        public void Load()
        {
            if (this.inputField == null)
            {
                LogHelper.LogWarning("'InputField' is null", this);
                return;
            }
            TrackingManager.CatchCommandErrors(
                UEyeCameraCommands.LoadParametersFromFileAsync(
                    TrackingManager.Instance.Worker,
                    this.inputField.text));
        }
        
        public void Save()
        {
            if (this.inputField == null)
            {
                LogHelper.LogWarning("'InputField' is null", this);
                return;
            }

            TrackingManager.CatchCommandErrors(
                UEyeCameraCommands.SaveParametersToFileAsync(
                    TrackingManager.Instance.Worker,
                    this.inputField.text));
        }
    }
}
