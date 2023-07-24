using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Tangyuan.Data
{
    public class PostInfo
    {
        internal uint PostID { get; private set; }
        internal uint AuthorID { get; private set; }
        internal uint SchoolID { get; private set; }
        internal DateTime PostDate { get; private set; }
        internal uint Likes { get; private set; }
        internal uint Views { get; private set; }
        internal XDocument Content { get; private set; }

        internal PostInfo(uint postID, uint authorID, uint schoolID, DateTime postDate, uint likes, uint views, XDocument content)
        {
            PostID = postID;
            AuthorID = authorID;
            SchoolID = schoolID;
            PostDate = postDate;
            Likes = likes;
            Views = views;
            Content = content;
        }
    }
}
