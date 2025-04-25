using UnityEngine;

namespace DragonBall.VFX
{
    public class VFXService
    {
        private VFXPool vFXPool;

        public VFXService(VFXView vFXPrefab) => vFXPool = new VFXPool(vFXPrefab);

        public void PlayVFXAtPosition(VFXType type, Vector2 spawnPosition)
        {
            VFXController vfxToPlay = vFXPool.GetVFX();
            vfxToPlay.Configure(type, spawnPosition);
        }

        public void ReturnVFXToPool(VFXController vfxToReturn) => vFXPool.ReturnItem(vfxToReturn);
    }
}