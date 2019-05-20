using System.Drawing;
using System.Windows.Controls;

namespace BFM.WPF.Base.Controls
{
    public class ImageButton : Button
    {
        private string _normalImgPath;

        /// <summary>
        /// 默认图片
        /// </summary>
        public string NormalImgPath
        {
            get { return _normalImgPath; }
            set { _normalImgPath = value; }
        }

        /// <summary>
        /// 按钮按下的图片
        /// </summary>
        private string _pressedImgPath;

        public string PressedImgPath
        {
            get { return _pressedImgPath; }
            set { _pressedImgPath = value; }
        }

        private string _hoverImgPath;

        /// <summary>
        /// 鼠标悬停的图片
        /// </summary>
        public string HoverImgPath
        {
            get { return _hoverImgPath; }
            set { _hoverImgPath = value; }
        }

        private string _disableImgPath;

        /// <summary>
        /// 按钮被禁用时的图片
        /// </summary>
        public string DisableImgPath
        {
            get { return _disableImgPath; }
            set { _disableImgPath = value; }
        }

        private bool _hasText;

        /// <summary>
        /// 是否有文本
        /// </summary>
        public bool HasText
        {
            get { return _hasText; }

            set { _hasText = value; }
        }

        private Color _pressedColor;

        /// <summary>
        /// 按钮按下时文本的颜色
        /// </summary>
        public Color PressedColor
        {
            get { return _pressedColor; }

            set { _pressedColor = value; }
        }

        private Color _hoveColor;

        /// <summary>
        ///鼠标悬停时文本的颜色
        /// </summary>
        public Color HoveColor
        {
            get { return _hoveColor; }

            set { _hoveColor = value; }
        }

        private Color _disableColor;

        /// <summary>
        /// 按钮被禁用时文本的颜色
        /// </summary>
        public Color DisableColor
        {
            get { return _disableColor; }

            set { _disableColor = value; }
        }

    }
}
