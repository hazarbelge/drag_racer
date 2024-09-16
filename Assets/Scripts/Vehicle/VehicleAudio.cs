using UnityEngine;

namespace Vehicle
{
    [RequireComponent(typeof(VehicleAudio))]
    public class VehicleAudio : MonoBehaviour
    {
        [SerializeField] private AudioSource engineAudioSource;
        
        private VehicleModel _vehicleModel;
        
        public void Init(VehicleModel vehicleModel)
        {
            _vehicleModel = vehicleModel;
            _vehicleModel.SetVehicleAudio(this);
        }
        
        public void PlayEngineSound()
        {
            engineAudioSource.Play();
        }
        
        public void UpdateEngineSound()
        {
            engineAudioSource.pitch = Mathf.Lerp(0.5f, 2f, _vehicleModel.EngineRpm / _vehicleModel.MaxRpm);
            engineAudioSource.volume = Mathf.Lerp(0.75f, 1f, _vehicleModel.EngineRpm / _vehicleModel.MaxRpm);
        }
        
        public void StopEngineSound()
        {
            engineAudioSource.Stop();
            engineAudioSource.pitch = 1f;
            engineAudioSource.volume = 1f;
        }
    }
}