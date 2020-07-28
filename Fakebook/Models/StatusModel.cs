using Entities;
using iTextSharp.text;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Fakebook.Models
{
    public class StatusModel
    {
        public User User { get; set; }
        public Status NewStatus { get; set; }
        public List<StatusInfo> AllStatuses { get; set; }
    }
}