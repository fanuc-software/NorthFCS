using DevExpress.Xpf.Scheduler;
using DevExpress.XtraScheduler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using BFM.Common.Base.Utils;
using BFM.ContractModel;
using BFM.Server.DataAsset.SDMService;

namespace BFM.WPF.SDM.Calendar
{
    /// <summary>
    /// AppointMentEdite.xaml 的交互逻辑
    /// </summary>
    public partial class AppointMentEdit : Window
    {
        private WcfClient<ISDMService> _SDMService;
        public AppointMentEdit()
        {
            InitializeComponent();
        }
        public AppointMentEdit(SchedulerControl scheduler,Appointment appointment)
        {
            InitializeComponent();

            Initialize(scheduler, appointment);
        }

        private void Initialize(SchedulerControl scheduler, Appointment appointment)
        {
            this.txt_endtime.DateTime = appointment.End;
            this.txt_starttime.DateTime = appointment.Start;
            _SDMService = new WcfClient<ISDMService>();
            //Dictionary<string, string> LinqWhere = new Dictionary<string, string>();
            //LinqWhere.Add("MARK_DATE", this.txt_starttime.DateTime.ToString("yyyy-MM-dd"));
            //string jsWhere = JsonSerializer.GetJsonString(LinqWhere);
       
         
           
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
          
            if (isvacation.IsChecked==true)//如果为休息日
            {
                Dictionary<string, string> LinqWhere = new Dictionary<string, string>();
                //LinqWhere.Add("MARK_DATE", this.txt_starttime.DateTime.ToString("yyyy-MM-dd"));
                string jsWhere = JsonSerializer.GetJsonString(LinqWhere);//todo:未考虑跨天业务
                List<RsWorkSchedule> m_RsWorkSchedules = _SDMService.UseService(s => s.GetRsWorkSchedules(""));
                foreach (RsWorkSchedule item in m_RsWorkSchedules)
                {
                    if (DateTime.Parse( item.MARK_DATE.ToString()).ToString("yyyy/MM/dd") == this.txt_starttime.DateTime.ToString("yyyy/MM/dd"))
                    {
                        item.WORK_FLAG = "0";
                        _SDMService.UseService(s => s.UpdateRsWorkSchedule(item));
                    }
                }
             
            }
            else
            {
             
                WeekDays a= chk_weekday.WeekDays  ;
                if (a != 0)
                {
                    int count = 0;
                    int.TryParse(this.txt_day.Text, out count);
                    for (int i = 0; i < count; i++)
                    {
                        DateTime day = DateTime.Now.AddDays(i);
                        if (chk_weekday.WeekDays.ToString().Contains(day.DayOfWeek.ToString()))
                        {
                            List<RsWorkSchedule> m_RsWorkSchedules = _SDMService.UseService(s => s.GetRsWorkSchedules($"MARK_DATE = '{this.txt_starttime.DateTime}'"));
                            if (m_RsWorkSchedules.Count != 0)
                            {
                                m_RsWorkSchedules[0].MARK_DATE = day;
                                m_RsWorkSchedules[0].MARK_STARTTIME = DateTime.Parse(day.Year.ToString() + "-" + day.Month.ToString() + "-" + day.Day.ToString() + " " + txt_starttime.DateTime.Hour.ToString() + ":" + txt_starttime.DateTime.Minute.ToString() + ":00");
                                m_RsWorkSchedules[0].MARK_ENDTIME= DateTime.Parse(day.Year.ToString() + "-" + day.Month.ToString() + "-" + day.Day.ToString() + " " + txt_endtime.DateTime.Hour.ToString() + ":" + txt_endtime.DateTime.Minute.ToString() + ":00");
                                m_RsWorkSchedules[0].WORK_FLAG = "1";
                                _SDMService.UseService(s => s.UpdateRsWorkSchedule(m_RsWorkSchedules[0]));
                            }
                            else
                            {
                                RsWorkSchedule m_RsWorkSchedule = new RsWorkSchedule();
                                m_RsWorkSchedule.PKNO = Guid.NewGuid().ToString("N");
                                m_RsWorkSchedule.MARK_DATE = day;
                                m_RsWorkSchedule.MARK_STARTTIME = DateTime.Parse(day.Year.ToString() + "-" + day.Month.ToString() + "-" + day.Day.ToString() + " " + txt_starttime.DateTime.Hour.ToString() + ":" + txt_starttime.DateTime.Minute.ToString() + ":00");
                                m_RsWorkSchedule.MARK_ENDTIME = DateTime.Parse(day.Year.ToString() + "-" + day.Month.ToString() + "-" + day.Day.ToString() + " " + txt_endtime.DateTime.Hour.ToString() + ":" + txt_endtime.DateTime.Minute.ToString() + ":00");
                                m_RsWorkSchedule.WORK_FLAG = "1";
                                _SDMService.UseService(s => s.AddRsWorkSchedule(m_RsWorkSchedule));
                            }
                        }
                      
                    }
                }


            }

        }
    }
}
