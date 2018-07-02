using System;
using System.Collections.Generic;
using System.Text;

namespace MagazineStore.Models
{
   public class Subsciber
    {

        public string id { get; set; }
        public string firstName { get; set; }

        public string lastName { get; set; }

        public List<Int32> magazineIds { get; set; }

    }
}
