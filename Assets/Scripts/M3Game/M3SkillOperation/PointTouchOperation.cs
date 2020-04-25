
namespace Game.Match3
{
    public class PointTouchOperation : OperationBase
    {
        public PointTouchOperation(M3SkillBase s, KCat c) : base(s, c)
        {
        }

        public override void OnSkillStart()
        {
            base.OnSkillStart();

            //var hightList = skill.itemList;
            //for (int i = 0; i < hightList.Count; i++)
            //{
            //    Game.TransformUtils.SetLayer(hightList[i].itemView.itemGameobject, LayerMask.NameToLayer("HightLight"));
            //}
            //Game.TransformUtils.SetLayer(M3GameManager.Instance.catManager.catObj, LayerMask.NameToLayer("HightLight"));

            //KUIWindow.GetWindow<M3GameUIWindow>().SetSkillHightLightMask(true, null);

            //M3GameManager.Instance.catManager.CatBehaviour.SkillPre(false, delegate (KCatBehaviour.State currentState)
            //{
            //    //M3GameManager.Instance.catManager.catBehaviour.Skilling(true, null);
            //    skill.OnUseSkill(null);
            //});

            //FrameScheduler.instance.Add(30, delegate ()
            //{
            //    if (mode == 1)
            //    {
            //        //skill.SetItemList();
            //        M3Item item = skill.GetRandomItem();
            //        //OnSelectItem(item.itemInfo.posX, item.itemInfo.posY);
            //        skill.OnUseSkill(new M3UseSkillArgs(cat, item.itemInfo.posX, item.itemInfo.posY));
            //    }
            //});
        }

        public override void OnSkillUsed()
        {
            base.OnSkillUsed();
            //var hightList = skill.itemList;
            //for (int i = 0; i < hightList.Count; i++)
            //{
            //    Game.TransformUtils.SetLayer(hightList[i].itemView.itemGameobject, LayerMask.NameToLayer("Default"));
            //}
            //hightList.Clear();
            //Game.TransformUtils.SetLayer(M3GameManager.Instance.catManager.catObj, LayerMask.NameToLayer("Default"));
            //KUIWindow.GetWindow<M3GameUIWindow>().SetSkillHightLightMask(false, null);



            //M3GameManager.Instance.catManager.CatBehaviour.SkillPost(false, delegate (KCatBehaviour.State currentState)
            //{
            //    M3GameManager.Instance.catManager.CatBehaviour.Idle();
            //});
        }
        public override void OnSkillCancel()
        {
            base.OnSkillCancel();
            //var hightList = skill.itemList;
            //for (int i = 0; i < hightList.Count; i++)
            //{
            //    Game.TransformUtils.SetLayer(hightList[i].itemView.itemGameobject, LayerMask.NameToLayer("Default"));
            //}
            //hightList.Clear();
            //Game.TransformUtils.SetLayer(M3GameManager.Instance.catManager.catObj, LayerMask.NameToLayer("Default"));
            //KUIWindow.GetWindow<M3GameUIWindow>().SetSkillHightLightMask(false, null);
            //M3GameManager.Instance.catManager.CatBehaviour.Idle();
        }

    }
}