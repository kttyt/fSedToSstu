using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace SstuLib
{
    public class Request
    {
        [JsonProperty(PropertyName = "departmentId")]
        public Guid DepartmentId { get; set; }

        [JsonProperty(PropertyName = "isDirect")]
        public bool IsDirect { get; set; }

        [JsonProperty(PropertyName = "format")]
        [JsonConverter(typeof(StringEnumConverter))]
        public RequestFormat RequestFormat { get; set; }

        [JsonProperty(PropertyName = "number")]
        public string Number { get; set; }

        [JsonProperty(PropertyName = "createDate")]
        public DateTime? CreateDate { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "address")]
        public string Address { get; set; }

        [JsonProperty(PropertyName = "email")]
        public string Email { get; set; }

        [JsonProperty(PropertyName = "questions")]
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
