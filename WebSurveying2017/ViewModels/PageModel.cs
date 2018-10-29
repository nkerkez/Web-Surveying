using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebSurveying2017.ViewModels
{
    public class PageModel<T>
    {
        public int CurrentPage { get; set; }
        public int Count { get; set; }
        public int Size { get; set; }

        [JsonProperty]
        public IEnumerable<T> Models { get; set; }
    }
}