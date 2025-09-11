using UnityEngine;

public class BotEffects : MonoBehaviour
{
    private static readonly int s_baseColorId = Shader.PropertyToID("_BaseColor");

    [Header("Pickup Effect")]
    [SerializeField] private ParticleSystem _pickupParticle;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _pickupClip;

    public void PlayPickupEffect(Vector3 position)
    {
        _audioSource.Stop();
        _audioSource.PlayOneShot(_pickupClip);
        ParticleSystem pickupInstance = Instantiate(_pickupParticle, position, Quaternion.identity);
        pickupInstance.Play();
    }

    public void OverrideMaterialColor(Renderer[] renderers)
    {
        Color randomColor = Random.ColorHSV(0f, 1f, 0.8f, 1f, 0.8f, 1f);

        MaterialPropertyBlock materialPropertyBlock = new MaterialPropertyBlock();
        materialPropertyBlock.SetColor(s_baseColorId, randomColor);

        foreach (Renderer renderer in renderers)
        {
            renderer.SetPropertyBlock(materialPropertyBlock);
        }
    }
}
