using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Journal_be.Models
{
    public partial class TestFile
    {
        [JsonIgnore]
        public int Id { get; set; }
        public byte[]? FileTest { get; set; }
    }
}
