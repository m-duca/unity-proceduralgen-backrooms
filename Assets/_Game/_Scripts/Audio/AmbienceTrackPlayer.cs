using UnityEngine;

namespace Backrooms
{
    public class AmbienceTrackPlayer : MonoBehaviour
    {
        [Header("Parameters")]
        [SerializeField] private AmbienceTrackType _track;
        
        private void Start() => AudioManager.Instance.PlayAmbience(_track);
    }
}
