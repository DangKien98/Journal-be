using System;
using System.Collections.Generic;

namespace Journal_be.Models
{
    public partial class TestFile
    {
        public int Id { get; set; }
        public byte[]? FileTest { get; set; }
    }
}
