using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.DTO
{
    public class NoteResponse
    {
        public String NoteId { get; set; } = String.Empty;
        public String Title { get; set;} = String.Empty;
        public String Content { get; set;} = String.Empty;
        public DateTime ModifiedDateTime { get; set;}
    }
}