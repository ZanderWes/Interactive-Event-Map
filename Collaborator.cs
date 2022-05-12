using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFEventMap
{
    public class Collaborator : SingletonObject
    {
        public string Name { get; set; }
        public int CarerID { get; set; }
        public string CarerOrganisation { get; set; }
        public SupportLevel Support { get; set; }
        public enum SupportLevel{
            CASUAL_SUPPORT,
            INTERMEDIATE_SUPPORT,
            HIGH_SUPPORT
        }

       


        public Collaborator() : base() { }
        
    }
}
