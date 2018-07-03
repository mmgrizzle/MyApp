using MyApp.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyApp.Mvc.ViewModels
{
    public class AboutViewModel
    {
        public IEnumerable<Supplier> Suppliers { get; private set; }
            = new List<Supplier>();

        public string Address { get; private set; } = string.Empty;

        public AboutViewModel(string address, List<Supplier> suppliers)
        {
            Address = address;
            Suppliers = suppliers;
        }
    }
}
