using PropertyChanged;
using System.Collections.Generic;
using YamlDotNet.Serialization;

namespace SkeletonGame.Models {

    /// <summary>
    /// Games config to hold variables for assets, dmd options, modes etc.
    /// </summary>    
    [AddINotifyPropertyChangedInterface]
    public class GameConfig
    {
        [YamlMember(Alias = "pinproc_class", ApplyNamingConventions = false)]
        public string PinProcClass { get; set; } = @"procgame.fakepinproc.FakePinPROC";

        [YamlMember(Alias = "vp_game_map_file", ApplyNamingConventions = false)]
        public string VpGameMapFile { get; set; } = @"/P-ROC/shared/vp_game_map.yaml";

        [YamlMember(Alias = "PYSDL2_DLL_PATH", ApplyNamingConventions = false)]
        public string PySdlDllPath { get; set; } = @"C:\P-ROC\DLLs\";

        [YamlMember(Alias = "use_virtual_dmd_only", ApplyNamingConventions = false)]
        public bool UseVirtualDmdOnly { get; set; } = true;

        [YamlMember(Alias = "font_path", ApplyNamingConventions = false)]
        public List<string> FontPath { get; set; }

        [YamlMember(Alias = "config_path", ApplyNamingConventions = false)]
        public List<string> ConfigPath { get; set; }

        [YamlMember(Alias = "dmd_path", ApplyNamingConventions = false)]
        public string DmdPath { get; set; } = @"./assets/dmd/";

        [YamlMember(Alias = "sound_path", ApplyNamingConventions = false)]
        public string SoundPath { get; set; } = @"./assets/sound/";

        [YamlMember(Alias = "voice_dir", ApplyNamingConventions = false)]
        public string VoiceDir { get; set; } = @"voice/";

        [YamlMember(Alias = "sfx_dir", ApplyNamingConventions = false)]
        public string SfxDir { get; set; } = @"sfx/";

        [YamlMember(Alias = "music_dir", ApplyNamingConventions = false)]
        public string MusicDir { get; set; } = @"music/";

        [YamlMember(Alias = "hdfont_path", ApplyNamingConventions = false)]
        public string HdFontPath { get; set; } = @"./assets/fonts/";

        [YamlMember(Alias = "dmd_grid_path", ApplyNamingConventions = false)]
        public string DmdGridPath { get; set; } = @"./assets/dmd/";

        [YamlMember(Alias = "default_modes", ApplyNamingConventions = false)]
        public DefaultModes DefaultModes { get; set; }

        [YamlMember(Alias = "dmd_dots_w", ApplyNamingConventions = false)]
        public int DmdDotsWidth { get; set; } = 224;

        [YamlMember(Alias = "dmd_dots_h", ApplyNamingConventions = false)]
        public int DmdDotsHeight { get; set; } = 112;

        [YamlMember(Alias = "dmd_dot_filter", ApplyNamingConventions = false)]
        public bool DmdDotFilter { get; set; } = true;

        [YamlMember(Alias = "dmd_fullscreen", ApplyNamingConventions = false)]
        public bool DmdFullscreen { get; set; } = false;

        [YamlMember(Alias = "dmd_window_border", ApplyNamingConventions = false)]
        public string DmdWindowBorder { get; set; } = "true";

        [YamlMember(Alias = "desktop_dmd_scale", ApplyNamingConventions = false)]
        public byte DesktopDmdScale { get; set; } = 5;

        [YamlMember(Alias = "dmd_framerate", ApplyNamingConventions = false)]
        public double DmdFramerate { get; set; } = 30;

        [YamlMember(Alias = "dmd_flip", ApplyNamingConventions = false)]
        public string DmdFlip { get; set; } = "0";

        [YamlMember(Alias = "screen_position_x", ApplyNamingConventions = false)]
        public int ScreenPosX { get; set; } = 123;

        [YamlMember(Alias = "screen_position_y", ApplyNamingConventions = false)]
        public int ScreenPosY { get; set; } = 104;

        [YamlMember(Alias = "audio_buffer_size", ApplyNamingConventions = false)]
        public int AudioBufferSize { get; set; } = 512;

        [YamlMember(Alias = "keyboard_switch_map", ApplyNamingConventions = false)]
        public object KeyboardSwitchMap { get; set; }        
    }

    /// <summary>
    /// Global Skeleton game options for modes
    /// </summary>
    public class DefaultModes
    {
        [YamlMember(Alias = "score_display", ApplyNamingConventions = false)]
        public string ScoreDisplay { get; set; } = "HD";

        [YamlMember(Alias = "bonus_tally", ApplyNamingConventions = false)]
        public bool BonusTally { get; set; } = true;

        [YamlMember(Alias = "attract", ApplyNamingConventions = false)]
        public bool Attract { get; set; } = true;

        [YamlMember(Alias = "osc_input", ApplyNamingConventions = false)]
        public bool OscInput { get; set; } = true;

        [YamlMember(Alias = "service_mode", ApplyNamingConventions = false)]
        public bool ServiceMode { get; set; } = true;

        [YamlMember(Alias = "ball_search", ApplyNamingConventions = false)]
        public bool BallSearch { get; set; } = true;

        [YamlMember(Alias = "ball_search_delay", ApplyNamingConventions = false)]
        public int BallSearchDelay { get; set; } = 30;

        [YamlMember(Alias = "tilt_mode", ApplyNamingConventions = false)]
        public bool TiltMode { get; set; } = true;

        [YamlMember(Alias = "multiline_highscore_entry", ApplyNamingConventions = false)]
        public bool MultilineHiscoreEntry { get; set; } = false;

        [YamlMember(Alias = "player_profiles", ApplyNamingConventions = false)]
        public bool PlayerProfiles { get; set; } = false;

        [YamlMember(Alias = "player_trophys", ApplyNamingConventions = false)]
        public bool PlayerTrophys { get; set; } = false;

    }
}
