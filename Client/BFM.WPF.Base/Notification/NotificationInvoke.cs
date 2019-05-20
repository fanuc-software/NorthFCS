using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using Hardcodet.Wpf.TaskbarNotification;
using Samples;

namespace BFM.WPF.Base.Notification
{
    public class NotificationInvoke
    {
        private static readonly TaskbarIcon tb = new TaskbarIcon();
        private const int TimeShow = 5000; //显示时间

        public static void NewNotification(string title, string content)
        {
            FancyBalloon balloon = new FancyBalloon();
            balloon.BalloonText = content;
            tb.ShowCustomBalloon(balloon, PopupAnimation.None, TimeShow);
        }

        public static List<NotificationView> _dialogs = new List<NotificationView>();

        //public static void NewNotification(string title, string content)
        //{
        //    i++;
        //    NotifyData data = new NotifyData();
        //    data.Title = title;
        //    data.Content = content;

        //    NotificationView dialog = new NotificationView(); //new 一个通知
        //    dialog.Closed += Dialog_Closed;
        //    dialog.TopFrom = GetTopFrom();
        //    _dialogs.Add(dialog);
        //    dialog.DataContext = data; //设置通知里要显示的数据
        //    dialog.Show();
        //}

        public static void NewFancyToolTip(string content)
        {
            FancyToolTip fancyToolTip = new FancyToolTip();
            fancyToolTip.InfoText = content;
            tb.ShowCustomBalloon(fancyToolTip, PopupAnimation.Fade, TimeShow);
        }

        public static void NewWelcomeBalloon(string content)
        {
            WelcomeBalloon welcomeBalloon = new WelcomeBalloon();
            tb.ShowCustomBalloon(welcomeBalloon, PopupAnimation.Fade, TimeShow);
        }

        private static double GetTopFrom()
        {
            //屏幕的高度-底部TaskBar的高度。
            double topFrom = System.Windows.SystemParameters.WorkArea.Bottom - 10;
            bool isContinueFind = _dialogs.Any(o => o.TopFrom == topFrom);

            while (isContinueFind)
            {
                topFrom = topFrom - 110; //此处100是NotifyWindow的高 110-100剩下的10  是通知之间的间距
                isContinueFind = _dialogs.Any(o => o.TopFrom == topFrom);
            }

            if (topFrom <= 0)
                topFrom = System.Windows.SystemParameters.WorkArea.Bottom - 10;

            return topFrom;
        }
    }
}
