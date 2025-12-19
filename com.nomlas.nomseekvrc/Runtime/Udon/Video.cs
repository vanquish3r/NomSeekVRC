
/*

UdonSharpはinternalの処理をミスることがあるので、本来外部からアクセスするものではないが、全てpublicにしてある
詳細: https://feedback.vrchat.com/udon/p/udonsharp-cannot-access-internal-properties

*/

using TMPro;
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;

namespace Nomlas.NomSeekVRC
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class Video : UdonSharpBehaviour
    {
        [SerializeField] private NomSeek nomSeek;
        [SerializeField] private RawImage thumbnail;
        [SerializeField] private TextMeshProUGUI title;
        [SerializeField] private TextMeshProUGUI viewCountAndUploaded;
        [SerializeField] private RawImage channelIcon;
        [SerializeField] private TextMeshProUGUI channelName;
        private VRCUrl vrcUrl;

        public void OnClick()
        {
            nomSeek.PlayURL(vrcUrl, title.text);
        }

        public void SetMetaDatas(NomSeek nomSeek, VRCUrl playVrcUrl, string titleText, string viewCountText, string uploadedText, string channelNameText)
        {
            this.nomSeek = nomSeek;
            vrcUrl = playVrcUrl;
            title.text = string.IsNullOrEmpty(titleText) ? "[情報が取得できませんでした]" : titleText;
            if (!string.IsNullOrEmpty(viewCountText) && !string.IsNullOrEmpty(uploadedText))
            {
                viewCountAndUploaded.text = $"{viewCountText}・{uploadedText}";
            }
            else if (!string.IsNullOrEmpty(viewCountText))
            {
                viewCountAndUploaded.text = viewCountText;
            }
            else if (!string.IsNullOrEmpty(uploadedText))
            {
                viewCountAndUploaded.text = uploadedText;
            }
            else
            {
                viewCountAndUploaded.text = "-";
            }
            channelName.text = string.IsNullOrEmpty(channelNameText) ? "?" : channelNameText;
        }

        public void SetTexture(Texture2D imagesheet_texture, Rect thumbnailRect, Rect channelIconRect)
        {
            thumbnail.texture = imagesheet_texture;
            channelIcon.texture = imagesheet_texture;
            thumbnail.uvRect = thumbnailRect;
            channelIcon.uvRect = channelIconRect;
        }

        public void SetTexture_Thumbnail(Texture2D imagesheet_texture, Rect rect)
        {
            thumbnail.texture = imagesheet_texture;
            channelIcon.texture = imagesheet_texture;
            thumbnail.uvRect = rect;
        }

        public void SetTexture_Icon(Texture2D imagesheet_texture, Rect rect)
        {
            thumbnail.texture = imagesheet_texture;
            channelIcon.texture = imagesheet_texture;
            channelIcon.uvRect = rect;
        }

        private void OnDestroy()
        {
            // テクスチャを捨てる
            if (thumbnail.texture != null)
            {
                Destroy(thumbnail.texture);
                thumbnail.texture = null;
            }
            if (channelIcon.texture != null)
            {
                Destroy(channelIcon.texture);
                channelIcon.texture = null;
            }
        }
    }
}