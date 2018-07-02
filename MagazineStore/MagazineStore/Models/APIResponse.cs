using System;
using System.Collections.Generic;
using System.Text;

namespace MagazineStore.Models
{
    public class APIResponse
    {
        bool success { get; set; }
        string token { get; set; }

        string message { get; set; }

    }
}
