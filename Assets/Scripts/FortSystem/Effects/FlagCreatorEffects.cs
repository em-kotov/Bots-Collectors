using UnityEngine;

public class FlagCreatorEffects : MonoBehaviour
{
    [SerializeField] private ParticleSystem _fortSelectEffect;

    public void PlayFortSelectEffect()
    {
        _fortSelectEffect.Play();
    }

    public void PlayFlagPlacementEffect()
    {
        _fortSelectEffect.Stop();
    }

    public void PlayCancelEffect()
    {
        _fortSelectEffect.Stop();
    }
}
