using SkeletonGame.Models.Machine;
using System;
using System.Collections.Generic;
using static SkeletonGame.Models.SdlKeyCode;

namespace SkeletonGameManager.Module.Config.ViewModels
{
    [PropertyChanged.AddINotifyPropertyChangedInterface]
    public class KeyboardMapItemViewModel
    {
        #region Constructors
        public KeyboardMapItemViewModel(PRSwitch prSwitch, KeyValuePair<string, string> keySwitch)
        {
            Name = prSwitch.Name;
            Number = prSwitch.Number;
            Key = keySwitch.Key;

            GetKeycode(Key.ToString());
        }

        public KeyboardMapItemViewModel()
        {
        }

        #endregion

        #region Properties
        public string Name { get; set; }
        public string Key { get; set; }
        public string Number { get; set; }

        private SDL_Keycode _keycode;
        public SDL_Keycode Keycode {
            get { return _keycode; }
            set
            {
                _keycode = value;

                var str = ((char)Keycode).ToString().Replace("\t", "\\t").Replace("\b", "\\b").Replace("\r", "\\r");

                Key = str;
            }
        } 
        #endregion

        #region Private Methods

        /// <summary>
        /// Gets the keycode from the character/decimal set in the yaml.
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
                {
                    var newKey = key.Replace("\\b", "\b")
                                    .Replace("\\r", "\r")
                                    .Replace("\\t", "\t");

                    _keycode = (SDL_Keycode)(Convert.ToChar(newKey));
                }
                    

                this.Keycode = _keycode;
            }
        }
        #endregion
    }
}
