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
            yield return new WaitForSeconds(1f);
            AudioManager.Instance.PlayAmbience(_track);
        }
    }
}
