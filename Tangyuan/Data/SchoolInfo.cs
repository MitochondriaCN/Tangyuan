using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tangyuan.Data
{
    internal class SchoolInfo
    {
        internal uint SchoolID { get; private set; }
        internal string SchoolName { get; private set; }
        internal List<GradeDefinition> GradeDefinitions { get; private set; }
        internal Color ThemeColor { get; private set; }

        internal class GradeDefinition
        {
            internal uint GradeID { get; private set; }
            internal string GradeName { get; private set; }
            internal Color ThemeColor { get; private set; }

            internal GradeDefinition(uint gradeID, string gradeName, Color themeColor)
            {
                GradeID = gradeID;
                GradeName = gradeName;
                ThemeColor = themeColor;
            }

            public override string ToString()
            {
                return GradeName;
            }
        }

        internal SchoolInfo(uint schoolID, string schoolName, List<GradeDefinition> gradeDefinitions, Color themeColor)
        {
            SchoolID = schoolID;
            SchoolName = schoolName;
            GradeDefinitions = gradeDefinitions;
            ThemeColor = themeColor;
        }

        public override string ToString()
        {
            return SchoolName;
        }
    }
}
