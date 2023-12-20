using BepInEx;
using BepInEx.Configuration;
using EmotesAPI;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Networking;
using BepInEx.Bootstrap;
using BadAssEmotes;

namespace ExamplePlugin
{
    [BepInDependency("com.weliveinasociety.CustomEmotesAPI")]
    [BepInDependency("com.valve.CSS", BepInDependency.DependencyFlags.SoftDependency)]
    [BepInPlugin(PluginGUID, PluginName, PluginVersion)]
    public class BadAssEmotesPlugin : BaseUnityPlugin
    {
        public const string PluginGUID = "com.weliveinasociety.badasscompany";
        public const string PluginAuthor = "Nunchuk";
        public const string PluginName = "BadAssCompany";
        public const string PluginVersion = "1.0.0";
        int stageInt = -1;
        int pressInt = -1;
        internal static GameObject stage;
        internal static GameObject press;
        internal static GameObject pressMechanism;
        internal static LivingParticleArrayController LPAC;
        public static BadAssEmotesPlugin instance;
        static List<string> HatKidDances = new List<string>();
        public static PluginInfo PInfo { get; private set; }

        //internal static void TestFunction(BoneMapper mapper)
        //{
        //    mapper.audioObjects[mapper.currentClip.syncPos].GetComponent<AudioSource>().PlayOneShot(BoneMapper.primaryAudioClips[mapper.currentClip.syncPos][mapper.currEvent]); //replace this with the audio manager eventually
        //}
        public void Awake()
        {
            instance = this;
            PInfo = Info;
            Assets.LoadAssetBundlesFromFolder("assetbundles");


            Settings.Setup();
            AnimationClipParams param = new AnimationClipParams();
            param.animationClip = new AnimationClip[] { Assets.Load<AnimationClip>($"@ExampleEmotePlugin_badassemotes:assets/badassemotes/Breakin.anim") };
            param.looping = false;
            param._primaryAudioClips = [Assets.Load<AudioClip>($"assets/compressedaudio/breakin\'.ogg")];
            param.dimWhenClose = true;
            param.syncAnim = true;
            param.syncAudio = true;
            param.lockType = AnimationClipParams.LockType.headBobbing;
            param._primaryDMCAFreeAudioClips = [Assets.Load<AudioClip>($"assets/DMCAMusic/Breakin\'_NNTranscription.ogg")];
            //param.customPostEventCodeSync = TestFunction;
            CustomEmotesAPI.AddCustomAnimation(param);
            //CustomEmotesAPI.AddCustomAnimation(Assets.Load<AnimationClip>($"@ExampleEmotePlugin_badassemotes:assets/badassemotes/Breakin.anim"), false, $"Play_Breakin_", $"Stop_Breakin_", dimWhenClose: true, syncAnim: true, syncAudio: true);
            AddAnimation("Breakneck", new AudioClip[] { Assets.Load<AudioClip>($"assets/compressedaudio/Breakneck.ogg") }, null, true, true, true, AnimationClipParams.LockType.headBobbing, "", new AudioClip[] { Assets.Load<AudioClip>($"assets/compressedaudio/Breakneck.ogg") }, null, false, .7f);
            AddAnimation("Crabby", "Crabby", "", true, true, true, AnimationClipParams.LockType.rootMotion, "", "Crabby", "", false, .7f);
            AddAnimation("Dabstand", "Dabstand", "", false, false, false, AnimationClipParams.LockType.headBobbing, "", "Dabstand", "", false, .05f);
            AddAnimation("DanceMoves", "Fortnite default dance sound", "", false, true, true, AnimationClipParams.LockType.headBobbing, "Default Dance", "Fortnite default dance sound", "", false, .7f);
            AddAnimation("DanceTherapyIntro", "DanceTherapyLoop", "Dance Therapy Intro", "Dance Therapy Loop", true, true, true, AnimationClipParams.LockType.headBobbing, "Dance Therapy", "Dance Therapy Intro", "Dance Therapy Loop", false, .7f);
            AddAnimation("DeepDab", "Dabstand", "", false, false, false, AnimationClipParams.LockType.rootMotion, "Deep Dab", "Dabstand", "", false, .05f);
            AddAnimation("Droop", "Droop", "", true, true, true, AnimationClipParams.LockType.headBobbing, "", "Droop", "", false, .7f);
            AddAnimation("ElectroSwing", "ElectroSwing", "", true, true, true, AnimationClipParams.LockType.headBobbing, "Electro Swing", "ElectroSwing", "", false, .7f);
            AddAnimation("Extraterrestial", "Extraterestrial", "", true, true, true, AnimationClipParams.LockType.rootMotion, "", "Extraterestrial", "", false, .7f);
            AddAnimation("FancyFeet", "FancyFeet", "", true, true, true, AnimationClipParams.LockType.headBobbing, "Fancy Feet", "FancyFeet", "", false, .7f);
            AddAnimation("FlamencoIntro", "FlamencoLoop", "Flamenco", "FlamencoLoop", true, true, true, AnimationClipParams.LockType.headBobbing, "Flamenco", "FlamencoLoop", "", false, .7f);
            AddAnimation("Floss", "Floss", "", true, true, false, AnimationClipParams.LockType.rootMotion, "", "Floss", "", false, .5f);
            AddAnimation("Fresh", "Fresh", "", true, true, true, AnimationClipParams.LockType.headBobbing, "", "Fresh", "", false, .7f);
            AddAnimation("Hype", "Hype", "", true, true, true, AnimationClipParams.LockType.headBobbing, "", "Hype", "", false, .7f);
            AddAnimation("Infectious", "Infectious", "", true, true, true, AnimationClipParams.LockType.headBobbing, "", "Infectious", "", false, .7f);
            AddAnimation("InfinidabIntro", "InfinidabLoop", "InfinidabIntro", "InfinidabLoop", true, true, false, AnimationClipParams.LockType.headBobbing, "Infinidab", "InfinidabLoop", "", false, .1f);
            //AddAnimation("IntensityIntro", "Intensity", "IntensityLoop", true, true, "");//TODO "Intensity" is not a wav file, get help
            AddAnimation("NeverGonna", "Never Gonna Give you Up", "", true, true, true, AnimationClipParams.LockType.headBobbing, "Never Gonna", "Never Gonna Give you Up", "", true, .7f);
            AddAnimation("NinjaStyle", "NinjaStyle", "", true, true, true, AnimationClipParams.LockType.headBobbing, "Ninja Style", "NinjaStyle", "", false, .7f);
            AddAnimation("OldSchool", "Old School", "", true, true, true, AnimationClipParams.LockType.headBobbing, "Oldschool", "Old School", "", false, .7f);
            AddAnimation("OrangeJustice", "Orange Justice", "", true, true, true, AnimationClipParams.LockType.headBobbing, "Orange Justice", "Orange Justice", "", false, .6f);
            AddAnimation("Overdrive", "Overdrive", "", true, true, true, AnimationClipParams.LockType.headBobbing, "", "Overdrive", "", false, .7f);
            AddAnimation("PawsAndClaws", "im a cat", "", true, true, true, AnimationClipParams.LockType.headBobbing, "Paws and Claws", "im a cat", "", false, .7f);
            AddAnimation("PhoneItIn", "Phone It In", "", true, true, true, AnimationClipParams.LockType.headBobbing, "Phone It In", "Phone It In", "", false, .7f);
            AddAnimation("PopLock", "PopLock", "", true, true, true, AnimationClipParams.LockType.headBobbing, "Pop Lock", "PopLock", "", false, .7f);
            AddAnimation("Scenario", "Scenario", "", true, true, true, AnimationClipParams.LockType.headBobbing, "", "Scenario", "", false, .7f);
            AddAnimation("SpringLoaded", "SpringLoaded", "", true, false, false, AnimationClipParams.LockType.headBobbing, "Spring Loaded", "SpringLoaded", "", false, .1f);
            AddAnimation("Springy", "Springy", "", true, true, true, AnimationClipParams.LockType.headBobbing, "", "Springy", "", false, .7f);
            //AddAnimation("SquatKickIntro", "SquatKick", "SquatKickLoop", true, true, "");//TODO "SquatKickLoop" is not a wav file


            //update 1
            AddAnimation("AnkhaZone", "AnkhaZone", "", true, true, true, AnimationClipParams.LockType.headBobbing, "", "AnkhaZone", "", true, .7f);
            AddAnimation("GangnamStyle", "GangnamStyle", "", true, true, true, AnimationClipParams.LockType.headBobbing, "Gangnam Style", "GangnamStyle", "", true, .7f);
            AddAnimation("DontStart", "DontStart", "", true, true, true, AnimationClipParams.LockType.headBobbing, "Don't Start", "DontStart", "", true, .7f);
            AddAnimation("BunnyHop", "BunnyHop", "", true, true, true, AnimationClipParams.LockType.headBobbing, "Bunny Hop", "BunnyHop", "", false, .4f);
            AddAnimation("BestMates", "BestMates", "", true, true, true, AnimationClipParams.LockType.headBobbing, "Best Mates", "BestMates", "", false, .7f);
            AddAnimation("JackOPose", "", "", true, false, false, AnimationClipParams.LockType.headBobbing, "Jack-O Crouch", "", "", false, 0f);
            AddAnimation("Crackdown", "Crackdown", "", true, true, true, AnimationClipParams.LockType.headBobbing, "", "Crackdown", "", false, .7f);
            AddAnimation("Thicc", "Thicc", "", true, true, true, AnimationClipParams.LockType.headBobbing, "", "Thicc", "", false, .7f);
            AddAnimation("TakeTheL", "TakeTheL", "", true, true, true, AnimationClipParams.LockType.headBobbing, "Take The L", "TakeTheL", "", false, .8f);
            AddAnimation("LetsDanceBoys", "LetsDanceBoys", "", true, true, true, AnimationClipParams.LockType.rootMotion, "Let's Dance Boys", "LetsDanceBoys", "", false, .7f);
            AddAnimation("BlindingLightsIntro", "BlindingLights", "BlindingLightsIntro", "BlindingLightsLoop", true, true, true, AnimationClipParams.LockType.headBobbing, "Blinding Lights", "BlindingLightsIntro", "BlindingLightsLoop", true, .7f);
            AddAnimation("ImDiamond", "ImDiamond", "", true, true, true, AnimationClipParams.LockType.headBobbing, "I'm Diamond", "ImDiamond", "", true, .7f);
            AddAnimation("ItsDynamite", "ItsDynamite", "", true, true, true, AnimationClipParams.LockType.headBobbing, "It's Dynamite", "ItsDynamite", "", true, .7f);
            AddAnimation("TheRobot", "TheRobot", "", true, true, true, AnimationClipParams.LockType.headBobbing, "The Robot", "TheRobot", "", false, .7f);
            AddAnimation("Cartwheelin", "Cartwheelin", "", true, false, false, AnimationClipParams.LockType.headBobbing, "", "Cartwheelin", "", false, .1f);
            AddAnimation("CrazyFeet", "CrazyFeet", "", true, true, true, AnimationClipParams.LockType.headBobbing, "Crazy Feet", "CrazyFeet", "", false, .7f);
            AddAnimation("FullTilt", "FullTilt", "", true, true, true, AnimationClipParams.LockType.rootMotion, "Full Tilt", "FullTilt", "", false, .1f);
            AddAnimation("FloorSamus", "", "", true, false, false, AnimationClipParams.LockType.rootMotion, "Samus Crawl", "", "", false, 0f);
            AddAnimation("DEDEDE", "", "", true, false, false, AnimationClipParams.LockType.headBobbing, "", "", "", false, 0f);
            AddAnimation("Specialist", "Specialist", "", false, true, true, AnimationClipParams.LockType.rootMotion, "The Specialist", "Specialist", "", false, .7f);



            //Update 2
            AddStartAndJoinAnim(new string[] { "PPmusic", "PPmusicFollow" }, "PPmusic", true, true, true, AnimationClipParams.LockType.headBobbing, "Penis Music", "PPmusic", false, .7f);
            AddAnimation("GetDown", "GetDown", "", false, true, true, AnimationClipParams.LockType.rootMotion, "Get Down", "GetDown", "", true, .7f);
            AddAnimation("Yakuza", "Yakuza", "", true, true, true, AnimationClipParams.LockType.rootMotion, "Koi no Disco Queen", "Yakuza", "", false, .7f);
            AddAnimation("Miku", "Miku", "", true, true, true, AnimationClipParams.LockType.rootMotion, "", "Miku", "", false, .7f);
            AddAnimation("Horny", new string[] { "Horny", "TeddyLoid - ME!ME!ME! feat. Daoko" }, true, true, true, AnimationClipParams.LockType.headBobbing, "", new string[] { "Horny", "TeddyLoid - ME!ME!ME! feat. Daoko" }, true, .7f);
            AddAnimation("GangTorture", "GangTorture", "", false, true, true, AnimationClipParams.LockType.rootMotion, "", "GangTorture", "", true, .7f);
            AddAnimation("PoseBurter", "GinyuForce", "", true, true, true, AnimationClipParams.LockType.headBobbing, "Burter Pose", "GinyuForce", "", false, .7f);
            AddAnimation("PoseGinyu", "GinyuForce", "", true, true, true, AnimationClipParams.LockType.headBobbing, "Ginyu Pose", "GinyuForce", "", false, .7f);
            AddAnimation("PoseGuldo", "GinyuForce", "", true, true, true, AnimationClipParams.LockType.headBobbing, "Guldo Pose", "GinyuForce", "", false, .7f);
            AddAnimation("PoseJeice", "GinyuForce", "", true, true, true, AnimationClipParams.LockType.headBobbing, "Jeice Pose", "GinyuForce", "", false, .7f);
            AddAnimation("PoseRecoome", "GinyuForce", "", true, true, true, AnimationClipParams.LockType.headBobbing, "Recoome Pose", "GinyuForce", "", false, .7f);
            //TODO come back to this
            //AddAnimation("StoodHere", new string[] { "Play_StandingHere2" }, "StandingHere2", true, true, false, new JoinSpot[] { new JoinSpot("StandingHereJoinSpot", new Vector3(0, 0, 2)) });
            //CustomEmotesAPI.AddCustomAnimation(Assets.Load<AnimationClip>($"@ExampleEmotePlugin_badassemotes:assets/badassemotes/StandingHere.anim"), true, visible: false);
            //BoneMapper.animClips["StandingHere"].vulnerableEmote = true;
            AddAnimation("Carson", "Carson", "", false, true, true, AnimationClipParams.LockType.headBobbing, "", "Carson", "", true, .7f);
            AddAnimation("Frolic", "Frolic", "", true, true, true, AnimationClipParams.LockType.headBobbing, "", "Frolic", "", false, .7f);
            AddAnimation("MoveIt", "MoveIt", "", true, true, true, AnimationClipParams.LockType.headBobbing, "Move It", "MoveIt", "", true, .7f);
            AddStartAndJoinAnim(new string[] { "ByTheFireLead", "ByTheFirePartner" }, "ByTheFire", true, true, true, AnimationClipParams.LockType.headBobbing, "By The Fire", "ByTheFire", true, .7f);
            AddStartAndJoinAnim(new string[] { "SwayLead", "SwayPartner" }, "Sway", true, true, true, AnimationClipParams.LockType.headBobbing, "Sway", "Sway", true, .7f);
            AddAnimation("Macarena", "Macarena", "", true, true, true, AnimationClipParams.LockType.headBobbing, "", "Macarena", "", true, .7f);
            AddAnimation("Thanos", "Thanos", "", true, true, true, AnimationClipParams.LockType.headBobbing, "", "Thanos", "", true, .7f);
            AddAnimation("StarGet", new AudioClip[] { Assets.Load<AudioClip>($"assets/compressedaudio/starget2.ogg"), Assets.Load<AudioClip>($"assets/compressedaudio/starget3.ogg"), Assets.Load<AudioClip>($"assets/compressedaudio/starget4.ogg"), Assets.Load<AudioClip>($"assets/compressedaudio/starget5.ogg"), Assets.Load<AudioClip>($"assets/compressedaudio/starget6.ogg"), Assets.Load<AudioClip>($"assets/compressedaudio/starget7.ogg") }, null, false, false, false, AnimationClipParams.LockType.rootMotion, "Star Get", new AudioClip[] { Assets.Load<AudioClip>($"assets/DMCAMusic/starget2_NNTranscription.ogg"), Assets.Load<AudioClip>($"assets/DMCAMusic/starget3_NNTranscription.ogg"), Assets.Load<AudioClip>($"assets/DMCAMusic/starget4_NNTranscription.ogg"), Assets.Load<AudioClip>($"assets/DMCAMusic/starget5_NNTranscription.ogg"), Assets.Load<AudioClip>($"assets/DMCAMusic/starget6_NNTranscription.ogg"), Assets.Load<AudioClip>($"assets/DMCAMusic/starget7_NNTranscription.ogg") }, null, false, .6f);
            AddAnimation("Poyo", "Poyo", "", true, true, true, AnimationClipParams.LockType.headBobbing, "", "Poyo", "", false, .7f);
            //AddAnimation("Hi", "Hi", false, false, false);//TODO no hi wav
            AddAnimation("Chika", "Chika", "", false, true, true, AnimationClipParams.LockType.rootMotion, "", "Chika", "", true, .7f);
            AddAnimation("Goopie", "Goopie", "", false, true, true, AnimationClipParams.LockType.headBobbing, "", "Goopie", "", true, .7f);
            //AddAnimation("Sad", "Sad", false, true, true, "");//TODO no sad wav
            AddAnimation("Crossbounce", "Crossbounce", "", true, true, true, AnimationClipParams.LockType.headBobbing, "", "Crossbounce", "", false, .7f);
            AddAnimation("Butt", "Butt", "", false, false, false, AnimationClipParams.LockType.lockHead, "", "Butt", "", false, .5f);



            //update 3
            //AddAnimation("SuperIdol", "SuperIdol", "", true, true, true, AnimationClipParams.LockType.headBobbing, "Super Idol");//TODO second anim?
            AddAnimation("MakeItRainIntro", "MakeItRainLoop", "MakeItRainIntro", "MakeItRainLoop", true, true, true, AnimationClipParams.LockType.rootMotion, "Make it Rain", "MakeItRainLoop", "", true, .7f);
            AddAnimation("Penguin", "Penguin", "", true, true, true, AnimationClipParams.LockType.headBobbing, "Club Penguin", "Penguin", "", false, .4f);
            AddAnimation("DesertRivers", "DesertRivers", "", false, true, true, AnimationClipParams.LockType.rootMotion, "Rivers in the Dessert", "DesertRivers", "", false, .7f);
            AddAnimation("HondaStep", "HondaStep", "", false, true, true, AnimationClipParams.LockType.rootMotion, "Step!", "HondaStep", "", true, .7f);
            AddAnimation("UGotThat", "UGotThat", "", false, true, true, AnimationClipParams.LockType.rootMotion, "U Got That", "UGotThat", "", true, .7f);



            //update 4
            AddAnimation("OfficerEarl", "", "", true, false, false, AnimationClipParams.LockType.rootMotion, "Officer Earl", "", "", false, 0f);
            AddAnimation("Cirno", "Cirno", "", false, true, true, AnimationClipParams.LockType.rootMotion, "", "Cirno", "", false, .7f);
            CustomEmotesAPI.AddCustomAnimation(new AnimationClipParams() { animationClip = new AnimationClip[] { Assets.Load<AnimationClip>($"@ExampleEmotePlugin_badassemotes:assets/badassemotes/Haruhi.anim") }, looping = false, _primaryAudioClips = new AudioClip[] { Assets.Load<AudioClip>($"assets/compressedaudio/Haruhi.ogg"), Assets.Load<AudioClip>($"assets/compressedaudio/HaruhiYoung.ogg") }, dimWhenClose = true, syncAnim = true, syncAudio = true, startPref = 0, joinPref = 0, joinSpots = new JoinSpot[] { new JoinSpot("Yuki_Nagato", new Vector3(1.5f, 0, -1.5f)), new JoinSpot("Mikuru_Asahina", new Vector3(-1.5f, 0, -1.5f)) }, customName = "Hare Hare Yukai", lockType = AnimationClipParams.LockType.rootMotion, willGetClaimedByDMCA = true, _primaryDMCAFreeAudioClips = new AudioClip[] { Assets.Load<AudioClip>($"assets/DMCAMusic/Haruhi_NNTranscription.ogg"), Assets.Load<AudioClip>($"assets/DMCAMusic/HaruhiYoung_NNTranscription.ogg") }, audioLevel = .7f });
            CustomEmotesAPI.AddCustomAnimation(new AnimationClipParams() { animationClip = new AnimationClip[] { Assets.Load<AnimationClip>($"@ExampleEmotePlugin_badassemotes:assets/badassemotes/Yuki_Nagato.anim") }, looping = false, syncAnim = true, visible = false, lockType = AnimationClipParams.LockType.lockHead, audioLevel = 0 });
            CustomEmotesAPI.AddCustomAnimation(new AnimationClipParams() { animationClip = new AnimationClip[] { Assets.Load<AnimationClip>($"@ExampleEmotePlugin_badassemotes:assets/badassemotes/Mikuru_Asahina.anim") }, looping = false, syncAnim = true, visible = false, lockType = AnimationClipParams.LockType.lockHead, audioLevel = 0 });

            BoneMapper.animClips["Yuki_Nagato"].vulnerableEmote = true;
            BoneMapper.animClips["Mikuru_Asahina"].vulnerableEmote = true;
            CustomEmotesAPI.AddCustomAnimation(new AnimationClipParams() { animationClip = new AnimationClip[] { Assets.Load<AnimationClip>($"@ExampleEmotePlugin_badassemotes:assets/badassemotes/GGGG.anim"), Assets.Load<AnimationClip>($"@ExampleEmotePlugin_badassemotes:assets/badassemotes/GGGG2.anim"), Assets.Load<AnimationClip>($"@ExampleEmotePlugin_badassemotes:assets/badassemotes/GGGG3.anim") }, looping = false, _primaryAudioClips = new AudioClip[] { Assets.Load<AudioClip>($"assets/compressedaudio/GGGG.ogg") }, dimWhenClose = true, syncAnim = true, syncAudio = true, startPref = -2, joinPref = -2, lockType = AnimationClipParams.LockType.rootMotion, willGetClaimedByDMCA = true, _primaryDMCAFreeAudioClips = new AudioClip[] { Assets.Load<AudioClip>($"assets/DMCAMusic/GGGG_NNTranscription.ogg") }, audioLevel = .7f });
            AddAnimation("Shufflin", "Shufflin", "", false, true, true, AnimationClipParams.LockType.rootMotion, "", "Shufflin", "", true, .7f);
            //AddStartAndJoinAnim(new string[] { "Train", "TrainPassenger" }, "Train", new string[] { "Trainloop", "TrainPassenger" }, true, false, true, true, true);
            //BoneMapper.animClips["Train"].vulnerableEmote = true;
            //CustomEmotesAPI.BlackListEmote("Train");

            AddAnimation("BimBamBom", "BimBamBom", "", true, true, true, AnimationClipParams.LockType.headBobbing, "Bim Bam Boom", "BimBamBom", "", false, .7f);
            AddAnimation("Savage", "Savage", "", true, true, true, AnimationClipParams.LockType.headBobbing, "", "Savage", "", true, .7f);
            AddAnimation("Stuck", "Stuck", "", true, true, true, AnimationClipParams.LockType.headBobbing, "", "Stuck", "", true, .7f);
            AddAnimation("Roflcopter", "Roflcopter", "", true, true, true, AnimationClipParams.LockType.headBobbing, "", "Roflcopter", "", false, .7f);
            AddAnimation("Float", "Float", "", false, true, true, AnimationClipParams.LockType.headBobbing, "", "Float", "", false, .4f);
            AddAnimation("Rollie", "Rollie", "", true, true, true, AnimationClipParams.LockType.headBobbing, "", "Rollie", "", true, .7f);
            AddAnimation("GalaxyObservatory", new string[] { "GalaxyObservatory1", "GalaxyObservatory2", "GalaxyObservatory3" }, true, true, true, AnimationClipParams.LockType.rootMotion, "Galaxy Observatory", new string[] { "GalaxyObservatory1", "GalaxyObservatory2", "GalaxyObservatory3" }, false, .5f);
            AddAnimation("Markiplier", "Markiplier", "", false, false, false, AnimationClipParams.LockType.rootMotion, "", "Markiplier", "", true, .6f);
            AddAnimation("DevilSpawn", "DevilSpawn", "", true, true, true, AnimationClipParams.LockType.headBobbing, "", "DevilSpawn", "", true, .7f);
            CustomEmotesAPI.AddCustomAnimation(new AnimationClipParams() { animationClip = new AnimationClip[] { Assets.Load<AnimationClip>($"@ExampleEmotePlugin_badassemotes:assets/badassemotes/DuckThisOneIdle.anim") }, looping = true, _primaryAudioClips = new AudioClip[] { Assets.Load<AudioClip>($"assets/compressedaudio/DuckThisOneIdle.ogg") }, joinSpots = new JoinSpot[] { new JoinSpot("DuckThisJoinSpot", new Vector3(0, 0, 2)) }, dimWhenClose = true, lockType = AnimationClipParams.LockType.lockHead, customName = "Duck This One", _primaryDMCAFreeAudioClips = new AudioClip[] { Assets.Load<AudioClip>($"assets/DMCAMusic/DuckThisOneIdle_NNTranscription.ogg") }, willGetClaimedByDMCA = false, audioLevel = .7f });
            //CustomEmotesAPI.BlackListEmote("DuckThisOneIdle");
            CustomEmotesAPI.AddCustomAnimation(new AnimationClipParams() { animationClip = new AnimationClip[] { Assets.Load<AnimationClip>($"@ExampleEmotePlugin_badassemotes:assets/badassemotes/DuckThisOne.anim") }, looping = false, _primaryAudioClips = new AudioClip[] { Assets.Load<AudioClip>($"assets/compressedaudio/DuckThisOne.ogg") }, visible = false, dimWhenClose = true, lockType = AnimationClipParams.LockType.lockHead, _primaryDMCAFreeAudioClips = new AudioClip[] { Assets.Load<AudioClip>($"assets/compressedaudio/DuckThisOne.ogg") } });
            CustomEmotesAPI.AddCustomAnimation(new AnimationClipParams() { animationClip = new AnimationClip[] { Assets.Load<AnimationClip>($"@ExampleEmotePlugin_badassemotes:assets/badassemotes/DuckThisOneJoin.anim") }, looping = false, visible = false, dimWhenClose = true, lockType = AnimationClipParams.LockType.lockHead });
            BoneMapper.animClips["DuckThisOneIdle"].vulnerableEmote = true;
            BoneMapper.animClips["DuckThisOne"].vulnerableEmote = true;
            BoneMapper.animClips["DuckThisOneJoin"].vulnerableEmote = true;
            AddAnimation("Griddy", "Griddy", "", true, true, true, AnimationClipParams.LockType.headBobbing, "", "Griddy", "", true, .7f);
            AddAnimation("Tidy", "Tidy", "", true, true, true, AnimationClipParams.LockType.headBobbing, "", "Tidy", "", true, .7f);
            AddAnimation("Toosie", "Toosie", "", true, true, true, AnimationClipParams.LockType.headBobbing, "", "Toosie", "", true, .7f);
            AddAnimation("INEEDAMEDICBAG", new AudioClip[] { Assets.Load<AudioClip>($"assets/compressedaudio/INEEDAMEDICBAG1.ogg"), Assets.Load<AudioClip>($"assets/compressedaudio/INEEDAMEDICBAG2.ogg") }, null, false, false, false, AnimationClipParams.LockType.rootMotion, "I NEED A MEDIC BAG", new AudioClip[] { Assets.Load<AudioClip>($"assets/compressedaudio/INEEDAMEDICBAG1.ogg"), Assets.Load<AudioClip>($"assets/compressedaudio/INEEDAMEDICBAG2.ogg") }, null, false, .9f);
            AddAnimation("Smoke", "Smoke", "", true, true, true, AnimationClipParams.LockType.rootMotion, "Ralsei Dies", "Smoke", "", false, .7f);
            AddAnimation("FamilyGuyDeath", "", "", true, false, false, AnimationClipParams.LockType.headBobbing, "Family Guy Death Pose", "", "", false, 0f);
            AddAnimation("Panda", "", "", false, false, false, AnimationClipParams.LockType.rootMotion, "", "", "", false, 0f);
            AddAnimation("Yamcha", "", "", true, false, false, AnimationClipParams.LockType.headBobbing, "Yamcha Death Pose", "", "", false, 0f);


            //update 5
            AddAnimation("Thriller", "Thriller", "", true, true, true, AnimationClipParams.LockType.rootMotion, "", "Thriller", "", true, .7f);
            AddAnimation("Wess", "Wess", "", false, true, true, AnimationClipParams.LockType.headBobbing, "", "Wess", "", true, .7f);
            AddAnimation("Distraction", "Distraction", "", true, true, true, AnimationClipParams.LockType.headBobbing, "Distraction Dance", "Distraction", "", false, 1f);
            AddAnimation("Security", "Security", "", true, true, true, AnimationClipParams.LockType.headBobbing, "Last Surprise", "Security", "", false, .7f);


            //update 6
            AddAnimation("KillMeBaby", "KillMeBaby", "", false, true, true, AnimationClipParams.LockType.headBobbing, "Kill Me Baby", "KillMeBaby", "", true, .7f);
            //CustomEmotesAPI.AddCustomAnimation(new AnimationClipParams() { animationClip = new AnimationClip[] { Assets.Load<AnimationClip>($"@ExampleEmotePlugin_badassemotes:assets/badassemotes/MyWorld.anim") }, looping = true, _primaryAudioClips = new AudioClip[] { Assets.Load<AudioClip>($"assets/compressedaudio/MyWorld.ogg") }, dimWhenClose = true, syncAnim = true, syncAudio = true, joinSpots = new JoinSpot[] { new JoinSpot("MyWorldJoinSpot", new Vector3(2, 0, 0)) } });
            //CustomEmotesAPI.AddCustomAnimation(new AnimationClipParams() { animationClip = new AnimationClip[] { Assets.Load<AnimationClip>($"@ExampleEmotePlugin_badassemotes:assets/badassemotes/MyWorldJoin.anim") }, looping = true, _primaryAudioClips = new AudioClip[] { Assets.Load<AudioClip>($"assets/compressedaudio/MyWorld.ogg") }, dimWhenClose = true, syncAnim = true, syncAudio = true, visible = false });
            //BoneMapper.animClips["MyWorldJoin"].syncPos--;
            //BoneMapper.animClips["MyWorldJoin"].vulnerableEmote = true;
            CustomEmotesAPI.AddCustomAnimation(new AnimationClipParams() { animationClip = new AnimationClip[] { Assets.Load<AnimationClip>($"@ExampleEmotePlugin_badassemotes:assets/badassemotes/VSWORLD.anim") }, looping = true, _primaryAudioClips = new AudioClip[] { Assets.Load<AudioClip>($"assets/compressedaudio/VSWORLD.ogg") }, dimWhenClose = true, syncAnim = true, syncAudio = true, joinSpots = new JoinSpot[] { new JoinSpot("VSWORLDJoinSpot", new Vector3(-2, 0, 0)) }, lockType = AnimationClipParams.LockType.headBobbing, _primaryDMCAFreeAudioClips = new AudioClip[] { Assets.Load<AudioClip>($"assets/DMCAMusic/VSWORLD_NNTranscription.ogg") }, audioLevel= .7f });
            CustomEmotesAPI.AddCustomAnimation(new AnimationClipParams() { animationClip = new AnimationClip[] { Assets.Load<AnimationClip>($"@ExampleEmotePlugin_badassemotes:assets/badassemotes/VSWORLDLEFT.anim") }, looping = true, _primaryAudioClips = new AudioClip[] { Assets.Load<AudioClip>($"assets/compressedaudio/VSWORLD.ogg") }, dimWhenClose = true, syncAnim = true, syncAudio = true, visible = false, lockType = AnimationClipParams.LockType.headBobbing, _primaryDMCAFreeAudioClips = new AudioClip[] { Assets.Load<AudioClip>($"assets/DMCAMusic/VSWORLD_NNTranscription.ogg") } });
            BoneMapper.animClips["VSWORLDLEFT"].syncPos--;
            BoneMapper.animClips["VSWORLDLEFT"].vulnerableEmote = true;
            CustomEmotesAPI.AddCustomAnimation(new AnimationClipParams() { animationClip = new AnimationClip[] { Assets.Load<AnimationClip>($"@ExampleEmotePlugin_badassemotes:assets/badassemotes/ChugJug.anim") }, looping = false, _primaryAudioClips = new AudioClip[] { Assets.Load<AudioClip>($"assets/compressedaudio/ChugJug.ogg"), Assets.Load<AudioClip>($"assets/compressedaudio/MikuJug.ogg") }, dimWhenClose = true, syncAnim = true, syncAudio = true, lockType = AnimationClipParams.LockType.headBobbing, _primaryDMCAFreeAudioClips = new AudioClip[] { Assets.Load<AudioClip>($"assets/DMCAMusic/ChugJug_NNTranscription.ogg"), Assets.Load<AudioClip>($"assets/DMCAMusic/MikuJug_NNTranscription.ogg") }, audioLevel = .7f });

            //TODO IFU
            //CustomEmotesAPI.AddNonAnimatingEmote("IFU Stage");
            //CustomEmotesAPI.BlackListEmote("IFU Stage");
            //CustomEmotesAPI.AddCustomAnimation(new AnimationClip[] { Assets.Load<AnimationClip>($"@ExampleEmotePlugin_badassemotes:assets/badassemotes/ifu.anim") }, false, new string[] { "Play_ifu", "Play_ifucover" }, new string[] { "Stop_ifu", "Stop_ifu" }, dimWhenClose: true, syncAnim: true, syncAudio: true, visible: false);
            //CustomEmotesAPI.AddCustomAnimation(new AnimationClip[] { Assets.Load<AnimationClip>($"@ExampleEmotePlugin_badassemotes:assets/badassemotes/ifeleft.anim") }, false, new string[] { "Play_ifu", "Play_ifucover" }, new string[] { "Stop_ifu", "Stop_ifu" }, dimWhenClose: true, syncAnim: true, syncAudio: true, visible: false);
            //BoneMapper.animClips["ifeleft"].syncPos--;
            //CustomEmotesAPI.AddCustomAnimation(new AnimationClip[] { Assets.Load<AnimationClip>($"@ExampleEmotePlugin_badassemotes:assets/badassemotes/ifuright.anim") }, false, new string[] { "Play_ifu", "Play_ifucover" }, new string[] { "Stop_ifu", "Stop_ifu" }, dimWhenClose: true, syncAnim: true, syncAudio: true, visible: false);
            //BoneMapper.animClips["ifuright"].syncPos -= 2;
            //BoneMapper.animClips["ifu"].vulnerableEmote = true;
            //BoneMapper.animClips["ifeleft"].vulnerableEmote = true;
            //BoneMapper.animClips["ifuright"].vulnerableEmote = true;
            //GameObject g2 = Assets.Load<GameObject>($"assets/prefabs/ifustagebasebased.prefab");
            //var g = g2.transform.Find("ifuStage").Find("GameObject").Find("LivingParticlesFloor11_Audio").gameObject;
            //g2.AddComponent<StageHandler>();
            //g2.transform.Find("ifuStage").Find("GameObject").Find("Plane").gameObject.AddComponent<SurfaceDefProvider>().surfaceDef = Addressables.LoadAssetAsync<SurfaceDef>("RoR2/Base/Common/sdMetal.asset").WaitForCompletion();
            //g2.transform.Find("ifuStage").Find("GameObject").Find("stairs front").gameObject.AddComponent<SurfaceDefProvider>().surfaceDef = Addressables.LoadAssetAsync<SurfaceDef>("RoR2/Base/Common/sdMetal.asset").WaitForCompletion();
            //g2.transform.Find("ifuStage").Find("GameObject").Find("stairs left").gameObject.AddComponent<SurfaceDefProvider>().surfaceDef = Addressables.LoadAssetAsync<SurfaceDef>("RoR2/Base/Common/sdMetal.asset").WaitForCompletion();
            //g2.transform.Find("ifuStage").Find("GameObject").Find("stairs right").gameObject.AddComponent<SurfaceDefProvider>().surfaceDef = Addressables.LoadAssetAsync<SurfaceDef>("RoR2/Base/Common/sdMetal.asset").WaitForCompletion();
            //g2.transform.Find("ifuStage").Find("GameObject").Find("stairs back").gameObject.AddComponent<SurfaceDefProvider>().surfaceDef = Addressables.LoadAssetAsync<SurfaceDef>("RoR2/Base/Common/sdMetal.asset").WaitForCompletion();
            //LivingParticlesAudioModule module = g.GetComponent<LivingParticlesAudioModule>();
            //module.audioPosition = g.transform;
            //g.GetComponent<ParticleSystemRenderer>().material.SetFloat("_DistancePower", .5f);
            //g.GetComponent<ParticleSystemRenderer>().material.SetFloat("_NoisePower", 8f);
            //g.GetComponent<ParticleSystemRenderer>().material.SetFloat("_AudioAmplitudeOffsetPower2", 1.5f);
            //stageInt = CustomEmotesAPI.RegisterWorldProp(g2, new JoinSpot[] { new JoinSpot("ifumiddle", new Vector3(0, .4f, 0)), new JoinSpot("ifeleft", new Vector3(-2, .4f, 0)), new JoinSpot("ifuright", new Vector3(2, .4f, 0)) });

            AddAnimation("Summertime", "Summertime", "", false, true, true, AnimationClipParams.LockType.headBobbing, "", "Summertime", "", true, .7f);
            AddAnimation("Dougie", "Dougie", "", true, true, true, AnimationClipParams.LockType.headBobbing, "", "Dougie", "", true, .7f);



            //Update 7?
            AddAnimation("CaliforniaGirls", "CaliforniaGirls", "", true, true, true, AnimationClipParams.LockType.headBobbing, "California Gurls", "CaliforniaGirls", "", true, .7f);
            AddAnimation("SeeTinh", "SeeTinh", "", false, true, true, AnimationClipParams.LockType.headBobbing, "See Tình", "SeeTinh", "", true, .7f);


            //Update 8ish
            //CustomEmotesAPI.AddNonAnimatingEmote("Hydraulic Press");
            //CustomEmotesAPI.BlackListEmote("Hydraulic Press");
            AddAnimation("VirtualInsanityIntro", "VirtualInsanityLoop", "VirtualInsanityStart", "VirtualInsanityLoop", false, true, true, AnimationClipParams.LockType.headBobbing, "Virtual Insanity", "VirtualInsanityStart", "VirtualInsanityLoop", true, .5f);


            //9?
            //AddAnimation("Terrestrial", "Terrestrial Start", "Terrestrial", true, true, true); //TODO anim missing? not even in unity


            //TODO
            //GameObject pressObject = Assets.Load<GameObject>($"assets/hydrolic/homedepot1.prefab");
            //foreach (var item in pressObject.GetComponentsInChildren<Renderer>())
            //{
            //    foreach (var mat in item.sharedMaterials)
            //    {
            //        //TODO addressables
            //        //mat.shader = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Commando/CommandoBody.prefab").WaitForCompletion().GetComponentInChildren<SkinnedMeshRenderer>().material.shader;
            //    }
            //}
            //pressObject.AddComponent<HydrolicPressMechanism>();
            //pressInt = CustomEmotesAPI.RegisterWorldProp(pressObject, new JoinSpot[] { new JoinSpot("HydrolicPressJoinSpot", new Vector3(0, .1f, 0)) });

            CustomEmotesAPI.animChanged += CustomEmotesAPI_animChanged;
            CustomEmotesAPI.emoteSpotJoined_Body += CustomEmotesAPI_emoteSpotJoined_Body;
            CustomEmotesAPI.emoteSpotJoined_Prop += CustomEmotesAPI_emoteSpotJoined_Prop;
            CustomEmotesAPI.boneMapperCreated += CustomEmotesAPI_boneMapperCreated;

            //TODO game over
            //On.RoR2.Run.OnClientGameOver += Run_OnClientGameOver;
        }

        //TODO on all players dead
        //private void Run_OnClientGameOver(On.RoR2.Run.orig_OnClientGameOver orig, Run self, RunReport runReport)
        //{
        //    orig(self, runReport);
        //    if (NetworkServer.active)
        //    {
        //        if (UnityEngine.Random.Range(1, 101) < Settings.EnemyTauntOnDeathChance.Value + 1)
        //        {
        //            foreach (var item in CustomEmotesAPI.GetAllBoneMappers())
        //            {
        //                if (item.mapperBody.GetComponent<TeamComponent>().teamIndex != TeamIndex.Player)
        //                {
        //                    CustomEmotesAPI.PlayAnimation("DanceMoves", item);
        //                }
        //            }
        //        }
        //    }
        //}

        private void CustomEmotesAPI_boneMapperCreated(BoneMapper mapper)
        {
            //TODO foot effectors for IFU
            //if (LPAC && LPAC.affectors.Length != 0 && !mapper.worldProp)
            //{
            //    List<Transform> transforms = new List<Transform>(LPAC.affectors);
            //    foreach (var bone in mapper.smr2.bones)
            //    {
            //        if (bone.GetComponent<EmoteConstraint>() && (bone.GetComponent<EmoteConstraint>().emoteBone == mapper.a2.GetBoneTransform(HumanBodyBones.LeftFoot) || bone.GetComponent<EmoteConstraint>().emoteBone == mapper.a2.GetBoneTransform(HumanBodyBones.RightFoot)))
            //        {
            //            transforms.Add(bone);
            //        }
            //    }
            //    BadAssEmotesPlugin.LPAC.affectors = transforms.ToArray();
            //}
        }

        private void CustomEmotesAPI_emoteSpotJoined_Prop(GameObject emoteSpot, BoneMapper joiner, BoneMapper host)
        {
            string emoteSpotName = emoteSpot.name;
            GameObject g;
            switch (emoteSpotName)
            {
                case "ifumiddle":
                    host.GetComponentsInChildren<Animator>()[1].SetTrigger("Start");
                    joiner.PlayAnim("ifu", 0);
                    g = new GameObject();
                    g.name = "ifumiddle_JoinProp";
                    IFU(joiner, host, emoteSpot, g);
                    break;
                case "ifeleft":
                    host.GetComponentsInChildren<Animator>()[1].SetTrigger("Start");
                    joiner.PlayAnim("ifeleft", 0);
                    g = new GameObject();
                    g.name = "ifeleft_JoinProp";
                    IFU(joiner, host, emoteSpot, g);
                    break;
                case "ifuright":
                    host.GetComponentsInChildren<Animator>()[1].SetTrigger("Start");
                    joiner.PlayAnim("ifuright", 0);
                    g = new GameObject();
                    g.name = "ifuright_JoinProp";
                    IFU(joiner, host, emoteSpot, g);
                    break;
                case "HydrolicPressJoinSpot":
                    g = new GameObject();
                    g.name = "hydrolicpress_JoinProp";
                    HydrolicPress(joiner, host, emoteSpot, g);
                    break;
                default:
                    break;
            }
        }
        private void IFU(BoneMapper joiner, BoneMapper host, GameObject emoteSpot, GameObject g)
        {
            //TODO IFU
            //joiner.props.Add(g);
            //g.transform.SetParent(host.transform);
            //g.transform.localPosition = new Vector3(0, .5f, 0);
            //g.transform.localEulerAngles = Vector3.zero;
            //g.transform.localScale = host.transform.localScale;
            //joiner.AssignParentGameObject(g, true, true, true, true, true);
            //emoteSpot.GetComponent<EmoteLocation>().SetEmoterAndHideLocation(joiner);
            //if (joiner.local)
            //{
            //    localBody = NetworkUser.readOnlyLocalPlayersList[0].master?.GetBody();
            //    CharacterCameraParamsData data = new CharacterCameraParamsData();
            //    data.fov = 70f;
            //    data.idealLocalCameraPos = new Vector3(0, 1.5f, -16);
            //    if (!fovHandle.isValid)
            //    {
            //        fovHandle = localBody.GetComponentInChildren<EntityStateMachine>().commonComponents.cameraTargetParams.AddParamsOverride(new CameraTargetParams.CameraParamsOverrideRequest
            //        {
            //            cameraParamsData = data
            //        }, 1f);
            //    }
            //}
        }
        private void HydrolicPress(BoneMapper joiner, BoneMapper host, GameObject emoteSpot, GameObject g)
        {
            joiner.props.Add(g);
            g.transform.SetParent(host.transform);
            g.transform.localPosition = new Vector3(0, .03f, 0);
            g.transform.localEulerAngles = Vector3.zero;
            g.transform.localScale = Vector3.one;
            g.AddComponent<HydrolicPressComponent>().boneMapper = joiner;
            joiner.AssignParentGameObject(g, true, false, true, false, false);
            emoteSpot.GetComponent<EmoteLocation>().SetEmoterAndHideLocation(joiner);
            StartCoroutine(WaitForSecondsThenEndEmote(joiner, 10f, g));
        }
        IEnumerator WaitForSecondsThenEndEmote(BoneMapper mapper, float time, GameObject parent)
        {
            yield return new WaitForSeconds(time);
            if (mapper)
            {
                if (mapper.parentGameObject == parent)
                {
                    mapper.preserveProps = true;
                    mapper.AssignParentGameObject(mapper.parentGameObject, false, false, true, false, false);
                    mapper.preserveParent = true;
                    mapper.preserveProps = true;
                    mapper.PlayAnim("none", 0);
                }
            }
        }
        internal IEnumerator WaitForSecondsThenDeleteGameObject(GameObject obj, float time)
        {
            yield return new WaitForSeconds(time);
            if (obj)
            {
                GameObject.Destroy(obj);
            }
        }
        private void CustomEmotesAPI_emoteSpotJoined_Body(GameObject emoteSpot, BoneMapper joiner, BoneMapper host)
        {
            string emoteSpotName = emoteSpot.name;
            if (emoteSpotName == "StandingHereJoinSpot")
            {
                joiner.PlayAnim("StandingHere", 0);
                GameObject g = new GameObject();
                g.name = "StandingHereProp";
                joiner.props.Add(g);
                g.transform.SetParent(host.transform);
                Vector3 scal = host.transform.lossyScale;
                g.transform.localPosition = new Vector3(0, 0, .75f / scal.z);
                g.transform.localEulerAngles = new Vector3(0, 130, 0);
                g.transform.localScale = new Vector3(.8f, .8f, .8f);
                joiner.AssignParentGameObject(g, true, true, true, true, true);
                joiner.SetAnimationSpeed(2);
                emoteSpot.GetComponent<EmoteLocation>().SetEmoterAndHideLocation(joiner);
            }

            if (emoteSpotName == "DuckThisJoinSpot")
            {
                joiner.PlayAnim("DuckThisOneJoin", 0);

                GameObject g = new GameObject();
                g.name = "DuckThisOneJoinProp";
                joiner.props.Add(g);
                g.transform.SetParent(host.transform);
                Vector3 scal = host.transform.lossyScale;
                g.transform.localPosition = new Vector3(0, 0, 1 / scal.z);
                g.transform.localEulerAngles = new Vector3(0, 180, 0);
                g.transform.localScale = Vector3.one;
                joiner.AssignParentGameObject(g, true, true, true, true, true);


                host.PlayAnim("DuckThisOne", 0);

                g = new GameObject();
                g.name = "DuckThisOneHostProp";
                host.props.Add(g);
                g.transform.localPosition = host.transform.position;
                g.transform.localEulerAngles = host.transform.eulerAngles;
                g.transform.localScale = Vector3.one;
                g.transform.SetParent(host.mapperBodyTransform.parent);
                host.AssignParentGameObject(g, true, true, true, true, false);
            }

            if (emoteSpotName == "Yuki_Nagato")
            {
                joiner.PlayAnim("Yuki_Nagato", 0);
                CustomAnimationClip.syncTimer[joiner.currentClip.syncPos] = CustomAnimationClip.syncTimer[host.currentClip.syncPos];
                CustomAnimationClip.syncPlayerCount[joiner.currentClip.syncPos]++;
                joiner.PlayAnim("Yuki_Nagato", 0);
                CustomAnimationClip.syncPlayerCount[joiner.currentClip.syncPos]--;

                GameObject g = new GameObject();
                g.name = "Yuki_NagatoProp";
                joiner.props.Add(g);
                g.transform.SetParent(host.transform);
                Vector3 scal = host.transform.lossyScale;
                g.transform.localPosition = new Vector3(0, 0, 0);
                g.transform.localEulerAngles = new Vector3(0, 0, 0);
                g.transform.localScale = Vector3.one;
                joiner.AssignParentGameObject(g, true, true, true, true, true);
                emoteSpot.GetComponent<EmoteLocation>().SetEmoterAndHideLocation(joiner);
            }
            if (emoteSpotName == "Mikuru_Asahina")
            {
                joiner.PlayAnim("Mikuru_Asahina", 0);
                CustomAnimationClip.syncTimer[joiner.currentClip.syncPos] = CustomAnimationClip.syncTimer[host.currentClip.syncPos];
                CustomAnimationClip.syncPlayerCount[joiner.currentClip.syncPos]++;
                joiner.PlayAnim("Mikuru_Asahina", 0);
                CustomAnimationClip.syncPlayerCount[joiner.currentClip.syncPos]--;

                GameObject g = new GameObject();
                g.name = "Mikuru_AsahinaProp";
                joiner.props.Add(g);
                g.transform.SetParent(host.transform);
                Vector3 scal = host.transform.lossyScale;
                g.transform.localPosition = new Vector3(0, 0, 0);
                g.transform.localEulerAngles = new Vector3(0, 0, 0);
                g.transform.localScale = Vector3.one;
                joiner.AssignParentGameObject(g, true, true, true, true, true);
                emoteSpot.GetComponent<EmoteLocation>().SetEmoterAndHideLocation(joiner);
            }
            if (emoteSpotName == "MyWorldJoinSpot")
            {
                joiner.PlayAnim("MyWorldJoin", 0);

                GameObject g = new GameObject();
                g.name = "MyWorldJoinProp";
                joiner.props.Add(g);
                g.transform.SetParent(host.transform);
                g.transform.localPosition = new Vector3(0, 0, 0);
                g.transform.localEulerAngles = Vector3.zero;
                g.transform.localScale = Vector3.one;
                joiner.AssignParentGameObject(g, true, true, true, true, true);
                emoteSpot.GetComponent<EmoteLocation>().SetEmoterAndHideLocation(joiner);
            }
            if (emoteSpotName == "VSWORLDJoinSpot")
            {

                joiner.PlayAnim("VSWORLDLEFT", 0);
                GameObject g = new GameObject();
                g.name = "VSWORLDLEFTJoinProp";
                joiner.props.Add(g);
                Vector3 scale = host.transform.parent.localScale;
                host.transform.parent.localScale = Vector3.one;
                g.transform.SetParent(host.transform.parent);
                g.transform.localPosition = new Vector3(-2, 0, 0);
                g.transform.localEulerAngles = new Vector3(90,0,0);
                g.transform.localScale = Vector3.one * (host.scale / joiner.scale);
                //g.transform.SetParent(null);
                host.transform.parent.localScale = scale;
                //g.transform.SetParent(host.transform.parent);
                joiner.AssignParentGameObject(g, true, true, false, false, true);
                emoteSpot.GetComponent<EmoteLocation>().SetEmoterAndHideLocation(joiner);
            }
        }

        int stand = -1;
        List<BoneMapper> punchingMappers = new List<BoneMapper>();
        int prop1 = -1;
        private void CustomEmotesAPI_animChanged(string newAnimation, BoneMapper mapper)
        {
            prop1 = -1;
            try
            {
                if (newAnimation != "none")
                {
                    stand = mapper.currentClip.syncPos;
                }
            }
            catch (System.Exception e)
            {
            }
            if (punchingMappers.Contains(mapper))
            {
                punchingMappers.Remove(mapper);
            }
            if (punchingMappers.Count == 0)
            {
                //TODO audio
                //AkSoundEngine.SetRTPCValue("MetalGear_Vocals", 0);
            }
            if (newAnimation == "StandingHere")
            {
                punchingMappers.Add(mapper);
                //TODO audio
                //AkSoundEngine.SetRTPCValue("MetalGear_Vocals", 1);
            }
            if (newAnimation == "StoodHere")
            {
                GameObject g = new GameObject();
                g.name = "StoodHereProp";
                mapper.props.Add(g);
                g.transform.localPosition = mapper.transform.position;
                g.transform.localEulerAngles = mapper.transform.eulerAngles;
                g.transform.localScale = Vector3.one;
                mapper.AssignParentGameObject(g, false, false, true, true, false);
            }
            if (newAnimation == "Chika")
            {
                prop1 = mapper.props.Count;
                GameObject sex;


                try
                {
                    //TODO CSS
                    //sex = CSS_Loader.LoadDesk();
                }
                catch (System.Exception)
                {
                }
                sex = Assets.Load<GameObject>("@BadAssEmotes_badassemotes:assets/models/desker.prefab");



                mapper.props.Add(GameObject.Instantiate(sex));
                mapper.props[prop1].transform.SetParent(mapper.transform);
                mapper.props[prop1].transform.localEulerAngles = Vector3.zero;
                mapper.props[prop1].transform.localPosition = Vector3.zero;
                mapper.ScaleProps();
            }
            if (newAnimation == "MakeItRainIntro")
            {
                prop1 = mapper.props.Count;
                mapper.props.Add(GameObject.Instantiate(Assets.Load<GameObject>("@BadAssEmotes_badassemotes:assets/Prefabs/money.prefab")));
                mapper.props[prop1].transform.SetParent(mapper.transform);
                mapper.props[prop1].transform.localEulerAngles = Vector3.zero;
                mapper.props[prop1].transform.localPosition = Vector3.zero;
                mapper.ScaleProps();
            }
            if (newAnimation == "DesertRivers" || newAnimation == "Cirno" || newAnimation == "Haruhi" || newAnimation == "GGGG")
            {
                prop1 = mapper.props.Count;
                mapper.props.Add(GameObject.Instantiate(Assets.Load<GameObject>("@BadAssEmotes_badassemotes:assets/Prefabs/desertlight.prefab")));
                mapper.props[prop1].transform.SetParent(mapper.transform);
                mapper.props[prop1].transform.localEulerAngles = Vector3.zero;
                mapper.props[prop1].transform.localPosition = Vector3.zero;
            }
            if (newAnimation == "HondaStep")
            {
                prop1 = mapper.props.Count;
                GameObject myNutz = GameObject.Instantiate(Assets.Load<GameObject>("@BadAssEmotes_badassemotes:assets/Prefabs/hondastuff.prefab"));
                foreach (var item in myNutz.GetComponentsInChildren<ParticleSystem>())
                {
                    item.time = CustomAnimationClip.syncTimer[mapper.currentClip.syncPos];
                }
                Animator a = myNutz.GetComponentInChildren<Animator>();
                //a.Play("MusicSync", -1);
                a.Play("MusicSync", 0, (CustomAnimationClip.syncTimer[mapper.currentClip.syncPos] % a.GetCurrentAnimatorClipInfo(0)[0].clip.length) / a.GetCurrentAnimatorClipInfo(0)[0].clip.length);
                myNutz.transform.SetParent(mapper.transform);
                myNutz.transform.localEulerAngles = Vector3.zero;
                myNutz.transform.localPosition = Vector3.zero;
                mapper.props.Add(myNutz);
            }
            if (newAnimation == "Train")
            {
                prop1 = mapper.props.Count;
                if (CustomAnimationClip.syncPlayerCount[stand] == 1)
                {
                    mapper.props.Add(GameObject.Instantiate(Assets.Load<GameObject>("@BadAssEmotes_badassemotes:assets/Prefabs/train.prefab")));
                }
                else
                {
                    mapper.props.Add(GameObject.Instantiate(Assets.Load<GameObject>("@BadAssEmotes_badassemotes:assets/Prefabs/passenger.prefab")));
                }
                mapper.props[prop1].transform.SetParent(mapper.transform);
                mapper.props[prop1].transform.localEulerAngles = Vector3.zero;
                mapper.props[prop1].transform.localPosition = Vector3.zero;
                mapper.SetAutoWalk(1, true);
                mapper.ScaleProps();
            }
            if (newAnimation == "BimBamBom")
            {
                prop1 = mapper.props.Count;
                mapper.props.Add(GameObject.Instantiate(Assets.Load<GameObject>("@BadAssEmotes_badassemotes:assets/Prefabs/BimBamBom.prefab")));
                mapper.props[prop1].transform.SetParent(mapper.transform);
                mapper.props[prop1].transform.localEulerAngles = Vector3.zero;
                mapper.props[prop1].transform.localPosition = Vector3.zero;
                mapper.ScaleProps();
            }
            if (newAnimation == "Summertime")
            {
                prop1 = mapper.props.Count;
                mapper.props.Add(GameObject.Instantiate(Assets.Load<GameObject>("@BadAssEmotes_badassemotes:Assets/Prefabs/Summermogus.prefab")));
                mapper.props[prop1].transform.SetParent(mapper.transform);
                mapper.props[prop1].transform.localEulerAngles = Vector3.zero;
                mapper.props[prop1].transform.localPosition = Vector3.zero;
                mapper.ScaleProps();
            }
            if (newAnimation == "Float")
            {
                prop1 = mapper.props.Count;
                mapper.props.Add(GameObject.Instantiate(Assets.Load<GameObject>("@BadAssEmotes_badassemotes:assets/Prefabs/FloatLight.prefab")));
                mapper.props[prop1].transform.SetParent(mapper.transform);
                mapper.props[prop1].transform.localEulerAngles = Vector3.zero;
                mapper.props[prop1].transform.localPosition = Vector3.zero;
                mapper.ScaleProps();
            }
            if (newAnimation == "Markiplier")
            {
                prop1 = mapper.props.Count;
                mapper.props.Add(GameObject.Instantiate(Assets.Load<GameObject>("@BadAssEmotes_badassemotes:assets/Prefabs/Amogus.prefab")));
                mapper.props[prop1].transform.SetParent(mapper.transform);
                mapper.props[prop1].transform.localEulerAngles = Vector3.zero;
                mapper.props[prop1].transform.localPosition = Vector3.zero;
                mapper.ScaleProps();
            }
            if (newAnimation == "OfficerEarl")
            {
                mapper.SetAutoWalk(1, false);
            }
            if (newAnimation == "VirtualInsanityIntro")
            {
                mapper.SetAutoWalk(.2f, false);
            }
            if (newAnimation == "MoveIt")
            {
                mapper.SetAutoWalk(.2f, false);
            }
            if (newAnimation == "SpringLoaded")
            {
                mapper.SetAutoWalk(-1, false);
            }
            if (newAnimation == "DuckThisOneIdle")
            {
                GameObject g = new GameObject();
                g.name = "DuckThisOneIdleProp";
                mapper.props.Add(g);
                g.transform.localPosition = mapper.transform.position;
                g.transform.localEulerAngles = mapper.transform.eulerAngles + new Vector3(0, 0, 0);
                g.transform.localScale = Vector3.one;
                g.transform.SetParent(mapper.mapperBodyTransform.parent);
                mapper.AssignParentGameObject(g, true, true, true, true, false);
            }
            if (newAnimation == "FullTilt")
            {
                mapper.SetAutoWalk(1, false);
            }
            if (newAnimation == "Cartwheelin")
            {
                mapper.SetAutoWalk(.75f, false);
            }
            if (newAnimation == "Smoke")
            {
                prop1 = mapper.props.Count;
                mapper.props.Add(GameObject.Instantiate(Assets.Load<GameObject>("@BadAssEmotes_badassemotes:assets/Prefabs/BluntAnimator.prefab")));
                mapper.props[prop1].transform.SetParent(mapper.transform);
                mapper.props[prop1].transform.localEulerAngles = Vector3.zero;
                mapper.props[prop1].transform.localPosition = Vector3.zero;
                mapper.props[prop1].GetComponentInChildren<ParticleSystem>().gravityModifier *= mapper.scale;
                var velocity = mapper.props[prop1].GetComponentInChildren<ParticleSystem>().limitVelocityOverLifetime;
                velocity.dampen *= mapper.scale;
                velocity.limitMultiplier = mapper.scale;
                mapper.ScaleProps();
            }
            if (newAnimation == "Haruhi")
            {
                GameObject g = new GameObject();
                g.name = "HaruhiProp";
                mapper.props.Add(g);
                g.transform.localPosition = mapper.transform.position;
                g.transform.localEulerAngles = mapper.transform.eulerAngles;
                g.transform.localScale = Vector3.one;
                mapper.AssignParentGameObject(g, false, false, true, true, false);
            }
            if (newAnimation == "Security")
            {
                if (mapper != CustomEmotesAPI.localMapper)
                {
                    prop1 = mapper.props.Count;
                    mapper.props.Add(GameObject.Instantiate(Assets.Load<GameObject>("@BadAssEmotes_badassemotes:assets/prefabs/neversee.prefab")));
                    mapper.props[prop1].transform.SetParent(mapper.gameObject.GetComponent<Animator>().GetBoneTransform(HumanBodyBones.Spine));
                    mapper.props[prop1].transform.localEulerAngles = Vector3.zero;
                    mapper.props[prop1].transform.localPosition = Vector3.zero;
                    mapper.ScaleProps();
                }
            }
            if (newAnimation == "IFU Stage")
            {
                //TODO IFU networking
                //if (NetworkServer.active)
                //{
                //    if (stage)
                //    {
                //        NetworkServer.Destroy(stage);
                //    }
                //    stage = CustomEmotesAPI.SpawnWorldProp(stageInt);
                //    stage.transform.SetParent(mapper.transform.parent);
                //    stage.transform.localPosition = new Vector3(0, 0, 0);
                //    stage.transform.SetParent(null);
                //    stage.transform.localPosition += new Vector3(0, .5f, 0);
                //    NetworkServer.Spawn(stage);
                //}
            }
            if (newAnimation == "Hydraulic Press")
            {
                //TODO networking
                //if (NetworkServer.active)
                //{
                //    if (press)
                //    {
                //        NetworkServer.Destroy(press);
                //    }
                //    bool lowes = UnityEngine.Random.Range(0, 15) == 0;
                //    press = CustomEmotesAPI.SpawnWorldProp(pressInt);
                //    press.GetComponent<HydrolicPressMechanism>().lowes = lowes;
                //    press.transform.SetParent(mapper.transform.parent);
                //    press.transform.localPosition = new Vector3(0, 0, 0);
                //    press.transform.SetParent(null);
                //    //press.transform.localPosition += new Vector3(0, .5f, 0);
                //    NetworkServer.Spawn(press);
                //}
            }
            //if (newAnimation == "Sad")
            //{
            //    prop1 = mapper.props.Count;
            //    mapper.props.Add(GameObject.Instantiate(Assets.Load<GameObject>("@BadAssEmotes_badassemotes:assets/models/trombone.prefab")));
            //    mapper.props[prop1].transform.SetParent(mapper.a2.GetBoneTransform(HumanBodyBones.RightHand));
            //    mapper.props[prop1].transform.localEulerAngles = new Vector3(0, 270, 0);
            //    mapper.props[prop1].transform.localPosition = Vector3.zero;
            //    mapper.props[prop1].transform.localScale = Vector3.one;
            //}
        }
        internal void AddAnimation(string AnimClip, AudioClip[] startClips, AudioClip[] loopClips, bool looping, bool dimAudio, bool sync, AnimationClipParams.LockType lockType, string customName, AudioClip[] dmcaStartClips, AudioClip[] dmcaLoopClips, bool DMCA, float audio)
        {
            CustomEmotesAPI.AddCustomAnimation(new AnimationClipParams() { animationClip = new AnimationClip[] { Assets.Load<AnimationClip>($"@ExampleEmotePlugin_badassemotes:assets/badassemotes/{AnimClip}.anim") }, looping = looping, _primaryAudioClips = startClips, _secondaryAudioClips = loopClips, dimWhenClose = dimAudio, syncAnim = sync, syncAudio = sync, lockType = lockType, customName = customName, _primaryDMCAFreeAudioClips = dmcaStartClips, _secondaryDMCAFreeAudioClips = dmcaLoopClips, willGetClaimedByDMCA = DMCA, audioLevel = audio });
        }
        internal void AddAnimation(string AnimClip, string startClip, string loopClip, bool looping, bool dimAudio, bool sync, AnimationClipParams.LockType lockType, string customName, string dmcaStartClip, string dmcaLoopClip, bool DMCA, float audio)
        {
            AudioClip[] startC;
            if (startClip == "")
            {
                startC = null;
            }
            else
            {
                startC = new AudioClip[] { Assets.Load<AudioClip>($"assets/compressedaudio/{startClip}.ogg") };
            }
            AudioClip[] loopC;
            if (loopClip == "")
            {
                loopC = null;
            }
            else
            {
                loopC = new AudioClip[] { Assets.Load<AudioClip>($"assets/compressedaudio/{loopClip}.ogg") };
            }

            AudioClip[] DMCAstartC;
            if (dmcaStartClip == "")
            {
                DMCAstartC = null;
            }
            else
            {
                DMCAstartC = new AudioClip[] { Assets.Load<AudioClip>($"assets/DMCAMusic/{dmcaStartClip}_NNTranscription.ogg") };
            }
            AudioClip[] DMCAloopC;
            if (dmcaLoopClip == "")
            {
                DMCAloopC = null;
            }
            else
            {
                DMCAloopC = new AudioClip[] { Assets.Load<AudioClip>($"assets/DMCAMusic/{dmcaLoopClip}_NNTranscription.ogg") };
            }
            CustomEmotesAPI.AddCustomAnimation(new AnimationClipParams() { animationClip = new AnimationClip[] { Assets.Load<AnimationClip>($"@ExampleEmotePlugin_badassemotes:assets/badassemotes/{AnimClip}.anim") }, looping = looping, _primaryAudioClips = startC, _secondaryAudioClips = loopC, dimWhenClose = dimAudio, syncAnim = sync, syncAudio = sync, lockType = lockType, customName = customName, _primaryDMCAFreeAudioClips = DMCAstartC, _secondaryDMCAFreeAudioClips = DMCAloopC, willGetClaimedByDMCA = DMCA, audioLevel = audio });
        }
        internal void AddAnimation(string AnimClip, string AnimClip2, string startClip, string loopClip, bool looping, bool dimAudio, bool sync, AnimationClipParams.LockType lockType, string customName, string dmcaStartClip, string dmcaLoopClip, bool DMCA, float audio)
        {
            AudioClip[] startC;
            if (startClip == "")
            {
                startC = null;
            }
            else
            {
                startC = new AudioClip[] { Assets.Load<AudioClip>($"assets/compressedaudio/{startClip}.ogg") };
            }
            AudioClip[] loopC;
            if (loopClip == "")
            {
                loopC = null;
            }
            else
            {
                loopC = new AudioClip[] { Assets.Load<AudioClip>($"assets/compressedaudio/{loopClip}.ogg") };
            }

            AudioClip[] DMCAstartC;
            if (dmcaStartClip == "")
            {
                DMCAstartC = null;
            }
            else
            {
                DMCAstartC = new AudioClip[] { Assets.Load<AudioClip>($"assets/DMCAMusic/{dmcaStartClip}_NNTranscription.ogg") };
            }
            AudioClip[] DMCAloopC;
            if (dmcaLoopClip == "")
            {
                DMCAloopC = null;
            }
            else
            {
                DMCAloopC = new AudioClip[] { Assets.Load<AudioClip>($"assets/DMCAMusic/{dmcaLoopClip}_NNTranscription.ogg") };
            }
            CustomEmotesAPI.AddCustomAnimation(new AnimationClipParams() { animationClip = new AnimationClip[] { Assets.Load<AnimationClip>($"@ExampleEmotePlugin_badassemotes:assets/badassemotes/{AnimClip}.anim") }, secondaryAnimation = new AnimationClip[] { Assets.Load<AnimationClip>($"@ExampleEmotePlugin_badassemotes:assets/badassemotes/{AnimClip2}.anim") }, looping = looping, _primaryAudioClips = startC, _secondaryAudioClips = loopC, dimWhenClose = dimAudio, syncAnim = sync, syncAudio = sync, lockType = lockType, customName = customName, _primaryDMCAFreeAudioClips = DMCAstartC, _secondaryDMCAFreeAudioClips = DMCAloopC, willGetClaimedByDMCA = DMCA, audioLevel = audio });
        }
        internal void AddAnimation(string AnimClip, string[] startClip, bool looping, bool dimaudio, bool sync, AnimationClipParams.LockType lockType, string customName, string[] dmcaStartClips, bool DMCA, float audio)
        {
            List<AudioClip> startC = new List<AudioClip>();
            foreach (var item in startClip)
            {

                startC.Add(Assets.Load<AudioClip>($"assets/compressedaudio/{item}.ogg"));
            }

            List<AudioClip> DMCAstartC = new List<AudioClip>();
            foreach (var item in dmcaStartClips)
            {
                if (item == "")
                {
                    DMCAstartC.Add(null);
                }
                else
                {
                    DMCAstartC.Add(Assets.Load<AudioClip>($"assets/DMCAMusic/{item}_NNTranscription.ogg"));
                }
            }
            CustomEmotesAPI.AddCustomAnimation(new AnimationClipParams() { animationClip = new AnimationClip[] { Assets.Load<AnimationClip>($"@ExampleEmotePlugin_badassemotes:assets/badassemotes/{AnimClip}.anim") }, looping = looping, _primaryAudioClips = startC.ToArray(), _secondaryAudioClips = null, dimWhenClose = dimaudio, syncAnim = sync, syncAudio = sync, lockType = lockType, customName = customName, _primaryDMCAFreeAudioClips = DMCAstartC.ToArray(), willGetClaimedByDMCA = DMCA, audioLevel = audio });
        }
        internal void AddStartAndJoinAnim(string[] AnimClip, string startClip, bool looping, bool dimaudio, bool sync, AnimationClipParams.LockType lockType, string customName, string dmcaStartClip, bool DMCA, float audio)
        {
            AudioClip[] startC;
            if (startClip == "")
            {
                startC = null;
            }
            else
            {
                startC = new AudioClip[] { Assets.Load<AudioClip>($"assets/compressedaudio/{startClip}.ogg") };
            }

            AudioClip[] DMCAstartC;
            if (dmcaStartClip == "")
            {
                DMCAstartC = null;
            }
            else
            {
                DMCAstartC = new AudioClip[] { Assets.Load<AudioClip>($"assets/DMCAMusic/{dmcaStartClip}_NNTranscription.ogg") };
            }
            List<AnimationClip> nuts = new List<AnimationClip>();
            foreach (var item in AnimClip)
            {
                nuts.Add(Assets.Load<AnimationClip>($"@ExampleEmotePlugin_badassemotes:assets/badassemotes/{item}.anim"));
            }
            CustomEmotesAPI.AddCustomAnimation(new AnimationClipParams() { animationClip = nuts.ToArray(), looping = looping, _primaryAudioClips = startC, _secondaryAudioClips = null, dimWhenClose = dimaudio, syncAnim = sync, syncAudio = sync, startPref = 0, joinPref = 1, lockType = lockType, customName = customName, _primaryDMCAFreeAudioClips = DMCAstartC, willGetClaimedByDMCA = DMCA, audioLevel = audio });
        }
        internal void AddStartAndJoinAnim(string[] AnimClip, string startClip, string[] AnimClip2, bool looping, bool dimaudio, bool sync, AnimationClipParams.LockType lockType, string customName, string dmcaStartClip, bool DMCA, float audio)
        {
            AudioClip[] startC;
            if (startClip == "")
            {
                startC = null;
            }
            else
            {
                startC = new AudioClip[] { Assets.Load<AudioClip>($"assets/compressedaudio/{startClip}.ogg") };
            }
            AudioClip[] DMCAstartC;
            if (dmcaStartClip == "")
            {
                DMCAstartC = null;
            }
            else
            {
                DMCAstartC = new AudioClip[] { Assets.Load<AudioClip>($"assets/compressedaudio/{dmcaStartClip}.ogg") };
            }

            List<AnimationClip> nuts = new List<AnimationClip>();
            foreach (var item in AnimClip)
            {
                nuts.Add(Assets.Load<AnimationClip>($"@ExampleEmotePlugin_badassemotes:assets/DMCAMusic/{item}_NNTranscription.anim"));
            }
            List<AnimationClip> nuts2 = new List<AnimationClip>();
            foreach (var item in AnimClip2)
            {
                nuts2.Add(Assets.Load<AnimationClip>($"@ExampleEmotePlugin_badassemotes:assets/DMCAMusic/{item}_NNTranscription.anim"));
            }
            CustomEmotesAPI.AddCustomAnimation(new AnimationClipParams() { animationClip = nuts.ToArray(), secondaryAnimation = nuts2.ToArray(), looping = looping, _primaryAudioClips = startC, dimWhenClose = dimaudio, syncAnim = sync, syncAudio = sync, startPref = 0, joinPref = 1, lockType = lockType, customName = customName, _primaryDMCAFreeAudioClips = DMCAstartC, willGetClaimedByDMCA = DMCA, audioLevel = audio });
        }

    }
}
