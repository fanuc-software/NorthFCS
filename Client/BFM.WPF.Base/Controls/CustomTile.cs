using DevExpress.Xpf.LayoutControl;

namespace BFM.WPF.Base.Controls
{
    public class CustomTile : Tile
    {      
        private string m_icopath;

        public string NormalIcoPath
        {
            get { return m_icopath; }
            set { m_icopath = value; }
        }

        private string p_icopath;
        public string PressedIcoPath
        {
            get { return p_icopath; }
            set { p_icopath = value; }
        }

    }
}
