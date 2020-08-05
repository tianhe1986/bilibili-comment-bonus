using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bili_bonus.data
{
    class ResData
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<ResReply> replies { get; set; }
    }

    class ResReply
    {
        public ResReplyMember member { get; set; }
    }

    class ResReplyMember
    {
        public string mid { get; set; }
        public string uname { get; set; }
    }

    class ResResult
    {
        public int code { get; set; }
        public ResData data { get; set; }
    }

    class DynamicCardDesc
    {
        public int type { get; set; }
        public string rid_str { get; set; }
    }

    class DynamicCard
    {
        public DynamicCardDesc desc { get; set; }
    }

    class DynamicResData
    {
        public DynamicCard card { get; set; }
    }

    class DynamicResResult
    {
        public int code { get; set; }
        public DynamicResData data { get; set; }
    }
}
