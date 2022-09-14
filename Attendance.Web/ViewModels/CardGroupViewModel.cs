using Attendance.Models.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

public class CardGroupViewModel
{
    public Card Card { get; set; }

    public IEnumerable<CardGroup> CardGroups { get; set; }

} 

public class CreateGroupCardViewModel
{
    public List<GroupCardViewModel> GroupCards { get; set; }
}

public class GroupCardViewModel
{
    public string GroupItemId { get; set; }
    public string    CardId { get; set; } 
}