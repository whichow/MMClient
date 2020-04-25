// ***********************************************************************
// Company          : 
// Author           : Kimch
// Created          : 
//
// Last Modified By : Kimch
// Last Modified On : 
// ***********************************************************************
using UnityEngine;
using System;
using System.Collections;

namespace Game
{
    /// <summary>
    /// KNotification
    /// </summary>
    public class KNotification : MonoBehaviour
    {
        #region FIELD

        private bool isFirst;
        private DateTime _firstDate;

        #endregion

        #region PROPERTY

        #endregion

        #region METHOD

        private void CancelAllLocal()
        {
#if UNITY_IOS
            UnityEngine.iOS.NotificationServices.CancelAllLocalNotifications();
            UnityEngine.iOS.NotificationServices.ClearLocalNotifications();
#endif
        }

        private void PushLocal(System.DateTime fireDate, string alertBody)
        {
#if UNITY_IOS
            var local = new UnityEngine.iOS.LocalNotification();
            local.fireDate = fireDate;
            local.alertBody = alertBody;
			local.soundName = UnityEngine.iOS.LocalNotification.defaultSoundName;
            UnityEngine.iOS.NotificationServices.ScheduleLocalNotification(local);
#endif
        }

        private void RegisterLocal()
        {
#if UNITY_IOS
			UnityEngine.iOS.NotificationServices.RegisterForNotifications(
				UnityEngine.iOS.NotificationType.Alert | 
				UnityEngine.iOS.NotificationType.Badge | 
				UnityEngine.iOS.NotificationType.Sound);
#endif
        }

        private void OnEnterGame()
        {
            var today = System.DateTime.Today;

            var days = (today - _firstDate).TotalDays;
            if (days < 1)
            {
                if (isFirst)
                {
                    isFirst = false;
                    CancelAllLocal();
                }
            }
            else if (days < 7)
            {

            }
            else
            {
                CancelAllLocal();
            }
        }

        private void OnQuitGame()
        {
            var today = System.DateTime.Today;

            var days = (today - _firstDate).TotalDays;
            if (days < 1)
            {
            }
            else if (days < 7)
            {

            }
            else
            {
                var bodys = new string[] { };

                PushLocal(today.AddDays(1).AddHours(20), bodys[UnityEngine.Random.Range(0, bodys.Length)]);
            }
        }

        #endregion

        #region UNITY

        /// <summary>Starts this instance.</summary>
        private IEnumerator Start()
        {
            var firstInstall = PlayerPrefs.GetString("first_install");
            if (string.IsNullOrEmpty(firstInstall))
            {
                _firstDate = DateTime.Today;
                PlayerPrefs.SetString("first_install", _firstDate.ToShortDateString());
                isFirst = true;
            }
            else
            {
                DateTime tmpFD;
                if (DateTime.TryParse(firstInstall, out tmpFD))
                {
                    _firstDate = tmpFD;
                }
                else
                {
                    _firstDate = DateTime.Today;
                    PlayerPrefs.SetString("first_install", _firstDate.ToShortDateString());
                }
            }

            RegisterLocal();

            yield return new WaitForSeconds(1f);
            OnEnterGame();
        }

        private void OnDisable()
        {
            OnQuitGame();
        }

        private void OnApplicationPause(bool pause)
        {
            if (pause)
            {
                OnQuitGame();
            }
            else
            {
                OnEnterGame();
            }
        }

        #endregion
    }
}