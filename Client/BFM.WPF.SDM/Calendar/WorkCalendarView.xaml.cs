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
using System.Windows.Shapes;
using BFM.Common.Base.Utils;
using BFM.ContractModel;
using BFM.Server.DataAsset.SDMService;

namespace BFM.WPF.SDM.Calendar
{
    /// <summary>
    /// WorkCalendarView.xaml 的交互逻辑
    /// </summary>
    public partial class WorkCalendarView : Page
    {
        private WcfClient<ISDMService> _SDMService;
        List<RsWorkSchedule> m_RsWorkSchedules;

        public WorkCalendarView()
        {
            InitializeComponent();
       
            _SDMService = new WcfClient<ISDMService>();
            m_RsWorkSchedules= _SDMService.UseService(s => s.GetRsWorkSchedules("")).Where(c => c.WORK_FLAG != "0").ToList();
         
            this.schedulerControl1.Storage.AppointmentStorage.DataSource = m_RsWorkSchedules;

        }


        private void schedulerStorage_AppointmentsInserted_1(object sender, PersistentObjectsEventArgs e)
        {
           
          
         
        }

        private void schedulerControl1_EditAppointmentFormShowing(object sender, EditAppointmentFormEventArgs e)
        {

            e.Cancel = true;
            AppointMentEdit ame = new Calendar.AppointMentEdit(this.schedulerControl1, e.Appointment);
            ame.Closed += Ame_Closed;
            ame.Show();
        }

        private void Ame_Closed(object sender, EventArgs e)
        {
            m_RsWorkSchedules = _SDMService.UseService(s => s.GetRsWorkSchedules("")).Where(c => c.WORK_FLAG != "0").ToList();
           
            this.schedulerControl1.Storage.AppointmentStorage.DataSource = m_RsWorkSchedules;
        }

        private void EditSchedul(object sender, RoutedEventArgs e)
        {
            AppointMentEdit ame = new Calendar.AppointMentEdit(this.schedulerControl1, schedulerControl1.SelectedAppointments[0]);
            ame.Closed += Ame_Closed;
            ame.Show();
        }
    }
}
