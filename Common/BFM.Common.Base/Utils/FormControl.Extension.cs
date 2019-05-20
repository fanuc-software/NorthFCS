using System;
using System.Reflection;
using System.Windows.Forms;

namespace BFM.Common.Base.Utils
{
	public static class FormControl
	{
		public static void CreateFormInstance(this Form form, string assName, string fullName)
		{
			Assembly assem = Assembly.Load(assName);
			Type type = assem.GetType(fullName);
			Form result = type.InvokeMember("GetInstance", BindingFlags.Static | BindingFlags.Public | BindingFlags.InvokeMethod, null, null, new object[] { }) as Form;
			result.Activated -= Form_Activated;
			result.Activated += Form_Activated;
			if (result == form.ActiveControl)
			{
				Form_Activated(result, null);
			}
			else
			{
				result.Activate();
			}

			if (result.MdiParent == null)
			{
				result.MdiParent = form;
				result.Show();
			}
		}

		private static void Form_Activated(object sender, EventArgs e)
		{
			//Form childForm = sender as Form;
			//if (childForm == null || childForm.Parent == null) return;
			//RibbonControl controlParent = childForm.MdiParent.Controls.OfType<RibbonControl>().FirstOrDefault();
			//RibbonControl controlChild = childForm.Controls.OfType<RibbonControl>().FirstOrDefault();
			//if (controlParent == null || controlChild == null) return;
			//controlParent.SelectedPage = controlChild.SelectedPage;
		}
	}
}
