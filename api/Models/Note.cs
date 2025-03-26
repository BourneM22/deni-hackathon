using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
    public class Note
    {
        public String NoteId { get; set; } = String.Empty;
        public String FolderId { get; set;} = String.Empty;
        public String Title { get; set;} = String.Empty;
        public String Content { get; set;} = String.Empty;
        public DateTime CreatedDateTime { get; set;}
        public DateTime ModifiedDateTime { get; set;}
    }
}