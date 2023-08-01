using UnityEngine;
using UnityEngine.Serialization;
using Visometry.VisionLib.SDK.Core;

namespace Visometry.VisionLib.SDK.Examples
{
    /// <summary>
    ///  Enables switching between several models.
    /// </summary>
    /// @ingroup Examples
    [AddComponentMenu("VisionLib/Examples/Model Switcher")]
    public class ModelSwitcher : MonoBehaviour
    {
        [FormerlySerializedAs("modelTrackerBehaviour")]
        public ModelTracker modelTracker;
        public TrackingMesh[] models;

        // Index of activeModel
        private int activeModelIndex = 0;

        public void OnEnable()
        {
            TrackingManager.OnTrackerInitialized += Reset;
            TrackingManager.OnTrackerStopped += EnableActiveModelsMeshRender;
        }

        public void OnDisable()
        {
            TrackingManager.OnTrackerStopped -= EnableActiveModelsMeshRender;
            TrackingManager.OnTrackerInitialized -= Reset;
        }

        public void Start()
        {
            Reset();
        }

        /// <summary>
        ///  Activates the model specified by the index in Unity and the vlSDK.
        ///  The tracking will be reset after setting the new model.
        /// </summary>
        /// <param name="modelIndex">
        ///  Index of the model, which should be activated. The index has to be
        ///  between 0 and modelURIs.Lenght
        /// </param>
        public void SwitchToModel(int modelIndex)
        {
            if (modelIndex < 0 || modelIndex >= this.models.Length)
            {
                return;
            }

            this.activeModelIndex = modelIndex;

            // Only active model is visible
            for (var i = 0; i < this.models.Length; i++)
            {
                SetModelAndComponentsEnabled(this.models[i], i == this.activeModelIndex);
            }

            // Reset tracking, so new model can now be tracked
            this.modelTracker.ResetTrackingHard();
        }

        /// <summary>
        ///  Switches to the next model in the modelURIs array.
        /// </summary>
        public void NextModel()
        {
            this.activeModelIndex++;
            if (this.activeModelIndex >= this.models.Length)
            {
                this.activeModelIndex = 0;
            }

            SwitchToModel(this.activeModelIndex);
        }

        /// <summary>
        /// Resets the ModelSwitcher to display and track the first model in
        /// the modelURIs list.
        /// </summary>
        public void Reset()
        {
            SwitchToModel(this.activeModelIndex);
        }
        
        private static void SetModelAndComponentsEnabled(TrackingMesh model, bool enable)
        {
            model.enabled = enable;
            model.gameObject.SetActive(enable);
            model.GetComponent<MeshRenderer>().enabled = enable;
        }
        
        private void EnableActiveModelsMeshRender()
        {
            this.models[this.activeModelIndex].GetComponent<MeshRenderer>().enabled = true;
        }
    }
}
