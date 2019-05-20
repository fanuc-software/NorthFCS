/*图片按钮
 Add by leihj
*/

using System.Windows.Controls;

namespace BFM.WPF.Base.Controls
{
    public class ImageButtonWithIcon : Button
    {

        /// <summary>
        /// 图标
        /// </summary>
        private string _icon;
        public string Icon
        {
            get { return _icon; }
            set { _icon = value; }
        }


        /// <summary>
        /// 悬停，按下图标
        /// </summary>
        private string _selectedIcon;
        public string SelectedIcon
        {
            get { return _selectedIcon; }
            set { _selectedIcon = value; }
        }
    }
}
