using System.Collections;
using UnityEngine;

namespace Backrooms
{
    public class AmbienceTrackPlayer : MonoBehaviour
    {
        [Header("Parameters")]
        [SerializeField] private AmbienceTrackType _track;
        
        private IEnumerator Start() 
        {
            yield return new WaitForSeconds(0.2f);
            AudioManager.Instance.PlayAmbience(_track);
        }

        private void OnDestroy() => AudioManager.Instance.StopAmbience(_track);
    }
}
