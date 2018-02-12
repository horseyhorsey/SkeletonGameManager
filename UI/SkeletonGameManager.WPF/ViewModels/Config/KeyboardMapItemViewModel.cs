using SkeletonGame.Models;
using System;
using System.Collections.Generic;

namespace SkeletonGameManager.WPF.ViewModels.Config
{
    [PropertyChanged.AddINotifyPropertyChangedInterface]
    public class KeyboardMapItemViewModel
    {
        public KeyboardMapItemViewModel(KeyValuePair<string, string> keySwitch)
        {
            Key = keySwitch.Key;
            Number = keySwitch.Value;

            GetKeycode(Key);
        }

        private void GetKeycode(string key)
        {
            //string strKey = key.ToString();
            //if (char.IsDigit(key))
            //    strKey = strKey.Insert(0, "D");

            SDL_Scancode _keycode;            
            Enum.TryParse("SDL_SCANCODE_" + key.ToString().ToUpper(), out _keycode);

            if (_keycode == SDL_Scancode.SDL_SCANCODE_UNKNOWN)
                Enum.TryParse(key.ToString(), out _keycode);



            this.Keycode = _keycode;

            //Keycode = (SdlKeyCode.SDL_Scancode)Enum.Parse(typeof(SdlKeyCode.SDL_Scancode), key.ToString());
        }

        public KeyboardMapItemViewModel()
        {            
        }

        public string Key { get; set; }
        public string Number { get; set; }

        public SDL_Scancode Keycode { get; set; }
    }
}
