using System;
using System.Linq.Expressions;
using System.ServiceModel;
using System.Threading;

namespace BFM.Common.Base.Utils
{
    public class WcfClient
    {
        public static TReturn UseService<TChannel, TReturn>(Func<TChannel, TReturn> func)
        {
            var chanFactory = new ChannelFactory<TChannel>("*");
            TChannel channel = chanFactory.CreateChannel();
            ((IClientChannel)channel).Open();
            TReturn result = func(channel);
            try
            {
                ((IClientChannel)channel).Close();
            }
            catch
            {
                ((IClientChannel)channel).Abort();
            }
            return result;
        }
    }

    public class WcfClient<TService> where TService : class
    {
        private static readonly object LockSave = new object();  //锁线程用
        private const int TrySaveCount = 2;  //多次访问服务端数据库，最后一次报错

        public TReturn UseService<TReturn>(Expression<Func<TService, TReturn>> operation)
        {
            lock (LockSave)
            {
                int iSave = 1;  //最后一次访问服务端数据库失败后抛出异常

                var channelFactory = new ChannelFactory<TService>("*");
                TService channel = channelFactory.CreateChannel();
                var client = (IClientChannel)channel;
                client.Open();

                #region 重复访问服务端数据库，不抛出异常

                while (iSave < TrySaveCount)
                {
                    try
                    {
                        TReturn result0 = operation.Compile().Invoke(channel);

                        try
                        {
                            if (client.State != CommunicationState.Faulted)
                            {
                                client.Close();
                            }
                        }
                        catch
                        {
                            client.Abort();
                        }

                        return result0;
                    }
                    catch (Exception)
                    {
                        Console.WriteLine($"!!!!!!!!!!! 第【{iSave}】次访问服务端数据库失败 !!!!!!!!!!!!");
                        iSave++;
                        Thread.Sleep(100); //暂停100ms
                    }
                }

                #endregion

                #region 最后一次访问服务端数据库，失败后抛出异常

                TReturn result = operation.Compile().Invoke(channel);

                try
                {
                    if (client.State != CommunicationState.Faulted)
                    {
                        client.Close();
                    }
                }
                catch
                {
                    client.Abort();
                }

                return result;

                #endregion

                #region Old - Deleted

                //var channelFactory = new ChannelFactory<TService>("*");
                //TService channel = channelFactory.CreateChannel();
                //var client = (IClientChannel)channel;
                //client.Open();
                //TReturn result = operation.Compile().Invoke(channel);
                //try
                //{
                //    if (client.State != CommunicationState.Faulted)
                //    {
                //        client.Close();
                //    }
                //}
                //catch
                //{
                //    client.Abort();
                //}

                //return result;

                #endregion
            }

        }
    }

    /// <summary>
    /// WCF服务包装类，避免使用Using等方式导致服务出错的问题
    /// </summary>
    public static class WcfExtensions
    {
        public static TReturn Using<TClient, TReturn>(this TClient client, Func<TClient, TReturn> func)
            where TClient : ICommunicationObject
        {
            TReturn result = default(TReturn);
            try
            {
                result = func(client);
                client.Close();
            }
            catch (CommunicationException)
            {
                client.Abort();
            }
            catch (TimeoutException)
            {
                client.Abort();
            }
            catch (Exception)
            {
                client.Abort();
                throw ;
            }
            return result;
        }
    }
}
