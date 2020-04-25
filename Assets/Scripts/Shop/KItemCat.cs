//// ***********************************************************************
//// Assembly         : Unity
//// Author           : Kimch
//// Created          : 
////
//// Last Modified By : Kimch
//// Last Modified On : 
//// ***********************************************************************
//// <copyright file= "KItemCat" company=""></copyright>
//// <summary></summary>
//// ***********************************************************************
//using System.Collections;

//namespace Game
//{
//    /// <summary>
//    /// 猫
//    /// </summary>
//    public class KItemCat : KItem
//    {
//        public int mainColor
//        {
//            get;
//            private set;
//        }

//        public int colors
//        {
//            get;
//            private set;
//        }

//        public string model
//        {
//            get;
//            private set;
//        }

//        public int skillId
//        {
//            get;
//            private set;
//        }

//        /// <summary>
//        /// 初始值影响系数%
//        /// </summary>
//        public int initialRate
//        {
//            get;
//            private set;
//        }

//        /// <summary>
//        /// 固定成长率%   目前等级能力=INT[固定成长率 * 等级 + 初始值 + 初始值 * 系数 * (等级 - 1) + 升星增加能力]
//        /// </summary>
//        public int growthRate
//        {
//            get;
//            private set;
//        }

//        /// <summary>
//        /// 熔炼魂石价值
//        /// </summary>
//        public int soulStone
//        {
//            get;
//            private set;
//        }

//        /// <summary>
//        /// 碎片ID，合成数量
//        /// </summary>
//        public int[] composePiece
//        {
//            get;
//            private set;
//        }

//        /// <summary>
//        /// 升级所需经验
//        /// </summary>
//        public int[] upgradeExps
//        {
//            get;
//            private set;
//        }

//        /// <summary>
//        /// 升星消耗金币
//        /// </summary>
//        public int[] upstarCost
//        {
//            get;
//            private set;
//        }

//        /// <summary>
//        /// 升星消耗猫咪数量
//        /// </summary>
//        public int[] upstarCat
//        {
//            get;
//            private set;
//        }

//        /// <summary>
//        /// 星级等级上限
//        /// </summary>
//        public int[] maxGradeForStar
//        {
//            get;
//            private set;
//        }

//        /// <summary>
//        /// 升技能消耗猫
//        /// </summary>
//        public int[] upskillCat
//        {
//            get;
//            private set;
//        }

//        /// <summary>
//        /// 升技能消耗金币
//        /// </summary>
//        public int[] upskillCost
//        {
//            get;
//            private set;
//        }

//        /// <summary>
//        /// 星级增加的产金能力
//        /// </summary>
//        public int[] addCoin
//        {
//            get;
//            private set;
//        }

//        /// <summary>
//        /// 星级增加的探索能力
//        /// </summary>
//        public int[] addExplore
//        {
//            get;
//            private set;
//        }

//        /// <summary>
//        /// 星级增加的消除能力
//        /// </summary>
//        public int[] addMatch
//        {
//            get;
//            private set;
//        }

//        /// <summary>
//        /// 照片(全身像)
//        /// </summary>
//        public string photo
//        {
//            get;
//            private set;
//        }

//        public int[] feedCost
//        {
//            get;
//            private set;
//        }

//        /// <summary>
//        /// 猫技能欧气
//        /// </summary>
//        public int[] skillCombatValues
//        {
//            get;
//            private set;
//        }

//        #region Method

//        public int GetMaxGrade()
//        {
//            return upgradeExps.Length;
//        }

//        public int GetMaxStar()
//        {
//            return maxGradeForStar.Length;
//        }

//        public int GetMaxGrade(int star)
//        {
//            if (star > 0 && star <= GetMaxStar())
//            {
//                return maxGradeForStar[star - 1];
//            }
//            return -1;
//        }

//        public int GetUpgradeExp(int grade)
//        {
//            if (grade > 0 && grade <= GetMaxGrade())
//            {
//                return upgradeExps[grade - 1];
//            }
//            return -1;
//        }

//        public int GetCoinAbility(int initial, int star, int grade)
//        {
//            int starValue = 0;
//            if (star > 0 && star <= addCoin.Length)
//            {
//                starValue = addCoin[star - 1];
//            }

//            int gradeValue = 0;
//            if (grade > 0)
//            {
//                gradeValue = initial + (int)(((growthRate * 0.01) * grade) + (initial * (initialRate * 0.01) * (grade - 1)));
//            }
//            return starValue + gradeValue;
//        }

//        public int GetExploreAbility(int initial, int star, int grade)
//        {
//            int starValue = 0;
//            if (star > 0 && star <= addExplore.Length)
//            {
//                starValue = addExplore[star - 1];
//            }

//            int gradeValue = 0;
//            if (grade > 0)
//            {
//                gradeValue = initial + (int)(((growthRate * 0.01) * grade) + (initial * (initialRate * 0.01) * (grade - 1)));
//            }
//            return starValue + gradeValue;
//        }

//        public int GetMatchAbility(int initial, int star, int grade)
//        {
//            int starValue = 0;
//            if (star > 0 && star <= addMatch.Length)
//            {
//                starValue = addMatch[star - 1];
//            }

//            int gradeValue = 0;
//            if (grade > 0)
//            {
//                gradeValue = initial + (int)(((growthRate * 0.01) * grade) + (initial * (initialRate * 0.01) * (grade - 1)));
//            }
//            return starValue + gradeValue;
//        }

//        /// <summary>
//        /// 指定等级 的升级技能消耗
//        /// </summary>
//        /// <param name="grade"></param>
//        /// <returns></returns>
//        public int GetUpskillCost(int grade)
//        {
//            if (grade > 0 && grade <= upskillCost.Length)
//            {
//                return upskillCost[grade - 1];
//            }
//            return -1;
//        }

//        public int GetUpstarCost(int star)
//        {
//            if (star > 0 && star <= upstarCost.Length)
//            {
//                return upstarCost[star - 1];
//            }
//            return -1;
//        }

//        public int GetFeedCost(int grade)
//        {
//            if (grade > 0 && grade <= feedCost.Length)
//            {
//                return feedCost[grade - 1];
//            }
//            return -1;
//        }

//        /// <summary>
//        /// 猫技能欧气
//        /// </summary>
//        /// <param name="skillGrade"></param>
//        /// <returns></returns>
//        public int GetSkillCombatValue(int skillGrade)
//        {
//            if (skillGrade > 0 && skillGrade <= skillCombatValues.Length)
//            {
//                return skillCombatValues[skillGrade - 1];
//            }
//            return 0;
//        }

//        public int[] CoinAbilityRange {
//            get;
//            private set;
//        }
//        public int[] ExploreAbilityRange
//        {
//            get;
//            private set;
//        }
//        public int[] MatchAbilityRange
//        {
//            get;
//            private set;
//        }

//        public override void Load(Hashtable table)
//        {
//            base.Load(table);
//            mainColor = table.GetInt("MainColor");
//            colors = table.GetInt("Color");
//            model = table.GetString("Model");
//            skillId = table.GetInt("SkillId");
//            initialRate = table.GetInt("InitialRate");
//            growthRate = table.GetInt("GrowthRate");
//            soulStone = table.GetInt("Stone");
//            composePiece = table.GetArray<int>("Piece");

//            maxGradeForStar = table.GetArray<int>("UpstarMaxLevel");
//            upgradeExps = table.GetArray<int>("UpgradeExp");

//            upstarCat = table.GetArray<int>("UpstarCat");
//            upstarCost = table.GetArray<int>("UpstarCost");
//            upskillCat = table.GetArray<int>("UpSkill");
//            upskillCost = table.GetArray<int>("UpSkillCost");

//            addCoin = table.GetArray<int>("AddCoin");
//            addExplore = table.GetArray<int>("AddExplore");
//            addMatch = table.GetArray<int>("AddMatch");

//            photo = table.GetString("Icon3");
//            feedCost = table.GetArray<int>("FeedCost");

//            CoinAbilityRange = table.GetArray<int>("CoinAbilityRange");
//            ExploreAbilityRange = table.GetArray<int>("ExploreAbilityRange");
//            MatchAbilityRange = table.GetArray<int>("MatchAbilityRange");

//            skillCombatValues = table.GetArray<int>("SkillLevelScore");
//        }

//        #endregion
//    }
//}

