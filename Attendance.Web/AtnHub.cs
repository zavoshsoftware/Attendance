using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Attendance.Web
{
    public class AtnHub : Hub
    {
        public void Authenticate(Guid id,string name, string message,Guid LoginId,Guid opId)
        {
            // Call the addNewMessageToPage method to update clients.
            Clients.All.addNewMessageToPage(name, message,opId);
        }
        
        public void Inquiry(Guid id,string name, string message,Guid LoginId, Guid opId)
        {
            // Call the Inquiry method to update clients.
            Clients.All.Inquiry(name, message,opId);
        }
        
        public void CreateCard(string id, Guid opId)
        { 
            Clients.All.CreateCard(id,opId);
        }

        public void Exit(Guid id,string message, Guid opId)
        {
            Clients.All.Exit(id, message,opId);
        }
        
        public void Alarm(Guid id,string message, Guid opId)
        {
            Clients.All.Alarm(id, message,opId);
        }

        public void CodeGenerator(string code, Guid opId)
        {
            Clients.All.Generate(code,opId);
        }

    }
}