using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace SstuLib
{
    public class Request
    {
        public Guid DepartmentId { get; set; }
        public bool IsDirect { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public RequestFormat RequestFormat { get; set; }
        public string Number { get; set; }
        public DateTime? CreateDate { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public List<Question> Questions { get; set; }

        public Request()
        {
            DepartmentId = Guid.Parse("00000000-0000-0000-0000-000000000000");
            Questions = new List<Question>();
        }

        public bool Validate()
        {
            return (IsDirect || (CreateDate.HasValue && CreateDate.Value != default(DateTime)))
                   && !string.IsNullOrEmpty(Number)
                   && !string.IsNullOrEmpty(Name)
                   && Questions.Count > 0
                   && Questions.All(q => q.Validate());
        }
    }
}
