using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blog_management
{
    class Blog
    {
        public Blog(string content, DateTime createtime, string title, string description, string fatherDictionary = null, DateTime updatetime = default)
        {
            Content = content;
            Createtime = createtime;
            Title = title;
            Description = description;
            FatherDictionary = fatherDictionary;
            Updatetime = updatetime;
        }

        public String Content { get; set; }
        public DateTime Createtime { get; set; }
        public DateTime Updatetime { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string FatherDictionary { get; set; }
        
    }
}
