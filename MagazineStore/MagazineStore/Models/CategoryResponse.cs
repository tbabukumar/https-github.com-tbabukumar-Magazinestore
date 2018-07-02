using System;
using System.Collections.Generic;
using System.Text;

namespace MagazineStore.Models
{
    public class CategoryResponse : APIResponse
    {
        //bool success { get; set; }
        //string token { get; set; }
        public List<string> data { get; set; }

    }
}
