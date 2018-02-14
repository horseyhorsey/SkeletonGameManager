using System;
using System.Collections.Generic;
using static SkeletonGame.Models.SdlKeyCode;

namespace SkeletonGameManager.WPF.ViewModels.Config
{
    [PropertyChanged.AddINotifyPropertyChangedInterface]
    public class KeyboardMapItemViewModel
    {
        #region Constructors
        public KeyboardMapItemViewModel(KeyValuePair<string, string> keySwitch)
        {
            Key = keySwitch.Key;
            Number = keySwitch.Value;

            GetKeycode(Key);
        }

        public KeyboardMapItemViewModel()
        {
        } 
        #endregion

        #region Properties
        public string Key { get; set; }
        public string Number { get; set; }
        public SDL_Keycode Keycode { get; set; } 
        #endregion

        #region Private Methods
        /// <summary>
        /// Gets the keycode from the character/dcimal set inthe yaml.
        /// </summary>
        /// <param name="key">The key.</param>
        private void GetKeycode(string key)
        {
            if (key.Length == 1)
                this.Keycode = (SDL_Keycode)(Convert.ToChar(key));
            else
            {
                SDL_Keycode _keycode;
                Enum.TryParse(key, out _keycode);
                if (_keycode == SDL_Keycode.SDLK_UNKNOWN)
                    Enum.TryParse(key, out _keycode);

                this.Keycode = _keycode;
            }
        }
        #endregion
    }
}
