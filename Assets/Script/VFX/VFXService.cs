using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXService
{
    private VFXPool vfxPool;
    public VFXService(VFXView vfxPrefab) => vfxPool = new VFXPool(vfxPrefab);

    public void PlayVFXAtPosition(VFXType type, Vector3 spawnPosition)
    {
        VFXController vfxToPlay = vfxPool.GetVFX();
        vfxToPlay.Configure(type, spawnPosition);
    }

    public void ReturnVFXToPool(VFXController vfxToReturn) => vfxPool.ReturnItem(vfxToReturn);
}
