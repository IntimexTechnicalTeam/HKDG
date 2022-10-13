using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class KeyValueAddress : KeyValue
    {
        public KeyValueAddress()
        {

        }

        public KeyValueAddress(string id, string text, string address)
        {
            this.Id = id;
            this.Text = text;
            this.Address = address;
        }

        public string Address { get; set; }
    }
}
