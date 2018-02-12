using YamlDotNet.Serialization;

namespace SkeletonGame.Models.Score
{
    public class ScoreDisplay
    {
        [YamlMember(Alias = "ScoreLayout", ApplyNamingConventions = false)]
        public ScoreLayout ScoreLayout { get; set; }
    }

    public class ScoreLayout
    {
        [YamlMember(Alias = "SinglePlayer", ApplyNamingConventions = false)]
        public SinglePlayer SinglePlayer { get; set; }

        [YamlMember(Alias = "MultiPlayer", ApplyNamingConventions = false)]
        public MultiPlayer MultiPlayer { get; set; }
    }

    public class Player : ScoreOptionBase { }

    public class ActivePlayer : Player
    {
        [YamlMember(Alias = "in_place_if_active", ApplyNamingConventions = false)]
        public string InPlaceIfActive { get; set; }
    }

    public abstract class PlayerLayout
    {
        [YamlMember(Alias = "Foreground", ApplyNamingConventions = false)]
        public Foreground Foreground { get; set; } = null;

        [YamlMember(Alias = "Background", ApplyNamingConventions = false)]
        public Background Background { get; set; } = null;

        [YamlMember(Alias = "BallNumber", ApplyNamingConventions = false)]
        public BallNumber BallNumber { get; set; }

        [YamlMember(Alias = "CreditIndicator", ApplyNamingConventions = false)]
        public CreditIndicator CreditIndicator { get; set; }
    }
    
    public class SinglePlayer : PlayerLayout
    {
        [YamlMember(Alias = "Score", ApplyNamingConventions = false)]
        public Score Score { get; set; }
    }

    public class MultiPlayer : PlayerLayout
    {
        [YamlMember(Alias = "ActivePlayer", ApplyNamingConventions = false)]
        public ActivePlayer ActivePlayer { get; set; }

        [YamlMember(Alias = "PlayerOne", ApplyNamingConventions = false)]
        public Player PlayerOne { get; set; }

        [YamlMember(Alias = "PlayerTwo", ApplyNamingConventions = false)]
        public Player PlayerTwo { get; set; }

        [YamlMember(Alias = "PlayerThree", ApplyNamingConventions = false)]
        public Player PlayerThree { get; set; }

        [YamlMember(Alias = "PlayerFour", ApplyNamingConventions = false)]
        public Player PlayerFour { get; set; }        
    }

    public class Animation
    {
        [YamlMember(Alias = "Name", ApplyNamingConventions = false)]
        public string Name { get; set; }

        [YamlMember(Alias = "from_saved", ApplyNamingConventions = false)]
        public bool FromSaved { get; set; }
    }

    public class Background
    {
        [YamlMember(Alias = "Animation", ApplyNamingConventions = false)]
        public Animation Animation { get; set; }
    }

    public class Foreground
    {
        [YamlMember(Alias = "animation_layer", ApplyNamingConventions = false)]
        public AnimationLayer AnimationLayer { get; set; }
    }

    public class AnimationLayer
    {
        [YamlMember(Alias = "name", ApplyNamingConventions = false)]
        public string Name { get; set; }

        [YamlMember(Alias = "x", ApplyNamingConventions = false)]
        public double X { get; set; }

        [YamlMember(Alias = "y", ApplyNamingConventions = false)]
        public double Y { get; set; }
    }

    public class ScoreOptionBase
    {
        [YamlMember(Alias = "font", ApplyNamingConventions = false)]
        public string Font { get; set; }

        [YamlMember(Alias = "font_style", ApplyNamingConventions = false)]
        public string FontStyle { get; set; }

        [YamlMember(Alias = "h_justify", ApplyNamingConventions = false)]
        public string HJustify { get; set; }

        [YamlMember(Alias = "v_justify", ApplyNamingConventions = false)]
        public string VJustify { get; set; }

        [YamlMember(Alias = "x", ApplyNamingConventions = false)]
        public string X { get; set; }

        [YamlMember(Alias = "y", ApplyNamingConventions = false)]
        public string Y { get; set; }        
    }    

    public class Score : ScoreOptionBase
    {
        [YamlMember(Alias = "Background", ApplyNamingConventions = false)]
        /// <summary>
        /// Gets or sets the background animation for the font.
        /// </summary>
        public string Background { get; set; }

        [YamlMember(Alias = "visible", ApplyNamingConventions = false)]
        public string Visible { get; set; }
    }

    public class CreditIndicator : ScoreOptionBase
    {
        [YamlMember(Alias = "format", ApplyNamingConventions = false)]
        public string Format { get; set; }

        [YamlMember(Alias = "visible", ApplyNamingConventions = false)]
        public string Visible { get; set; }
    }

    public class BallNumber : ScoreOptionBase
    {
        [YamlMember(Alias = "visible", ApplyNamingConventions = false)]
        public string Visible { get; set; }

        [YamlMember(Alias = "format", ApplyNamingConventions = false)]
        public string Format { get; set; }
    }
}
