﻿using SkeletonGame.Models.Machine;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace SkeletonGame.Engine
{
    public interface IPinballFactoryBuilder
    {
        IPinballFactory Build(MachineType machineType);
    }

    public class PinballFactoryBuilder : IPinballFactoryBuilder
    {
        private readonly MachineConfig _config;

        public PinballFactoryBuilder(MachineConfig machineConfig)
        {
            _config = machineConfig;
        }

        public IPinballFactory Build(MachineType machineType)
        {
            if (machineType == MachineType.WPC || machineType == MachineType.WPC95 || machineType == MachineType.WPCALPHANUMERIC)
                return new WpcFactory(_config);
            else if (machineType == MachineType.STERNSAM)
            {
                return new SamFactory(_config);
            }
            else if (machineType == MachineType.PDB)
                return new PdbFactory(_config);
            else
                return null;
        }
    }

    public interface IPinballFactory
    {
        void GetCoils();
        void GetLamps();
        void GetSwitches();
        void GetLeds();
        Task PopConfig();
    }

    public abstract class PinballFactory : IPinballFactory
    {
        protected MachineConfig _machineConfig;

        public string DescriptionAttr<T>(T source)
        {
            FieldInfo fi = source.GetType().GetField(source.ToString());

            DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(
                typeof(DescriptionAttribute), false);

            if (attributes != null && attributes.Length > 0) return attributes[0].Description;
            else return source.ToString();
        }

        public virtual void GetCoils()
        {
            var numString = string.Empty;
            for (int i = 1; i < 33; i++)
            {                
                numString = (i < 10) ? $"C0{i}" : $"C{i}";

                var coil = _machineConfig.PRCoils.FirstOrDefault(x => x.Number.ToUpper() == numString);
                if (coil == null)
                    AddEmptyMachineItem(typeof(PRCoil), numString);
            }
        }

        public virtual void GetLamps()
        {
            if (_machineConfig.PRLamps == null)
                return;

            for (int i = 1; i < 81; i++)
            {
                if (i < 65)
                {
                    var numString = (i < 10) ? $"L0{i}" : $"L{i}";
                    var lamp = _machineConfig.PRLamps.FirstOrDefault(x => x.Number.ToUpper() == numString);
                    if (lamp == null)
                        AddEmptyMachineItem(typeof(PRLamp), numString);
                }
            }
        }

        public virtual void GetSwitches()
        {
            string numString = string.Empty;
            for (int i = 1; i < 81; i++)
            {
                numString = (i < 10) ? $"0{i}" : $"{i}";
                var _switch = _machineConfig.PRSwitches.FirstOrDefault(x => x.Number.ToUpper() == $"S{numString}");
                if (i < 65)
                {
                    if (_switch == null)
                        AddEmptyMachineItem(typeof(PRSwitch), numString);
                }
            }

            //if (i < 25)
            //    AddToDedicatedSwitchMatrix(i);
        }

        /// <summary>
        /// Gets the Leds from the machine config.
        /// </summary>
        /// <returns></returns>
        public virtual void GetLeds()
        {
            //TODO: LEds
            return;
        }

        /// <summary>
        /// Runs all methods in this class to build a machine.
        /// </summary>
        /// <returns></returns>
        public Task PopConfig()
        {
            return Task.Run(() =>
            {
                GetCoils();
                GetLamps();
                GetSwitches();
                GetLeds();
            });
        }

        public PinballFactory(MachineConfig machineConfig)
        {
            _machineConfig = machineConfig;
        }

        /// <summary>
        /// Adds an empty machine item with the name NOT USED
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="number">The number.</param>
        public void AddEmptyMachineItem(Type type, string number)
        {
            if (type == typeof(PRCoil))
                _machineConfig.PRCoils.Add(new PRCoil() { Number = number, Name = "NOT USED" });
            else if (type == typeof(PRSwitch))
                _machineConfig.PRSwitches.Add(new PRSwitch() { Number = number, Name = "NOT USED" });
            else if (type == typeof(PRLamp))
                _machineConfig.PRLamps.Add(new PRLamp() { Number = number, Name = "NOT USED" });
            else if (type == typeof(PRLed))
                _machineConfig.PRLeds.Add(new PRLed() { Number = number, Name = "NOT USED" });
        }
    }

    public class WpcFactory : PinballFactory
    {
        public WpcFactory(MachineConfig machineConfig) : base(machineConfig)
        {

        }        

        public override void GetLamps()
        {
            //Add switches, lamps and dedicated switches for wpc
            for (int i = 11; i < 99; i++)
            {
                var numStr = i.ToString();
                if (!numStr.Contains("9"))
                {
                    if (!numStr.Contains("0"))
                    {
                        var numString = (i < 10) ? $"L0{i}" : $"L{i}";
                        var lamp = _machineConfig.PRLamps.FirstOrDefault(x => x.Number.ToUpper() == numString);
                        if (lamp == null)
                            AddEmptyMachineItem(typeof(PRLamp), numString);

                    }
                }
                else if (i > 90)
                {
                    var numString = (i < 10) ? $"L0{i}" : $"L{i}";
                    var lamp = _machineConfig.PRLamps.FirstOrDefault(x => x.Number.ToUpper() == numString);
                    if (lamp == null)
                        AddEmptyMachineItem(typeof(PRLamp), numString);
                }
            }
        }

        public override void GetCoils()
        {            
            var numString = string.Empty;
            for (int i = 1; i < 29; i++)
            {                
                numString = (i < 10) ? $"C0{i}" : $"C{i}";
                var coil = _machineConfig.PRCoils.FirstOrDefault(x => x.Number.ToUpper() == numString);
                if (coil == null)
                    AddEmptyMachineItem(typeof(PRCoil), numString);
            }

            foreach (var flipCoil in Enum.GetNames(typeof(FlipperCoils)))
            {
                var t = Enum.Parse(typeof(FlipperCoils), flipCoil);
                var desc = DescriptionAttr((FlipperCoils)t);

                if (!_machineConfig.PRCoils.Any(x => x.Number == flipCoil))
                {
                    _machineConfig.PRCoils.Add(new PRCoil()
                    {
                        Name = desc,
                        Number = flipCoil
                    });
                }                
            }

            for (int i = 34; i < 74; i++)
            {
                numString = (i < 10) ? $"C0{i}" : $"C{i}";
                var coil = _machineConfig.PRCoils.FirstOrDefault(x => x.Number.ToUpper() == numString);
                if (coil == null)
                    AddEmptyMachineItem(typeof(PRCoil), numString);
            }
        }

        public override void GetSwitches()
        {
            var numStr = string.Empty;
            for (int i = 11; i < 99; i++)
            {
                numStr = i.ToString();
                if (!numStr.Contains("9"))
                {
                    if (!numStr.Contains("0"))
                    {
                        var _switch = _machineConfig.PRSwitches.FirstOrDefault(x => x.Number.ToUpper() == $"S{numStr}");
                        if (_switch == null)
                            AddEmptyMachineItem(typeof(PRSwitch),$"S{numStr}");

                    }
                }
                //else if (i > 90)
                //    AddToMatrix(numStr, i);
            }

            //Add dedicated for williams
            for (int i = 1; i < 10; i++)
            {
                numStr = $"SD{i}";
                var _switch = _machineConfig.PRSwitches.FirstOrDefault(x => x.Number.ToUpper() == $"{numStr}");
                if (_switch == null)
                    _machineConfig.PRSwitches.Add(new PRSwitch() { Number = $"{numStr}", Name = "NOT USED"});
            }        
        }
    }

    public class SamFactory : PinballFactory
    {
        public SamFactory(MachineConfig machineConfig) : base(machineConfig)
        {
        }

        public override void GetCoils()
        {
            for (int i = 1; i < 33; i++)
            {
                var numString = string.Empty;
                numString = (i < 10) ? $"C0{i}" : $"C{i}";

                var coil = _machineConfig.PRCoils.FirstOrDefault(x => x.Number.ToUpper() == numString);
                if (coil == null)
                    AddEmptyMachineItem(typeof(PRCoil), numString);
            }
        }

        public override void GetLamps()
        {
            for (int i = 1; i < 81; i++)
            {
                if (i < 65)
                {
                    var numString = (i < 10) ? $"L0{i}" : $"L{i}";
                    var lamp = _machineConfig.PRLamps.FirstOrDefault(x => x.Number.ToUpper() == numString);
                    if (lamp == null)
                        AddEmptyMachineItem(typeof(PRLamp), numString);
                }
            }
        }

        public override void GetSwitches()
        {
            string numString = string.Empty;
            for (int i = 1; i < 81; i++)
            {
                if (i < 65)
                {
                    numString = (i < 10) ? $"0{i}" : $"{i}";
                    var _switch = _machineConfig.PRSwitches.FirstOrDefault(x => x.Number.ToUpper() == $"S{numString}");
                    if (_switch == null)
                        AddEmptyMachineItem(typeof(PRSwitch), $"S{numString}");
                }

                //DEDICATED
                if (i < 25)
                {
                    var _switch = _machineConfig.PRSwitches.FirstOrDefault(x => x.Number.ToUpper() == $"SD{i}");
                    if (_switch == null)
                        AddEmptyMachineItem(typeof(PRSwitch), $"SD{i}");
                }
            }
        }
    }

    public class PdbFactory : PinballFactory
    {
        public PdbFactory(MachineConfig machineConfig) : base(machineConfig)
        {
        }

        public override void GetSwitches()
        {
            string numString = string.Empty;

            //Column / rows            
            for (int ii = 0; ii < 7; ii++)
            {
                for (int i = 0; i < 16; i++)
                {
                    numString = $"{ii}/{i}";

                    //Find switch
                    var _switch = _machineConfig.PRSwitches
                        .FirstOrDefault(x => x.Number.ToUpper() == $"{numString}");
                    if (_switch == null)
                        AddEmptyMachineItem(typeof(PRSwitch), numString);
                }
            }

            //Add dedicated for PDB. 32 switches.
            for (int i = 0; i < 32; i++)
            {
                numString = i < 10 ? $"SD0{i}" : $"SD{i}";
                var _switch = _machineConfig.PRSwitches.FirstOrDefault(x => x.Number.ToUpper() == $"{numString}");
                if (_switch == null)
                    _machineConfig.PRSwitches.Add(new PRSwitch() { Number = $"{numString}", Name = "NOT USED" });
            }
        }

        public override void GetLeds()
        {
            CreateLedBoard(2);
        }

        public override void GetCoils()
        {           
            CreateDriverCoils(0);
            CreateDriverCoils(1);
        }

        #region Private Methods
        /// <summary>
        /// Creates the driver coils from a given board address. 16 slots per board
        /// </summary>
        /// <param name="board">The board.</param>
        private void CreateDriverCoils(int board)
        {
            for (int i = 0; i < 2; i++)
            {
                for (int o = 0; o < 8; o++)
                {
                    var coilNum = $"A{board}-B{i}-{o}";

                    var coil = _machineConfig.PRCoils.FirstOrDefault(x => x.Number.ToUpper() == coilNum);
                    if (coil == null)
                        AddEmptyMachineItem(typeof(PRCoil), coilNum);
                }
            }
        }

        private void CreateLedBoard(byte boardAddress)
        {
            if (_machineConfig.PRLeds == null)
                _machineConfig.PRLeds = new List<PRLed>();

            for (int o = 0; o < 84; o += 3)
            {
                var ledNum = $"A{boardAddress}-R{o}-G{o + 1}-B{o + 2}";

                var led = _machineConfig.PRLeds
                    .FirstOrDefault(x => x.Number.ToUpper() == ledNum);
                if (led == null)
                    AddEmptyMachineItem(typeof(PRLed), ledNum);
            }
        }
        #endregion
    }
}
