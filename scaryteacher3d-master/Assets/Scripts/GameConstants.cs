using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameConstants : MonoBehaviour
{
    public static class InGameConstants
    {
        public enum TeacherAnimationNames
        {
            Yelling,
            Walking,
            RightTurn,
            Idle,
            SittingIdle,
            Running,
            Goofy,
            Terrified,
            TestPaper,
            Sliped,
            Crying,
            StrongYelling,
            LeftTurn,
            Clapping,
            None
        }

        public enum StudentAnimationNames
        {
            SadIdle,
            Run,
            Walking,
            PushUp,
            None
        }

        public enum GameCharacters
        {
            Teacher,
            SchoolBoy,
            SchoolGirl,
            Baby,
            None
        }

        public enum LevelProps
        {
            Key,
            Matchbox,
            ElectricityBill,
            HouseMainDoor,
            BabyCot,
            Purse,
            Cupboard,
            FireCrackers,
            CrackerBoundary,
            Lighter,
            TestPaperDestination,
            TestPaper,
            OilBottle,
            OilBottleBoundary,
            Phone,
            TeacherRoomDoor,
            Needle,
            Tyre,
            PurseBoundary,
            WaterTap,
            Pipe,
            //PipeStartingBoundary,
            PipeEndBoundary,
            TeacherBoundary,
            None
        }

        public enum GameModes
        {
            StoryMode,
            SurvivalMode,
            StreetChase,
            None
        }

        public enum BabyAnimation
        {
            Sitting,
            Sleeping,
            Crying,
            None
        }

        public enum SpecialAudioClips
        {
            BabyCrying,
            TeacherAngry,
            TeacherCrying
        }


        public const string LEVEL_IDENTIFIER = "level_";

        public const string PRIVACY_POLICY_SHOWN = "privacy_policy_shown";

        public const string ALL_LEVELS_UNLOCKED = "all_levels_unlocked";

        public const int totalLevelsInStoryMode = 11;

        public static int selectedLevel = -1;
    }
}
