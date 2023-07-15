using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Tangyuan.Data
{
    internal class CommentInfo
    {
        internal uint CommentID { get; private set; }
        internal uint UserID { get; private set; }
        internal uint PostID { get; private set; }
        internal DateTime Date { get; private set; }
        internal uint Likes { get; private set; }
        internal bool IsReply { get; private set; }
        internal uint ReplyID { get; private set; }
        internal string Content { get; private set; }

        internal CommentInfo(uint commentID, uint userID, uint postID, DateTime date, uint likes, string content, bool isReply = false, uint replyID = 0)
        {
            CommentID = commentID;
            UserID = userID;
            PostID = postID;
            Date = date;
            Likes = likes;
            Content = content;
            IsReply = isReply;
            ReplyID = replyID;
        }

    }
}
