using UnityEngine;

public class VFXController
{
    private VFXView vfxView;

    public VFXController(VFXView vfxPrefab)
    {
        vfxView = Object.Instantiate(vfxPrefab);
        vfxView.SetController(this);
    }

    public void Configure(VFXType type, Vector3 spawnPosition) => vfxView.ConfigureAndPlay(type, spawnPosition);

    public void OnParticleEffectCompleted() => GameService.Instance.VFXService.ReturnVFXToPool(this);
}
