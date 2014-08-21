using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xintric.DataRouter.Core
{
    public static class ConnectionExtensions
    {
        public static Task<Connection.IResponse> SendAsync(this IConnection connection, Connection.IRequest packet)
        {
            return connection.SendAsync(packet, System.Threading.Timeout.InfiniteTimeSpan);
        }


        static Dictionary<object, Func<Connection.ICommand, CommandFilterResult>> commandtranslator = new Dictionary<object, Func<Connection.ICommand, CommandFilterResult>>();
        public static void RegisterOnCommand<TCommand>(this IConnection connection, Func<TCommand, CommandFilterResult> cmd) where TCommand : class, Connection.ICommand
        {
            lock (commandtranslator)
            {
                var obj = new Func<Connection.ICommand, CommandFilterResult>(c =>
                {
                    if (!(c is TCommand)) return CommandFilterResult.PassOnToNext;
                    return cmd(c as TCommand);
                });
                commandtranslator.Add(cmd, obj);
                connection.RegisterOnCommand(obj);
            }
        }
        public static void UnregisterOnCommand<TCommand>(this IConnection connection, Func<TCommand, CommandFilterResult> cmd) where TCommand : class, Connection.ICommand
        {
            lock (commandtranslator)
            {
                var theobj = commandtranslator[cmd];
                connection.UnregisterOnCommand(theobj);
                commandtranslator.Remove(cmd);
            }
        }

        static Dictionary<object, Func<Connection.IRequest, Connection.IResponse>> requesttranslator = new Dictionary<object, Func<Connection.IRequest, Connection.IResponse>>();
        public static void RegisterOnRequest<TRequest>(this IConnection connection, Func<TRequest, Connection.IResponse> cmd) where TRequest : class, Connection.IRequest
        {
            lock (requesttranslator)
            {
                var obj = new Func<Connection.IRequest, Connection.IResponse>(c =>
                {
                    if (!(c is TRequest)) return null;
                    return cmd(c as TRequest);
                });
                requesttranslator.Add(cmd, obj);
                connection.RegisterOnRequest(obj);
            }
        }
        public static void UnregisterOnRequest<TRequest>(this IConnection connection, Func<TRequest, Connection.IResponse> cmd) where TRequest : class, Connection.IRequest
        {
            lock (requesttranslator)
            {
                var theobj = requesttranslator[cmd];
                connection.UnregisterOnRequest(theobj);
                requesttranslator.Remove(cmd);
            }
        }


    }
}
