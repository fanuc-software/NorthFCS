using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Media;
using BFM.WPF.FlowDesign.Controls;
using BFM.WPF.FlowDesign.MainCanvas;

namespace BFM.WPF.FlowDesign
{
    public class FlowDesignerViewModel : INotifyPropertyChanged
    {
        public FlowContent cvMain;

        public IEnumerable<double?> FontSizes { get; protected set; }
        public string[] FontFamilies { get; protected set; }

        #region 

        private string _FontFamily;
        public string FontFamily {
            get { return _FontFamily; } 
            set
            {
                _FontFamily = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("FontFamily"));
            } 
        }

        private double _FontSize;

        public double FontSize
        {
            get { return _FontSize; }
            set
            {
                _FontSize = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("FontSize"));
            }
        }

        public Brush SelectionTextBackgroundColor
        {
            get
            {
                if (cvMain.CurSelectedDragThumb != null) return cvMain.CurSelectedDragThumb.Background;
                return Brushes.Transparent;
            }
            set
            {
                foreach (DragThumb thumb in cvMain.SelectDragThumbs)
                {
                    thumb.Background = value;
                }
            }
            
        }


        private bool _IsBold;
        public bool IsBold
        {
            get { return _IsBold; }
            set
            {
                _IsBold = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsBold"));
            }
        }

        private bool _IsItalic;
        public bool IsItalic
        {
            get { return _IsItalic; }
            set
            {
                _IsItalic = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsItalic"));
            }
        }

        private bool _IsUnderline;
        public bool IsUnderline
        {
            get { return _IsUnderline; }
            set
            {
                _IsUnderline = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsUnderline"));
            }
        }

        public Brush _BackgoundColor;
        public Brush BackgoundColor
        {
            get { return _BackgoundColor; }
            set
            {
                _BackgoundColor = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("BackgoundColor"));
            }
        }


        public Brush _BorderColor;
        public Brush BorderColor
        {
            get { return _BorderColor; }
            set
            {
                _BorderColor = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("BorderColor"));
            }
        }

        public Brush _SelectionTextColor;
        public Brush SelectionTextColor
        {
            get
            {
                return _SelectionTextColor;
            }
            set
            {
                _SelectionTextColor = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SelectionTextColor"));
            }

        }

        public string _SelectName;
        public string SelectName
        {
            get
            {
                return _SelectName;
            }
            set
            {
                _SelectName = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SelectName"));
            }

        }

        public double _SelectX;
        public double SelectX
        {
            get
            {
                return _SelectX;
            }
            set
            {
                _SelectX = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SelectX"));
            }

        }

        public double _SelectY;
        public double SelectY
        {
            get
            {
                return _SelectY;
            }
            set
            {
                _SelectY = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SelectY"));
            }

        }

        public double _SelectWidth;
        public double SelectWidth
        {
            get
            {
                return _SelectWidth;
            }
            set
            {
                _SelectWidth = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SelectWidth"));
            }

        }

        public double _SelectHeight;
        public double SelectHeight
        {
            get
            {
                return _SelectHeight;
            }
            set
            {
                _SelectHeight = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SelectHeight"));
            }

        }

        public bool _SelectReadOnlyCanClick;
        public bool SelectReadOnlyCanClick
        {
            get
            {
                return _SelectReadOnlyCanClick;
            }
            set
            {
                _SelectReadOnlyCanClick = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SelectReadOnlyCanClick"));
            }

        }

        public bool _SelectMonitorItem;
        public bool SelectMonitorItem
        {
            get
            {
                return _SelectMonitorItem;
            }
            set
            {
                _SelectMonitorItem = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SelectMonitorItem"));
            }

        }

        #endregion

        public FlowDesignerViewModel()
        {
            Initialize();
        }

        private void Initialize()
        {
            FontSizes = new Nullable<double>[]
            {
                3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 22, 24, 26, 28, 30,
                32, 34, 36, 38, 40, 44, 48, 52, 56, 60, 64, 68, 72, 76, 80, 88, 96, 104, 112, 120, 128, 136, 144
            };
            FontFamilies = GetFontFamilies();

        }

        private string[] GetFontFamilies()
        {
            return Fonts.SystemFontFamilies.Select(f => f.ToString()).OrderBy(f => f).ToArray();
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
