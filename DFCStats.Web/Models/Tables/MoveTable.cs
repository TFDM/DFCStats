using System.ComponentModel;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DFCStats.Web.Models.Tables
{
    public class MoveTable
    {
        public Guid Id { get; set; }
        public string Direction { get; set; } = string.Empty;
    }
}