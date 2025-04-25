using DragonBall.Utilities;

namespace DragonBall.VFX
{
    public class VFXPool : GenericObjectPool<VFXController>
    {
        private VFXView vFXPrefab;

        public VFXPool(VFXView vFXPrefab) => this.vFXPrefab = vFXPrefab;

        public VFXController GetVFX() => GetItem<VFXController>();

        protected override VFXController CreateItem<T>() => new VFXController(vFXPrefab);
    }
}