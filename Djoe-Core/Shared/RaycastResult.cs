using CitizenFX.Core;
using Newtonsoft.Json;

namespace Shared
{
    public class RayCastResult
    {
        public Vector3 HitPosition;
        public Vector3 SurfaceNormal;
        public bool DidHit;
        public bool DidHitEntity;
        public int Result;
        public bool IsPed;
    }
}
