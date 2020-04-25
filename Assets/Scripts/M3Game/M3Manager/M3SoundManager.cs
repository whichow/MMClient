/** 
*FileName:     M3SoundManager.cs 
*Author:       HeJunJie 
*Version:      1.0 
*UnityVersion：5.6.2f1
*Date:         2018-01-19 
*Description:    
*History: 
*/
using UnityEngine;

namespace Game.Match3
{
    public class M3SoundClip
    {
        public const string eliminate = "sounds_{0}_eliminate{1}";
        public const string dropLand = "sounds_1_drop";
        public const string select = "sounds_10_selected";
        public const string exchange = "sounds_11_change";
        public const string autotips = "sounds_12_autotips";
        public const string createline = "sounds_19_crateline";
        public const string createwrap = "sounds_21_creatwrap";
        public const string createcolor = "sounds_23_creatcolor";
        public const string eleminateline = "sounds_19_crateline";
        public const string eleminatewrap = "sounds_21_creatwrap";
        public const string eleminatecolor = "sounds_23_creatcolor";
        public const string eleminatecrit = "sounds_25_eliminatecrit";

        public const string linetoline = "sounds_26_swapdoubleline";
        public const string linetowrap = "sounds_27_swaplinewarp";
        public const string wraptowrap = "sounds_28_swapdoublewarp";
        public const string linetocolor = "sounds_29_swapcolorline";
        public const string colortowrap = "sounds_30_swapcolorwarp";
        public const string colortocolor = "sounds_31_swapdoublecolor";

        public const string eleminateice = "sounds_33_eliminateice";
        public const string eleminatebook = "sounds_34_eliminatebook";
        public const string eleminatecurtain = "sounds_35_eliminatecurtain";
        public const string eleminatechain = "sounds_39_eliminatechain";
        public const string eleminatevenom = "sounds_40_eliminatevenom";
        public const string eleminatecoin = "sounds_41_eliminatecoin";
        public const string eleminatecoom = "sounds_42_eliminatezombie";
        public const string eleminatecoomshield = "sounds_42_eliminatezombie";
        public const string eleminategift = "sounds_45_eliminategift";
        public const string eleminateenergy = "sounds_46_eliminateenergybottle";
        public const string eleminatecattery = "sounds_44_eliminatecattery";

        public const string sendenergy = "sounds_50_crateenergy";
        public const string collectenergy = "sounds_51_eliminateenergy";

        public const string preGameMusic = "music_pregamescene";
        public const string mainMusic = "music_gamescene";
        public const string bonus = "music_bonustime";
        public const string win = "music_gamewin";
        public const string lose = "music_gamelose";
    }

    public class M3SoundManager
    {

        private string audioPath = "Audio/";
        private string musicPath = "Music/";


        public void PlayBonus()
        {
            AudioClip clip = LoadAudioClip(M3SoundClip.bonus, false);
            if (clip != null)
                KSoundManager.PlayMusic(clip, false);
        }
        public void PlayWin()
        {
            AudioClip clip = LoadAudioClip(M3SoundClip.win, false);
            if (clip != null)
                KSoundManager.PlayMusic(clip, false);
        }
        public void PlayLose()
        {
            AudioClip clip = LoadAudioClip(M3SoundClip.lose, false);
            if (clip != null)
                KSoundManager.PlayMusic(clip, false);
        }

        public void PlayMainMusic()
        {
            AudioClip clip = LoadAudioClip(M3SoundClip.mainMusic, false);
            if (clip != null)
                KSoundManager.PlayMusic(clip, false);
        }

        public void PlayPreGameMusic()
        {
            AudioClip clip = LoadAudioClip(M3SoundClip.preGameMusic, false);
            if (clip != null)
                KSoundManager.PlayInterlude(clip, false);
        }

        public void PlaySendEnergy()
        {
            AudioClip clip = LoadAudioClip(M3SoundClip.sendenergy, true);
            if (clip != null)
                KSoundManager.PlayAudio(clip);
        }
        public void PlayCollectEnergy()
        {
            AudioClip clip = LoadAudioClip(M3SoundClip.collectenergy, true);
            if (clip != null)
                KSoundManager.PlayAudio(clip);
        }
        public void PlayEleminateBottle()
        {
            AudioClip clip = LoadAudioClip(M3SoundClip.eleminateenergy, true);
            if (clip != null)
                KSoundManager.PlayAudio(clip);
        }
        public void PlayEleminateGift()
        {
            AudioClip clip = LoadAudioClip(M3SoundClip.eleminategift, true);
            if (clip != null)
                KSoundManager.PlayAudio(clip);
        }
        public void PlayEleminateCattery()
        {
            AudioClip clip = LoadAudioClip(M3SoundClip.eleminatecattery, true);
            if (clip != null)
                KSoundManager.PlayAudio(clip);
        }
        public void PlayEleminateCoomShield()
        {
            AudioClip clip = LoadAudioClip(M3SoundClip.eleminatecoomshield, true);
            if (clip != null)
                KSoundManager.PlayAudio(clip);
        }
        public void PlayEleminateCoom()
        {
            AudioClip clip = LoadAudioClip(M3SoundClip.eleminatecoom, true);
            if (clip != null)
                KSoundManager.PlayAudio(clip);
        }
        public void PlayEleminateCoin()
        {
            AudioClip clip = LoadAudioClip(M3SoundClip.eleminatecoin, true);
            if (clip != null)
                KSoundManager.PlayAudio(clip);
        }
        public void PlayEleminateVenom()
        {
            AudioClip clip = LoadAudioClip(M3SoundClip.eleminatevenom, true);
            if (clip != null)
                KSoundManager.PlayAudio(clip);
        }
        public void PlayEleminateLock()
        {
            AudioClip clip = LoadAudioClip(M3SoundClip.eleminatechain, true);
            if (clip != null)
                KSoundManager.PlayAudio(clip);
        }
        public void PlayEleminateIce()
        {
            AudioClip clip = LoadAudioClip(M3SoundClip.eleminateice, true);
            if (clip != null)
                KSoundManager.PlayAudio(clip);
        }
        public void PlayEleminateCurtain()
        {
            AudioClip clip = LoadAudioClip(M3SoundClip.eleminatecurtain, true);
            if (clip != null)
                KSoundManager.PlayAudio(clip);
        }
        public void PlayEleminateBook()
        {
            AudioClip clip = LoadAudioClip(M3SoundClip.eleminatebook, true);
            if (clip != null)
                KSoundManager.PlayAudio(clip);
        }
        public void PlayLineToLineAudio()
        {
            AudioClip clip = LoadAudioClip(M3SoundClip.linetoline, true);
            if (clip != null)
                KSoundManager.PlayAudio(clip);
        }
        public void PlayLineToWrapAudio()
        {
            AudioClip clip = LoadAudioClip(M3SoundClip.linetowrap, true);
            if (clip != null)
                KSoundManager.PlayAudio(clip);
        }
        public void PlayWrapToWrapAudio()
        {
            AudioClip clip = LoadAudioClip(M3SoundClip.wraptowrap, true);
            if (clip != null)
                KSoundManager.PlayAudio(clip);
        }
        public void PlayLineToColorAudio()
        {
            AudioClip clip = LoadAudioClip(M3SoundClip.linetocolor, true);
            if (clip != null)
                KSoundManager.PlayAudio(clip);
        }
        public void PlayColorToWrapAudio()
        {
            AudioClip clip = LoadAudioClip(M3SoundClip.colortowrap, true);
            if (clip != null)
                KSoundManager.PlayAudio(clip);
        }
        public void PlayColorToColorAudio()
        {
            AudioClip clip = LoadAudioClip(M3SoundClip.colortocolor, true);
            if (clip != null)
                KSoundManager.PlayAudio(clip);
        }
        public void PlayElimainateCrit()
        {
            AudioClip clip = LoadAudioClip(M3SoundClip.eleminatecrit, true);
            if (clip != null)
                KSoundManager.PlayAudio(clip);
        }


        public void PlayEliminateWrapSpecialAudio()
        {
            AudioClip clip = LoadAudioClip(M3SoundClip.eleminatewrap, true);
            if (clip != null)
                KSoundManager.PlayAudio(clip);
        }
        public void PlayEliminateColorSpecialAudio()
        {
            AudioClip clip = LoadAudioClip(M3SoundClip.eleminatecolor, true);
            if (clip != null)
                KSoundManager.PlayAudio(clip);
        }
        public void PlayEliminateLineSpecialAudio()
        {
            AudioClip clip = LoadAudioClip(M3SoundClip.eleminateline, true);
            if (clip != null)
                KSoundManager.PlayAudio(clip);
        }


        public void PlayCreateWrapSpecialAudio()
        {
            AudioClip clip = LoadAudioClip(M3SoundClip.createwrap, true);
            if (clip != null)
                KSoundManager.PlayAudio(clip);
        }
        public void PlayCreateColorSpecialAudio()
        {
            AudioClip clip = LoadAudioClip(M3SoundClip.createcolor, true);
            if (clip != null)
                KSoundManager.PlayAudio(clip);
        }
        public void PlayCreateLineSpecialAudio()
        {
            AudioClip clip = LoadAudioClip(M3SoundClip.createline, true);
            if (clip != null)
                KSoundManager.PlayAudio(clip);
        }

        public void PlayAutoTips()
        {
            AudioClip clip = LoadAudioClip(M3SoundClip.autotips, true);
            if (clip != null)
                KSoundManager.PlayAudio(clip);
        }
        public void PlayExchangeElement()
        {
            AudioClip clip = LoadAudioClip(M3SoundClip.exchange, true);
            if (clip != null)
                KSoundManager.PlayAudio(clip);
        }

        public void PlaySelectElement()
        {
            AudioClip clip = LoadAudioClip(M3SoundClip.select, true);
            if (clip != null)
                KSoundManager.PlayAudio(clip);
        }

        public void PlayerElementLandedAudio()
        {
            int num = M3Supporter.Instance.GetRandomInt(1, 4);
            string clipName = M3SoundClip.dropLand + num;
            AudioClip clip = LoadAudioClip(clipName, true);
            if (clip != null)
                KSoundManager.PlayAudio(clip);
        }
        public void PlayEliminateAudio(int comboCount)
        {
            comboCount = Mathf.Max(1, comboCount);
            comboCount = Mathf.Min(8, comboCount);
            AudioClip clip = LoadAudioClip(string.Format(M3SoundClip.eliminate, comboCount + 1, comboCount), true);
            if (clip != null)
                KSoundManager.PlayAudio(clip);
        }


        public AudioClip LoadAudioClip(string clipName, bool isAudio)
        {
            if (isAudio)
                clipName = audioPath + clipName;
            else
                clipName = musicPath + clipName;
            AudioClip clip;
            if (KAssetManager.Instance.TryGetSoundAsset(clipName, out clip))
            {
                return clip;
            }
            else
                return null;
        }

    }
}