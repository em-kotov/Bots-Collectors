using UnityEngine;
using TMPro;

[RequireComponent(typeof(LineRenderer))]
public class HomebaseEffects : MonoBehaviour
{
    [Header("Scan Effect")]
    [SerializeField] private ParticleSystem _scanParticle;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _scanClip;

    [Header("Counter UI")]
    [SerializeField] private TMP_Text _pearlCounter;

    [Header("Scan Radius")]
    [SerializeField] private LineRenderer _lineRenderer;
    [SerializeField, Min(3)] private int _segments = 64;

    public void PlayScanEffect()
    {
        _audioSource.Stop();
        _audioSource.PlayOneShot(_scanClip);
        _scanParticle.Stop();
        _scanParticle.Play();
    }

    public void UpdatePearlCounter(int pearlCount)
    {
        _pearlCounter.text = pearlCount.ToString();
    }

    public void SetScanRadius(float radius, float height)
    {
        _lineRenderer.positionCount = _segments;
        _lineRenderer.SetPositions(Vector3Extensions.GetPositionsInRadius(Vector3.zero, radius, height, _segments).ToArray());
    }
}
