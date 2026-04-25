using FMOD.Studio;
using FMODUnity;
using System;
using System.Collections;
using System.Collections.Generic;
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
        private readonly Dictionary<AmbienceTrackType, EventInstance> _ambienceInstances = new();

        private Bus _masterBus;
        private Bus _ambienceBus;
        private Bus _sfxBus;

        private const string BUS_MASTER = "bus:/";
        private const string BUS_AMBIENCE = "bus:/Ambience";
        private const string BUS_SFX = "bus:/SFX";

        public static AudioManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);

            _masterBus = RuntimeManager.GetBus(BUS_MASTER);
            _ambienceBus = RuntimeManager.GetBus(BUS_AMBIENCE);
            _sfxBus = RuntimeManager.GetBus(BUS_SFX);
        }

        private void Start() => SetInitialVolumes();

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
            if (_ambienceInstances.ContainsKey(trackValue))
                return;

            EventReference ambience = _ambienceEvents[(int)trackValue];

            if (ambience.IsNull)
            {
                Debug.LogError("Ambience Track invalid!");
                return;
            }

            EventInstance instance = PlayTrack(ambience);

            _ambienceInstances.Add(trackValue, instance);
        }

        public void StopAmbience(AmbienceTrackType trackValue)
        {
            if (!_ambienceInstances.TryGetValue(trackValue, out EventInstance instance))
                return;

            StopTrack(ref instance);
            _ambienceInstances.Remove(trackValue);
        }

        public void StopAllAmbiences()
        {
            foreach (EventInstance instance in _ambienceInstances.Values)
            {
                EventInstance temp = instance;
                StopTrack(ref temp);
            }

            _ambienceInstances.Clear();
        }

        public void PauseAllAmbiences()
        {
            foreach (EventInstance instance in _ambienceInstances.Values)
            {
                EventInstance temp = instance;
                PauseTrack(ref temp);
            }
        }

        public void ResumeAllAmbiences()
        {
            foreach (EventInstance instance in _ambienceInstances.Values)
            {
                EventInstance temp = instance;
                ResumeTrack(ref temp);
            }
        }

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

        private EventInstance PlayTrack(EventReference referenceValue)
        {
            EventInstance instance = CreateEventInstance(referenceValue);

            Camera mainCam = Camera.main;
            if (mainCam != null)
                RuntimeManager.AttachInstanceToGameObject(instance, mainCam.gameObject);
            else
                Debug.LogError("Failed finding Main Camera!");

            instance.start();

            return instance;
        }

        #endregion
    }
}
