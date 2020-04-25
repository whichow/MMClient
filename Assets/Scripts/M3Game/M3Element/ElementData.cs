using Game.DataModel;
using System.Collections.Generic;

namespace Game.Match3
{
    /// <summary>
    /// 元素特殊效果
    /// </summary>
    public enum ElementSpecial
    {
        None = 0,
        //Transform = 1,
        /// <summary>
        /// 消除一行（摇头）
        /// </summary>
        Row = 2,
        /// <summary>
        /// 消除一列（点头）
        /// </summary>
        Column = 3,
        /// <summary>
        /// 消除区域（炸弹13消）
        /// </summary>
        Area = 4,
        /// <summary>
        /// 消除颜色（猫头）
        /// </summary>
        Color = 5,
        //RoundPause = 6,
    }

    /// <summary>
    /// 元素数据
    /// </summary>
    public class ElementData
    {
        public ElementXDM config;

        public bool canCollect { get; private set; }
        private ElementSpecial special;
        private ItemColor color;

        public void Init(int id)
        {
            config = XTable.ElementXTable.GetByID(id);

            for (int i = 0; i < config.ClearEvent.Count; i++)
            {
                if (config.ClearEvent[i] == 7)
                    canCollect = true;
                else if (config.ClearEvent[i] > 1 && config.ClearEvent[i] < 6)
                    special = (ElementSpecial)config.ClearEvent[i];
            }
            color = (ItemColor)config.ColorType;
        }

        public List<int> GetTargetId()
        {
            return config.Collect;
        }

        public ElementSpecial GetSpecial()
        {
            return special;
        }

        public void SetSpecial(ElementSpecial s)
        {
            special = s;
        }

        public ItemColor GetColor()
        {
            return color;
        }

        public void SetColor(ItemColor c)
        {
            color = c;
        }

        /// <summary>
        /// 能否参加消除
        /// </summary>
        /// <returns></returns>
        public bool CanNormalEliminate()
        {
            for (int i = 0; i < config.ClearType.Count; i++)
            {
                if (config.ClearType[i] == 1)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// 能否被技能消除
        /// </summary>
        /// <returns></returns>
        public bool CanSkillEliminate()
        {
            for (int i = 0; i < config.ClearType.Count; i++)
            {
                if (config.ClearType[i] == 2)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// 能否被道具消除
        /// </summary>
        /// <returns></returns>
        public bool CanPropEliminate()
        {
            for (int i = 0; i < config.ClearType.Count; i++)
            {
                if (config.ClearType[i] == 3)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// 能否内含元素消除
        /// </summary>
        /// <returns></returns>
        public bool CanContainElementEliminate()
        {
            for (int i = 0; i < config.ClearType.Count; i++)
            {
                if (config.ClearType[i] == 4)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// 能否被刮边消除
        /// </summary>
        /// <returns></returns>
        public bool CanScrapingEliminate()
        {
            for (int i = 0; i < config.ClearType.Count; i++)
            {
                if (config.ClearType[i] == 5)
                    return true;
            }
            return false;
        }

        public bool IsSpecialElement()
        {
            return special > 0 && (config.Type == M3ElementType.SpecialElement || config.Type == M3ElementType.MagicCatElement);
        }

        /// <summary>
        /// 是否是基础三消元素 如普通红元素，可变元素
        /// </summary>
        /// <returns></returns>
        public bool IsBaseElement()
        {
            return color > 0 && (config.Type == M3ElementType.NormalElement || config.Type == M3ElementType.CrystalElement);
        }

        public bool IsBaseAndeSpecialElement()
        {
            return (color > 0 && (config.Type == M3ElementType.CrystalElement || config.Type == M3ElementType.NormalElement || config.Type == M3ElementType.SpecialElement)) || config.Type == M3ElementType.MagicCatElement;
        }

        public string GetAnimationsByKey(string key)
        {
            return (string)config.Animations[key];
        }

    }
}