using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using NeoDraw.Core;
using NeoDraw.UI;
using Terraria;
using Terraria.GameInput;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace NeoDraw {

    public class NeoPlayer : ModPlayer {

        #region Variables

        #region Private Variables

        private const int CameraSpeed  = 16;
        private const int CameraSpeedX = 4;

        private static bool CutCopyPasteNotPressed;
        private static bool DebugRegenNotPressed;
        private static bool DrawModeToggleNotPressed;
        private static bool HomeNotPressed;
        private static bool MapToggleNotPressed;
        private static bool PauseNotPressed;
        private static bool SearchStartNotPressed;
        private static bool TeleportNotPressed;
        private static bool UndoNotPressed;

        private static Vector2 CurrentCameraPosition = new Vector2(-1, -1);
        private static Vector2 LastCameraPosition    = new Vector2(-1, -1);

        #endregion

        #region Public Variables

        public static bool Moving;

        #endregion

        #endregion

        #region Override Functions

        public override void ModifyScreenPosition() {

            if (!NeoDraw.DrawMode)
                return;

            MakeImmune();
            UpdateCameraPosition();

        }
        
        public override void ProcessTriggers(TriggersSet triggersSet) {

            ProcessControls();

            if (!NeoDraw.DrawMode)
                return;

            Player p = Main.LocalPlayer;

            p.controlUp        = false;
            p.controlDown      = false;
            p.controlLeft      = false;
            p.controlRight     = false;
            p.controlJump      = false;
            p.controlHook      = false;
            p.controlTorch     = false;
            p.controlSmart     = false;
            p.controlMount     = false;
            p.controlQuickHeal = false;
            p.controlQuickMana = false;
            p.controlInv       = false;
            p.controlThrow     = false;
            p.mapZoomIn        = false;
            p.mapZoomOut       = false;
            p.mapAlphaUp       = false;
            p.mapAlphaDown     = false;
            p.mapFullScreen    = false;
            p.mapStyle         = false;
            p.controlUseItem   = false;
            p.controlUseTile   = false;
            p.controlInv       = false;
            p.controlMap       = false;

        }

        public override void PostUpdateBuffs() {

            if (!NeoDraw.DrawMode)
                return;

            Main.LocalPlayer.InfoAccMechShowWires = true;

        }

        public override void UpdateAutopause() {

            if (!NeoDraw.DrawMode)
                return;

            if (!Main.gamePaused)
                return;

            ProcessControls();

        }

        #endregion

        #region Public Functions

        public static void CenterCameraOnPosition(float x, float y) {

            CurrentCameraPosition = new Vector2(x, y);
            LastCameraPosition    = new Vector2(x, y);

        }

        public static void ResetCameraPositionVector() {

            CurrentCameraPosition = new Vector2(-1, -1);
            LastCameraPosition    = new Vector2(-1, -1);

        }

        public static void SetLastCameraPosition(Vector2 newPosition) => LastCameraPosition = newPosition;

        #endregion

        #region Private Functions

        private void MakeImmune() {

            player.immune = player.noFallDmg = true;

            player.poisoned = player.venom = player.onFire = player.onFire2 = player.onFrostBurn = player.burned = player.suffocating = false;

            player.statLife = player.statLifeMax;

            player.immuneAlpha = player.lastTileRangeX = player.lastTileRangeY = 0;

            player.breath = player.breathMax;

            if (player.lifeRegen < 0)
                player.lifeRegen = 0;

        }
        
        private void ProcessControls() {

            if (!GetInstance<NeoConfigServer>().Activated || !GetInstance<NeoConfigServer>().Backuped)
                return;

            if (NeoDraw.ToggleDrawMode.JustPressed) {

                if (DrawModeToggleNotPressed) {

                    if (NeoDraw.DrawMode) {
                        NeoDraw.ExitDrawMode();
                    }
                    else {
                        NeoDraw.OpenDrawMode();
                    }

                }

                DrawModeToggleNotPressed = false;

            } else { DrawModeToggleNotPressed = true; }

            if (!NeoDraw.DrawMode)
                return;

            if (Keys.F6.Pressed() && Main.keyState.PressingAlt() && Main.keyState.PressingCtrl() && Main.keyState.PressingShift()) {
                
                if (DebugRegenNotPressed) {

                    DrawInterface.ShowDebugRegenDialog = true;

                }

                DebugRegenNotPressed = false;
                
            } else { DebugRegenNotPressed = true; }

            if (Main.hasFocus && Main.mapFullscreen) {

                float speed = Main.keyState.PressingShift() ? CameraSpeedX : 1f;

                if (NeoDraw.CameraUp.Current) {
                    Main.mapFullscreenPos.Y -= (16f / Main.mapFullscreenScale) * speed;
                }
                if (NeoDraw.CameraDown.Current) {
                    Main.mapFullscreenPos.Y += (16f / Main.mapFullscreenScale) * speed;
                }
                if (NeoDraw.CameraLeft.Current) {
                    Main.mapFullscreenPos.X -= (16f / Main.mapFullscreenScale) * speed;
                }
                if (NeoDraw.CameraRight.Current) {
                    Main.mapFullscreenPos.X += (16f / Main.mapFullscreenScale) * speed;
                }

            }

            if (DrawInterface.Searching)
                return;

            if (Keys.Enter.Pressed()) {

                if (SearchStartNotPressed)
                    DrawInterface.StartSearchBarTyping();

                SearchStartNotPressed = false;

            } else { SearchStartNotPressed = true; }

            if (NeoDraw.FindPlayer.JustPressed) {

                if (HomeNotPressed) {

                    float cameraX;
                    float cameraY;

                    if (Main.keyState.PressingShift()) {

                        cameraX = Main.spawnTileX * 16 - Main.screenWidth / 2f;
                        cameraY = Main.spawnTileY * 16 - Main.screenHeight / 2f;

                    }
                    else {

                        cameraX = Main.LocalPlayer.position.X - Main.screenWidth / 2f;
                        cameraY = Main.LocalPlayer.position.Y - Main.screenHeight / 2f;

                    }

                    CenterCameraOnPosition(cameraX, cameraY);

                }

                HomeNotPressed = false;

            } else { HomeNotPressed = true; }

            if (NeoDraw.TeleportPlayer.JustPressed) {

                if (TeleportNotPressed)
                    DrawInterface.TryTeleport();

                TeleportNotPressed = false;

            } else { TeleportNotPressed = true; }

            if (NeoDraw.PauseGame.JustPressed) {

                if (PauseNotPressed) {

                    Main.autoPause = !Main.autoPause;
                    DrawInterface.SetStatusBarTempMessage("Game " + (Main.autoPause ? "Paused" : "Unpaused"), 50);

                }

                PauseNotPressed = false;

            } else { PauseNotPressed = true; }

            if (Main.LocalPlayer.mapFullScreen) {

                if (MapToggleNotPressed) {

                    if (Main.keyState.PressingCtrl()) {

                        if (Main.keyState.PressingShift() && Main.keyState.PressingAlt()) {
                            DrawInterface.ShowMapResetDialog = true;
                        }
                        else {
                            DrawInterface.ShowMinimap = !DrawInterface.ShowMinimap;
                        }

                    }
                    else {

                        if (Main.mapFullscreen) {

                            Main.PlaySound(11);
                            Main.mapFullscreen = false;

                        }
                        else {

                            Main.resetMapFull = true;
                            Main.mapFullscreen = true;

                        }

                    }

                }

                MapToggleNotPressed = false;

            } else { MapToggleNotPressed = true; }

            if (NeoDraw.UndoRedo.JustPressed) {

                if (UndoNotPressed) {

                    if (Main.keyState.PressingCtrl()) {

                        if (Main.keyState.PressingShift()) {

                            if (NeoDraw.UndoManager.RedoCount > 0)
                                NeoDraw.UndoManager.Redo();

                        }
                        else {

                            if (NeoDraw.UndoManager.HistoryCount > 0)
                                NeoDraw.UndoManager.Undo();

                        }

                    }

                }

                UndoNotPressed = false;

            } else { UndoNotPressed = true; }

            if (Main.keyState.PressingCtrl()) {

                if (CutCopyPasteNotPressed) {

                    if (Keys.C.Pressed()) {
                        DrawInterface.CopySelection();
                        CutCopyPasteNotPressed = false;
                    }
                    else if (Keys.X.Pressed()) {
                        DrawInterface.CopySelection(true);
                        CutCopyPasteNotPressed = false;
                    }
                    else if (Keys.V.Pressed()) {
                        DrawInterface.PasteSelection();
                        CutCopyPasteNotPressed = false;
                    }

                }

                float zoomBy = PlayerInput.ScrollWheelDelta / 120;

                zoomBy /= 100f;

                Main.GameZoomTarget = Utils.Clamp(Main.GameZoomTarget + zoomBy, 1f, 2f);

            } else { CutCopyPasteNotPressed = true; }

        }

        private void UpdateCameraPosition() {

            CurrentCameraPosition = LastCameraPosition;

            Moving = false;

            if (!DrawInterface.Searching && !Main.mapFullscreen) {

                float speed = Main.keyState.PressingShift() ? CameraSpeed * CameraSpeedX : CameraSpeed;

                if (NeoDraw.CameraLeft.Current)
                    CurrentCameraPosition.X -= speed;

                if (NeoDraw.CameraRight.Current)
                    CurrentCameraPosition.X += speed;

                if (NeoDraw.CameraUp.Current)
                    CurrentCameraPosition.Y -= speed;

                if (NeoDraw.CameraDown.Current)
                    CurrentCameraPosition.Y += speed;

                if (CurrentCameraPosition.X < Main.leftWorld + 640 + 16)
                    CurrentCameraPosition.X = Main.leftWorld + 640 + 16;

                if (CurrentCameraPosition.Y < Main.topWorld + 640 + 16)
                    CurrentCameraPosition.Y = Main.topWorld + 640 + 16;

                if (CurrentCameraPosition.X + Main.screenWidth > Main.rightWorld - 640 - 32)
                    CurrentCameraPosition.X = Main.rightWorld - Main.screenWidth - 640 - 32;

                if (CurrentCameraPosition.Y + Main.screenHeight > Main.bottomWorld - 640 - 32)
                    CurrentCameraPosition.Y = Main.bottomWorld - Main.screenHeight - 640 - 32;

            }


            if (LastCameraPosition != CurrentCameraPosition)
                Moving = true;

            Main.screenPosition = CurrentCameraPosition;

            LastCameraPosition = CurrentCameraPosition;

        } // TODO: Move Key Checks to ProcessControls

        #endregion

    }

}
