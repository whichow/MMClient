using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace Game.DataModel
{
    public partial class ActivityXTable
    {
        public override void LoadFromHashtable(Hashtable table)
        {
            base.LoadFromHashtable(table);
            XTable.ActivityAddDayRewardXTable.LoadFromHashtable(table);
            XTable.ActivityDailyRewardXTable.LoadFromHashtable(table);
            XTable.ActivityGradeRewardXTable.LoadFromHashtable(table);
        }
    }
}
