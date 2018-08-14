﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SkeletonGameManager.Resources.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "15.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("SkeletonGameManager.Resources.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to This file contains global configuration for the game. Paths, Dmd, Audio, Modes..
        /// </summary>
        public static string Cfg_ConfigYaml {
            get {
                return ResourceManager.GetString("Cfg_ConfigYaml", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SkeletonGame path.
        /// </summary>
        public static string GameFolder {
            get {
                return ResourceManager.GetString("GameFolder", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Todo.
        /// </summary>
        public static string Tip_Attract {
            get {
                return ResourceManager.GetString("Tip_Attract", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Disabling will load into memory. (Only compatible with music). If using lots of music it&apos;s probably best to disable streaming load..
        /// </summary>
        public static string Tip_AudioStreamingLoad {
            get {
                return ResourceManager.GetString("Tip_AudioStreamingLoad", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Enables/Disables the BallSearch Mode..
        /// </summary>
        public static string Tip_BallSearch {
            get {
                return ResourceManager.GetString("Tip_BallSearch", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to How long to wait for a ball search to start..
        /// </summary>
        public static string Tip_BallSearchDelay {
            get {
                return ResourceManager.GetString("Tip_BallSearchDelay", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Enables/Disables the Bonus mode..
        /// </summary>
        public static string Tip_BonusTally {
            get {
                return ResourceManager.GetString("Tip_BonusTally", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Coils tagged for ballsearch are fired when the ball has not been seen or at the start of the game when all balls can&apos;t be found so these coils are coils that help return the ball to the trough. NOTE: trough should NOT be tagged for ballsearch since firing the
        ///        # trough would not help fill the trough...Add the tag autoPlunger to a coil to support autoplunging on ball save..
        /// </summary>
        public static string Tip_CoilBallSearch {
            get {
                return ResourceManager.GetString("Tip_CoilBallSearch", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to This view allows you to configure all the coils/flashers in a machine. Coils in the table must have a name to be exported to yaml and will be skipped if NOT USED. The SolenoidType isn&apos;t needed for pyprocgame but just as extra options for reference..
        /// </summary>
        public static string Tip_CoilFlashTable {
            get {
                return ResourceManager.GetString("Tip_CoilFlashTable", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Shows a border around the window. (F11 toggles window border and F12 will save position when window is running)..
        /// </summary>
        public static string Tip_DmdBordered {
            get {
                return ResourceManager.GetString("Tip_DmdBordered", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Flips the display. Options are 0, 1, 2, 3..
        /// </summary>
        public static string Tip_DmdFlipped {
            get {
                return ResourceManager.GetString("Tip_DmdFlipped", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The displays framerate..
        /// </summary>
        public static string Tip_DmdFramerate {
            get {
                return ResourceManager.GetString("Tip_DmdFramerate", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Full-screen mode scales the contents to fit the full display; may not look right..
        /// </summary>
        public static string Tip_DmdFullScreen {
            get {
                return ResourceManager.GetString("Tip_DmdFullScreen", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Where to find the dmdgrid32x32.png file..
        /// </summary>
        public static string Tip_DmdGrid {
            get {
                return ResourceManager.GetString("Tip_DmdGrid", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Raises the window above every other window. Good for Visual Pinball or always having the display in view..
        /// </summary>
        public static string Tip_DmdOnTop {
            get {
                return ResourceManager.GetString("Tip_DmdOnTop", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Sets the displays position..
        /// </summary>
        public static string Tip_DmdPosition {
            get {
                return ResourceManager.GetString("Tip_DmdPosition", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Display resolution. Shouldn&apos;t be changed after doing work to a game..
        /// </summary>
        public static string Tip_DmdResolution {
            get {
                return ResourceManager.GetString("Tip_DmdResolution", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The display scale is the multiplier per dot.  At 5 each dot is 5x5 pixels..
        /// </summary>
        public static string Tip_DmdScale {
            get {
                return ResourceManager.GetString("Tip_DmdScale", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Drop files to copy..
        /// </summary>
        public static string Tip_DropFilesInfo {
            get {
                return ResourceManager.GetString("Tip_DropFilesInfo", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Drag &amp; Drop Lampshows and RGBShows files and to copy to lampshow directory. Drag &amp; Drop from the available files to each collection, lampshow and rgbshow..
        /// </summary>
        public static string Tip_DropLampshows {
            get {
                return ResourceManager.GetString("Tip_DropLampshows", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Enabled game recording plaback. This will set the FakePinProc class to use the Playback version and set back when game is closed. The selected playback in the list will be copied to the main game directory as playback.txt..
        /// </summary>
        public static string Tip_EnablePlayback {
            get {
                return ResourceManager.GetString("Tip_EnablePlayback", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Enabled game recording. This will set the skeletons base class to BasicGameRecordable and set it back providing the game window is closed before the UI. Recordings are saved to the recordings directory..
        /// </summary>
        public static string Tip_EnableRecording {
            get {
                return ResourceManager.GetString("Tip_EnableRecording", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Exports all the lamps into a compatible rampantslug LampshowUI project saving time on initial setup. *You can load and move into place..
        /// </summary>
        public static string Tip_ExportLampshowUI {
            get {
                return ResourceManager.GetString("Tip_ExportLampshowUI", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Exports a python file that contains active switch method callbacks. All switches in the machine are exported except for the trough and dedicated switches..
        /// </summary>
        public static string Tip_ExportPythonSwitchCallbacks {
            get {
                return ResourceManager.GetString("Tip_ExportPythonSwitchCallbacks", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Double clicking opens with default process..
        /// </summary>
        public static string Tip_FileOptionInfo {
            get {
                return ResourceManager.GetString("Tip_FileOptionInfo", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Launches the game with your default python install..
        /// </summary>
        public static string Tip_GameLaunchInfo {
            get {
                return ResourceManager.GetString("Tip_GameLaunchInfo", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to This view allows you to configure all the lamps in a machine. Lamps must have a name to be exported to yaml and will be skipped if NOT USED. Some tags are needed for Skeleton game. Noteably; shoot_again (Used for ball save)..
        /// </summary>
        public static string Tip_LampMatrix {
            get {
                return ResourceManager.GetString("Tip_LampMatrix", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Reverses the selected Lamshow..
        /// </summary>
        public static string Tip_LampshowReverse {
            get {
                return ResourceManager.GetString("Tip_LampshowReverse", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Launches the game with the options set..
        /// </summary>
        public static string Tip_LaunchRecording {
            get {
                return ResourceManager.GetString("Tip_LaunchRecording", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to View SGM logs..
        /// </summary>
        public static string Tip_LogSgm {
            get {
                return ResourceManager.GetString("Tip_LogSgm", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to View logs from gamefolder/logs. Logs are produced when launched from SGM..
        /// </summary>
        public static string Tip_LogSgmGame {
            get {
                return ResourceManager.GetString("Tip_LogSgmGame", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to This file contains all the configuration for a machine..
        /// </summary>
        public static string Tip_MachineYaml {
            get {
                return ResourceManager.GetString("Tip_MachineYaml", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Creates and setups a new game..
        /// </summary>
        public static string Tip_NewGame {
            get {
                return ResourceManager.GetString("Tip_NewGame", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Opens the current directory..
        /// </summary>
        public static string Tip_OpenDir {
            get {
                return ResourceManager.GetString("Tip_OpenDir", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Loads a compatible SkeletonGame folder..
        /// </summary>
        public static string Tip_OpenGame {
            get {
                return ResourceManager.GetString("Tip_OpenGame", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Enables an OSC server which listens for switches pushed from a UI. eg SwitchMatrix Gui..
        /// </summary>
        public static string Tip_OSC {
            get {
                return ResourceManager.GetString("Tip_OSC", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Asset manager path for display files. Images, movie, .dmd..
        /// </summary>
        public static string Tip_PathsDmd {
            get {
                return ResourceManager.GetString("Tip_PathsDmd", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Asset manager path for fonts. .ttf..
        /// </summary>
        public static string Tip_PathsHdFonts {
            get {
                return ResourceManager.GetString("Tip_PathsHdFonts", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The path to switchMatrixClient.py. Default install C:\P-ROC\GUITool.
        /// </summary>
        public static string Tip_PathsOscUi {
            get {
                return ResourceManager.GetString("Tip_PathsOscUi", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The path to the playfield..
        /// </summary>
        public static string Tip_PathsPlayfield {
            get {
                return ResourceManager.GetString("Tip_PathsPlayfield", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The display uses SDL2. The Dlls path is the needed SDL2 Dlls..
        /// </summary>
        public static string Tip_PathsSdlPath {
            get {
                return ResourceManager.GetString("Tip_PathsSdlPath", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Asset manager for the sfx dir..
        /// </summary>
        public static string Tip_PathsSfx {
            get {
                return ResourceManager.GetString("Tip_PathsSfx", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Asset manager path for the parent Sound folder..
        /// </summary>
        public static string Tip_PathsSound {
            get {
                return ResourceManager.GetString("Tip_PathsSound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The path to the GUI tools layout switch / lamp file..
        /// </summary>
        public static string Tip_PathsUiLayout {
            get {
                return ResourceManager.GetString("Tip_PathsUiLayout", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Asset manager path for the parent Voice folder..
        /// </summary>
        public static string Tip_PathsVoice {
            get {
                return ResourceManager.GetString("Tip_PathsVoice", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The is where the vp_game_map.yaml can be found. This is more of a globally used path, not per game..
        /// </summary>
        public static string Tip_PathsVpGameMap {
            get {
                return ResourceManager.GetString("Tip_PathsVpGameMap", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Enables a profile menu players where players can create &amp;amp; select players..
        /// </summary>
        public static string Tip_Profiles {
            get {
                return ResourceManager.GetString("Tip_Profiles", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Playback a Recording..
        /// </summary>
        public static string Tip_RecordingPlaybackMenu {
            get {
                return ResourceManager.GetString("Tip_RecordingPlaybackMenu", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Reloads game config from disk..
        /// </summary>
        public static string Tip_Reload {
            get {
                return ResourceManager.GetString("Tip_Reload", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Saves the asset_list.yaml..
        /// </summary>
        public static string Tip_SaveAssetsFile {
            get {
                return ResourceManager.GetString("Tip_SaveAssetsFile", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Overwrites the config.yaml in the current game folder..
        /// </summary>
        public static string Tip_SaveConfigGame {
            get {
                return ResourceManager.GetString("Tip_SaveConfigGame", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Overwrites the machine.yaml in the games config folder..
        /// </summary>
        public static string Tip_SaveConfigMachine {
            get {
                return ResourceManager.GetString("Tip_SaveConfigMachine", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Overwrites the new_score_display.yaml in the games config folder..
        /// </summary>
        public static string Tip_SaveScoreDisplayConfig {
            get {
                return ResourceManager.GetString("Tip_SaveScoreDisplayConfig", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Saves the sequence file that&apos;s in view..
        /// </summary>
        public static string Tip_SaveSequences {
            get {
                return ResourceManager.GetString("Tip_SaveSequences", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to An animation layer, no text..
        /// </summary>
        public static string Tip_SeqAnimation {
            get {
                return ResourceManager.GetString("Tip_SeqAnimation", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to A multi-line text layer and animation Combo..
        /// </summary>
        public static string Tip_SeqCombo {
            get {
                return ResourceManager.GetString("Tip_SeqCombo", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to A static animation with a moving image(credit scroll) that scrolls over animation..
        /// </summary>
        public static string Tip_SeqCredits {
            get {
                return ResourceManager.GetString("Tip_SeqCredits", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Groups multiple display layers together..
        /// </summary>
        public static string Tip_SeqGroupLayer {
            get {
                return ResourceManager.GetString("Tip_SeqGroupLayer", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Displays the high scores..
        /// </summary>
        public static string Tip_SeqHighScores {
            get {
                return ResourceManager.GetString("Tip_SeqHighScores", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Displays the last games scores..
        /// </summary>
        public static string Tip_SeqLastScores {
            get {
                return ResourceManager.GetString("Tip_SeqLastScores", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Has a Bold # and Normal [ font. eg:
        ///        # Left, Right #, # Center #
        ///        [ Left, Right ], [ Center ]..
        /// </summary>
        public static string Tip_SeqMarkupLayer {
            get {
                return ResourceManager.GetString("Tip_SeqMarkupLayer", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Moves an animation or text..
        /// </summary>
        public static string Tip_SeqMoveLayer {
            get {
                return ResourceManager.GetString("Tip_SeqMoveLayer", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ToDo.
        /// </summary>
        public static string Tip_SeqPanningLayer {
            get {
                return ResourceManager.GetString("Tip_SeqPanningLayer", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Displays particle emitters in a layer..
        /// </summary>
        public static string Tip_SeqParticleLayer {
            get {
                return ResourceManager.GetString("Tip_SeqParticleLayer", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Create a list of sentences to display at random when this layer is used..
        /// </summary>
        public static string Tip_SeqRandomText {
            get {
                return ResourceManager.GetString("Tip_SeqRandomText", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Runs a script of text lists. Duration is how long each text list will be displayed. If an animation is set to play with the layers and the duration of the animation is longer than the text script, then the duration is set to let the whole animtion finish..
        /// </summary>
        public static string Tip_SeqScriptedText {
            get {
                return ResourceManager.GetString("Tip_SeqScriptedText", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to A text layer with no animation..
        /// </summary>
        public static string Tip_SeqTextLayer {
            get {
                return ResourceManager.GetString("Tip_SeqTextLayer", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Enables/Disables the Service Mode Menu..
        /// </summary>
        public static string Tip_ServiceMenu {
            get {
                return ResourceManager.GetString("Tip_ServiceMenu", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to This view allows you configure the switches. Switches must have a name to be exported to yaml and will be skipped if NOT USED. Don&apos;t use spaces in names, use the label. Some tags are needed for Skeleton game. Noteably; shooter, trough, early_save..
        /// </summary>
        public static string Tip_SwitchMatrix {
            get {
                return ResourceManager.GetString("Tip_SwitchMatrix", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Launch the OSC Switch Matrix Tool. Paths are configured in config.yaml..
        /// </summary>
        public static string Tip_SwitchMatrixLaunch {
            get {
                return ResourceManager.GetString("Tip_SwitchMatrixLaunch", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Enables/Disables the Tilt mode..
        /// </summary>
        public static string Tip_TiltConfig {
            get {
                return ResourceManager.GetString("Tip_TiltConfig", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Enables a mode that displays trophys when complete. See the trophy_default_data..
        /// </summary>
        public static string Tip_Trophys {
            get {
                return ResourceManager.GetString("Tip_Trophys", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Export VP_ScriptFull.vbs. Creates a script starter ready for Visual Pinball. Troughs, saucers, ball stacks are created if VP type is specified in the machines switches and coils..
        /// </summary>
        public static string Tip_VPExportScriptFull {
            get {
                return ResourceManager.GetString("Tip_VPExportScriptFull", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Export VP_Coil.txt - all the coils into Visual Pinball &apos;SolCallBacks&apos;. Creates Sub routines for the Solenoid callback and a list of SolCallBacks. Flippers should be skipped from this process..
        /// </summary>
        public static string Tip_VPExportSolenoid {
            get {
                return ResourceManager.GetString("Tip_VPExportSolenoid", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Export VP_Switch.txt -  all the switches into Visual Pinball switch hits. Creates Sub routines for switches and these can be copied into a tables script..
        /// </summary>
        public static string Tip_VPExportSwitch {
            get {
                return ResourceManager.GetString("Tip_VPExportSwitch", resourceCulture);
            }
        }
    }
}
