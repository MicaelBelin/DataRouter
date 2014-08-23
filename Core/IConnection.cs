using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xintric.DataRouter.Core
{


    /// <summary>
    /// 
    /// </summary>
    /// <remarks>Close the connection by disposing the object</remarks>
    public interface IConnection : IDisposable
    {

        /// <summary>
        /// Determines whether the connection is connected or not.
        /// </summary>
        bool IsConnected { get; }
        /// <summary>
        /// Is fired if the connection closes
        /// </summary>
        event Action ConnectionClosed;

        /// <summary>
        /// Sends the specified packet and awaits the response
        /// </summary>
        /// <param name="packet"></param>
        /// <param name="timeout"></param>
        /// <exception cref="TimeoutException">Is thrown if timeout expires</exception>
        /// <exception cref="InvalidOperationException">Is thrown if not connected</exception>
        /// <returns></returns>
        Task<Connection.IResponse> SendAsync(Connection.IRequest packet, TimeSpan timeout);
        /// <summary>
        /// Sends the specified command packet
        /// </summary>
        /// <param name="command"></param>
        /// <exception cref="InvalidOperationException">Is thrown if not connected</exception>
        /// <returns></returns>
        Task SendAsync(Connection.ICommand command);

        void RegisterOnCommand(Func<Connection.ICommand, Connection.Command.FilterResult> cmd);
        void UnregisterOnCommand(Func<Connection.ICommand, Connection.Command.FilterResult> cmd);

        void RegisterOnRequest(Func<Connection.IRequest, Connection.IResponse> requesthandler);
        void UnregisterOnRequest(Func<Connection.IRequest, Connection.IResponse> requesthandler);

        /// <summary>
        /// Start the collector runloop on current thread and blocks until connection is closed.
        /// </summary>
        void RunCollector();





    }
}
