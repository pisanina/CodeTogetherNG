using System;

namespace CodeTogetherNG.Models
{
    public class RequestsListViewModel
    {
        public bool? Accept                   { get; set; }
        public string Message                 { get; set; }
        public string UserName                { get; set; }
        public string MessageDate             { get; set; }

        public string Title                   { get; set; }
        public int ProjectId                  { get; set; }
        public string MemberId                { get; set; }
    }
}