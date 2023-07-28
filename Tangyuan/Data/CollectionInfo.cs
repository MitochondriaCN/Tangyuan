using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tangyuan.Data
{
    internal class CollectionInfo
    {
        internal uint CollectionID { get; private set; }
        internal uint AuthorID { get; private set; }
        internal string CollectionName { get; private set; }
        internal string Description { get; private set; }

        internal CollectionInfo(uint collectionID, uint authorID, string collectionName, string description)
        {
            CollectionID = collectionID;
            AuthorID = authorID;
            CollectionName = collectionName;
            Description = description;
        }
    }
}
