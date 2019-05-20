using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Runtime.InteropServices;
using HD.OPC.Client.Core;
using OpcRcw.Da;
using HD.OPC.Client.Core.Com;

namespace BFM.OPC.Client.Core
{
    public class OpcServer
    {
        /// <summary>
        /// Type tp = Type.GetTypeFromProgID(this.txtProgID.Text);
        /// tp.GUID.ToString()
        /// </summary>
        private string m_clsid;

        /// <summary>
        /// 主机名/IP地址
        /// </summary>
        private string m_hostName;

        /// <summary>
        /// 连接OPC服务器标识。
        /// true：已连接；false：未连接
        /// </summary>
        private bool m_isConnected;

        /// <summary>
        /// OPC服务器对象 IOPCServer
        /// </summary>
        private object m_server;

        private ArrayList m_groups;  //组集合

        #region Properties

        public Int64 PKID { get; set; }

        public string Clsid
        {
            get { return this.m_clsid; }
            set { this.m_clsid = value; }
        }

        /// <summary>
        /// 主机名/IP地址
        /// </summary>
        public string HostName
        {
            get { return m_hostName; }
            set { }
        }

        public bool IsConnected
        {
            get { return this.m_isConnected; }
            set { this.m_isConnected = value; }
        }

        public string ConnectionStatus
        {
            get { return m_isConnected ? "已连接" : "未连接"; }
            set { }
        }

        #endregion

        public OpcServer(string clsid, string hostName)
        {
            this.m_clsid = clsid;
            this.m_hostName = hostName;
            this.m_isConnected = false;

            this.m_groups = new ArrayList();
        }

        #region 连接OPC服务器

        /// <summary>
        /// 连接OPC服务器
        /// </summary>
        public void Connect()
        {
            Guid guid = new Guid(this.m_clsid);
            try
            {
                m_server = (IOPCServer)HD.OPC.Client.Core.Com.Interop.CreateInstance(guid, m_hostName, null);
                m_isConnected = true;
            }
            catch(Exception ex)
            {
                throw new Exception("Could not connect to server. error:" + ex.Message);
            }
        }

        public void Connect(System.Type tp)
        {
            try
            {
                m_server = (IOPCServer)System.Activator.CreateInstance(tp);
                m_isConnected = true;
            }
            catch (Exception ex)
            {
                throw new Exception("Could not connect to server. error:" + ex.Message);
            }
        }

        #endregion

        #region 取消连接OPC服务器

        /// <summary>
        /// 取消连接OPC服务器
        /// </summary>
        public void Disconnect()
        {
            // remove group first
            if (this.m_groups.Count > 0)
            {
                ArrayList grps = new ArrayList(this.m_groups);
                foreach (OpcGroup grp in grps)
                {
                    this.RemoveGroup(grp);
                }
                grps.Clear();
            }

            if (m_server != null)
            {
                HD.OPC.Client.Core.Com.Interop.ReleaseServer(m_server);
                m_server = null;
            }

            m_isConnected = false;
        }

        #endregion

        #region 添加Group

        /// <summary>
        /// 添加Group
        /// </summary>
        /// <param name="groupName"></param>
        /// <param name="updateRate"></param>
        /// <param name="bActive"></param>
        /// <returns></returns>
        public OpcGroup AddGroup(string groupName, int updateRate, bool bActive)
        {
            OpcGroup grp = new OpcGroup(groupName, updateRate, bActive);
            // initialize arguments.
            Guid iid = typeof(IOPCItemMgt).GUID;
            object group = null;

            int serverHandle = 0;
            int revisedUpdateRate = 0;

            GCHandle hDeadband = GCHandle.Alloc(grp.Deadband, GCHandleType.Pinned);

            // invoke COM method.
            try
            {
                ((IOPCServer)m_server).AddGroup(
                    grp.Name ?? "",
                    (grp.Active) ? 1 : 0,
                    grp.UpdateRate,
                    0,
                    IntPtr.Zero,
                    hDeadband.AddrOfPinnedObject(),
                    HD.OPC.Client.Core.Com.Interop.GetLocale(grp.Locale),
                    out serverHandle,
                    out revisedUpdateRate,
                    ref iid,
                    out group);
            }
            catch (Exception e)
            {
                throw HD.OPC.Client.Core.Com.Interop.CreateException("IOPCServer.AddGroup error:" + e.Message, e);
            }
            finally
            {
                if (hDeadband.IsAllocated) hDeadband.Free();
            }

            // set the revised update rate.
            if (revisedUpdateRate > grp.UpdateRate)
                grp.UpdateRate = revisedUpdateRate;

            // save server handle.
            grp.ServerHandle = serverHandle;

            // save group handle
            grp.ComGroup = group;

            // add group to server list
            this.m_groups.Add(grp);

            return grp;
        }

        #endregion

        #region 移除Group

        public void RemoveGroup(OpcGroup group)
        {

            // invoke COM method.
            try
            {
                ((IOPCServer)m_server).RemoveGroup((int)group.ServerHandle, 0);
            }
            catch (Exception e)
            {
                throw HD.OPC.Client.Core.Com.Interop.CreateException("IOPCServer.RemoveGroup", e);
            }


            this.m_groups.Remove(group);
        }

        #endregion

        public OpcGroup FindGroupByName(string groupName)
        {
            foreach (OpcGroup grp in this.m_groups)
            {
                if (grp.Name == groupName)
                {
                    return grp;
                }
            }

            return null;
        }

        /// <summary>
        /// Returns the current server status.
        /// </summary>
        /// <returns>The current server status.</returns>
        public ServerStatus GetStatus()
        {
            ServerStatus output = null;
            lock (this)
            {
                if (m_server == null) throw new Exception("The remote server is not currently connected.");

                // initialize arguments.
                IntPtr pStatus = IntPtr.Zero;

                // invoke COM method.
                try
                {
                    ((IOPCServer)m_server).GetStatus(out pStatus);
                }
                catch (Exception e)
                {
                    throw Interop.CreateException("IOPCServer.GetStatus", e);
                }


                if (pStatus != IntPtr.Zero)
                {
                    OpcRcw.Da.OPCSERVERSTATUS status = (OpcRcw.Da.OPCSERVERSTATUS)Marshal.PtrToStructure(pStatus, typeof(OpcRcw.Da.OPCSERVERSTATUS));

                    output = new ServerStatus();

                    output.VendorInfo = status.szVendorInfo;
                    output.ProductVersion = String.Format("{0}.{1}.{2}", status.wMajorVersion, status.wMinorVersion, status.wBuildNumber);
                    output.ServerState = (serverState)status.dwServerState;
                    output.StatusInfo = null;
                    output.StartTime = Interop.GetFILETIME(HD.OPC.Client.Core.Com.Convert.GetFileTime(status.ftStartTime));
                    output.CurrentTime = Interop.GetFILETIME(HD.OPC.Client.Core.Com.Convert.GetFileTime(status.ftCurrentTime));
                    output.LastUpdateTime = Interop.GetFILETIME(HD.OPC.Client.Core.Com.Convert.GetFileTime(status.ftLastUpdateTime));

                    Marshal.DestroyStructure(pStatus, typeof(OpcRcw.Da.OPCSERVERSTATUS));
                    Marshal.FreeCoTaskMem(pStatus);
                    pStatus = IntPtr.Zero;
                }
            }
            return output;
        }

        public void RefreshOpc()
        {
            foreach (OpcGroup grp in this.m_groups)
            {
                grp.Refresh();
            }
        }

    }

}