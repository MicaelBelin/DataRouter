using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xintric.DataRouter.Core
{

    public enum CommandFilterResult
    {
        Consume,
        PassOnToNext,
    }

    public interface IConnection
    {



        Task<Connection.IResponse> SendAsync(Connection.IRequest packet);
        /// <summary>
        /// Throws timeoutexception if timeout expires
        /// </summary>
        /// <param name="packet"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        Task<Connection.IResponse> SendAsync(Connection.IRequest packet, TimeSpan timeout);


        Task SendAsync(Connection.ICommand command);
        Task SendAsync(Connection.ICommand command, TimeSpan timeout);

        void RegisterOnCommand(Func<Connection.ICommand, CommandFilterResult> cmd);
        void UnregisterOnCommand(Func<Connection.ICommand, CommandFilterResult> cmd);
        void RegisterOnCommand<TCommand>(Func<TCommand, CommandFilterResult> cmd) where TCommand : Connection.ICommand;
        void UnregisterOnCommand<TCommand>(Func<TCommand, CommandFilterResult> cmd) where TCommand : Connection.ICommand;


        void RunCollector();





    }
}
