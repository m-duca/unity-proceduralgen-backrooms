using FMOD.Studio;
using FMODUnity;
using System;
using System.Collections;
using UnityEngine;
using STOP_MODE = FMOD.Studio.STOP_MODE;

namespace Backrooms
{
    public class AudioManager : MonoBehaviour
    {
        [Header("Parameters")]

        [Header("Initial Volumes")]
        [SerializeField, Range(0f, 1f)] private float _masterDefaultVolume;
        [SerializeField, Range(0f, 1f)] private float _ambienceDefaultVolume;
        [SerializeField, Range(0f, 1f)] private float _sfxDefaultVolume;

        [Header("FMOD Events")]
        [SerializeField] private EventReference[] _ambienceEvents;

        // Not serialized
        private EventInstance _ambienceInstance;

        private Bus _masterBus;
        private Bus _ambienceBus;
        private Bus _sfxBus;

        private const string BUS_MASTER = "bus:/";
        private const string BUS_AMBIENCE = "bus:/Ambience";
        private const string BUS_SFX = "bus:/SFX";

        public static AudioManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                return;
            }

            Destroy(gameObject);
        }

        private IEnumerator Start()
        {
            yield return new WaitForSeconds(0.1f);

            _masterBus = RuntimeManager.GetBus(BUS_MASTER);
            _ambienceBus = RuntimeManager.GetBus(BUS_AMBIENCE);
            _sfxBus = RuntimeManager.GetBus(BUS_SFX);
            SetInitialVolumes();
        }

        #region Volume

        public void SetInitialVolumes()
        {
            SetMasterBus(_masterDefaultVolume);
            SetAmbienceBus(_ambienceDefaultVolume);
            SetSFXBus(_sfxDefaultVolume);
        }

        public void SetMasterBus(float volumeValue)
        {
            float clamped = Mathf.Clamp01(volumeValue);
            _masterBus.setVolume(clamped);
        }

        public void SetAmbienceBus(float volumeValue)
        {
            float clamped = Mathf.Clamp01(volumeValue);
            _ambienceBus.setVolume(clamped);
        }

        public void SetSFXBus(float volumeValue)
        {
            float clamped = Mathf.Clamp01(volumeValue);
            _sfxBus.setVolume(clamped);
        }

        #endregion

        #region SFX

        public void PlayOneShot(EventReference eventRefValue, Vector3? positionValue = null)
        {
            if (positionValue.HasValue)
                RuntimeManager.PlayOneShot(eventRefValue, positionValue.Value);
            else
                RuntimeManager.PlayOneShot(eventRefValue);
        }

        public void PlayOneShotAttached(EventReference eventRefValue, GameObject targetValue)
        {
            RuntimeManager.PlayOneShotAttached(eventRefValue, targetValue);
        }

        public EventInstance CreateEventInstance(EventReference eventRefValue)
        {
            return RuntimeManager.CreateInstance(eventRefValue);
        }

        #endregion

        #region Ambience

        public void PlayAmbience(AmbienceTrackType trackValue)
        {
            EventReference ambience = _ambienceEvents[(int)trackValue];

            if (ambience.IsNull)
            {
                Debug.LogError("Ambience Track invalid!");
                return;
            }

            PlayTrack(ref _ambienceInstance, ambience);
        }

        public void StopAmbience()
        {
            StopTrack(ref _ambienceInstance);
        }

        public void PauseAmbience() => PauseTrack(ref _ambienceInstance);

        #endregion

        #region General

        private void StopTrack(ref EventInstance instanceValue)
        {
            if (!instanceValue.isValid()) return;

            instanceValue.stop(STOP_MODE.ALLOWFADEOUT);
            instanceValue.release();
        }

        private void PauseTrack(ref EventInstance instanceValue)
        {
            if (!instanceValue.isValid()) return;

            instanceValue.setPaused(true);
        }

        private void ResumeTrack(ref EventInstance instanceValue)
        {
            if (!instanceValue.isValid()) return;

            instanceValue.setPaused(false);
        }

        private void PlayTrack(ref EventInstance instanceValue, EventReference referenceValue)
        {
            StopTrack(ref instanceValue);

            instanceValue = CreateEventInstance(referenceValue);

            Camera mainCam = Camera.main;
            if (mainCam != null)
                RuntimeManager.AttachInstanceToGameObject(instanceValue, mainCam.transform);
            else
                Debug.LogError("Failed finding Main Camera!");

            instanceValue.start();
            instanceValue.release();
        }

        #endregion
    }
}
