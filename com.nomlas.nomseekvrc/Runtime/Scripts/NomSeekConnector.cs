
using UdonSharp;

namespace Nomlas.NomSeekVRC
{
    public abstract class NomSeekConnector : UdonSharpBehaviour
    {
        public abstract void ChangeURL(VRC.SDKBase.VRCUrl vrcUrl, string title);
    }
}