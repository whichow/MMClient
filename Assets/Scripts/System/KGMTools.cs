// ***********************************************************************
// Assembly         : Unity
// Author           : Kimch
// Created          : 
//
// Last Modified By : Kimch
// Last Modified On : 
// ***********************************************************************
// <copyright file= "KGMTools" company=""></copyright>
// <summary></summary>
// ***********************************************************************
using Game.DataModel;
using System.Collections.Generic;
using UnityEngine;

public class KGMTools : MonoBehaviour
{
    /// <summary>
    /// GM指令
    /// </summary>
    public static class GMCommand
    {
        public static readonly string Name = "GM";
        public static readonly string Description = "GM Command";
        public static readonly string Specification = "add id count,add_coin 1,add_diamond 1,add_catfood 1,add_cat id count";

        public static string Execute(string[] args)
        {
            if (args == null || args.Length == 0)
            {
                return "无效参数";
            }
            var cmd = args[0];
#if UNITY_EDITOR
            if ("savemap".Equals(cmd, System.StringComparison.OrdinalIgnoreCase) && Game.Build.Map.Instance)
            {
                Game.Build.Map.Instance.SaveMap();
                return "保存地图至桌面";
            }
#endif
            var cmdArgs = new List<string>();
            if ("add_all".Equals(cmd, System.StringComparison.OrdinalIgnoreCase))
            {

                if (Game.KServer.Instance)
                {
                    cmdArgs.Clear();
                    cmd = "add_diamond";
                    cmdArgs.Add("1000000");
                    Game.KServer.Instance.GMCommand(cmd, cmdArgs.ToArray());
                    cmdArgs.Clear();
                    cmd = "add_coin";
                    cmdArgs.Add("1000000");
                    Game.KServer.Instance.GMCommand(cmd, cmdArgs.ToArray());
                    cmdArgs.Clear();
                    cmd = "add_exp";
                    cmdArgs.Add("1000000");
                    Game.KServer.Instance.GMCommand(cmd, cmdArgs.ToArray());
                    cmd = "add_star";
                    cmdArgs.Add("1000000");
                    Game.KServer.Instance.GMCommand(cmd, cmdArgs.ToArray());

                    cmdArgs.Clear();
                    cmd = "add_all_item";
                    Game.KServer.Instance.GMCommand(cmd, cmdArgs.ToArray());

                    cmdArgs.Clear();
                    cmd = "all_depot_building";
                    Game.KServer.Instance.GMCommand(cmd, cmdArgs.ToArray());

                    foreach (var level in XTable.LevelXTable.AllLevels)
                    {
                        cmdArgs.Clear();
                        cmd = "finish_stage";
                        cmdArgs.Add("1");
                        cmdArgs.Add(level.ID.ToString());
                        Game.KServer.Instance.GMCommand(cmd, cmdArgs.ToArray());
                    }
                }
                return "add_all";
            }

            for (int i = 1; i < args.Length; i++)
            {
                cmdArgs.Add(args[i]);
            }

            if (Game.KServer.Instance)
            {
                Game.KServer.Instance.GMCommand(cmd, cmdArgs.ToArray());
            }

            return string.Empty;
        }
    }

    #region Unity  

    // Use this for initialization
    private void Start()
    {
        var prefab = Resources.Load("Console");
        if (prefab)
        {
            GameObject.Instantiate(prefab);
        }
        K.Console.ConsoleCommand.RegisterCommand(GMCommand.Name, GMCommand.Description, GMCommand.Specification, GMCommand.Execute);
    }

    // Update is called once per frame
    private void Update()
    {

    }

    #endregion
}

