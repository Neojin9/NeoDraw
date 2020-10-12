using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;

namespace NeoDraw.UI {

    public class Menu {

        public static bool modOptionChanged = false;
        public static string currentPage = "";
        public static string gotoAfterUpdate = null;
        public static string lastPage = "";
        public static string mouseOverText = "";
        public static bool drawOldMenu = false;
        public static int buttonLock = -1;
        public static int setButton = -1;
        public static int setButton2 = -1;
        public static int skip = 0;
        public static int skip2 = 0;
        public static List<string> loadLog = new List<string>();
        public static bool notMod = false;
        public static int optionsPage = 0;
        public static int optionsLastPage = 0;
        public static int progressTotalCount = 0;
        private static float _progressTotal = -1f;
        private static float _progressProcess = -1f;

        public static float progressTotal {
            get { return _progressTotal; }
            set {
                _progressTotal = Math.Min(Math.Max(value, -1f), 1f);
                if (value < 0f)
                    progressTotalCount = 0;
            }
        }

        public static float progressProcess {
            get { return _progressProcess; }
            set { _progressProcess = Math.Min(Math.Max(value, -1f), 1f); }
        }

        public static void MoveTo(string page, bool silent = false) {
            
            if (Main.dedServ)
                return;
            
            if (!silent)
                Main.PlaySound(SoundID.MenuOpen);
            
            setButton = -1;
            buttonLock = -2;
            
            currentPage = page;
            
        }

    }

}
