using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Attendance.Web
{
    public class AtnHub : Hub
    {
        public void Authenticate(Guid id,string name, string message,Guid LoginId)
        {
            // Call the addNewMessageToPage method to update clients.
            Clients.All.addNewMessageToPage(name, message);
        }

        public void Exit(Guid id,string message)
        {
            Clients.All.Exit(id, message);
        }
        
        public void Alarm(Guid id,string message)
        {
            Clients.All.Alarm(id, message);
        }

        public void CodeGenerator(string code)
        {
            Clients.All.Generate(code);
        }

    }
}