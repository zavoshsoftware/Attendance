using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Attendance.Web
{
    public class AtnHub : Hub
    {
        public void Authenticate(string name, string message)
        {
            // Call the addNewMessageToPage method to update clients.
            Clients.All.addNewMessageToPage(name, message);
        }

        public void CodeGenerator(string code)
        {
            Clients.All.Generate(code);
        }

    }
}