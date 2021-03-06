﻿using System;
using System.Collections.Generic;
using PSoCProgrammerCOMLib;

namespace CypressSemiconductor.ChinaManufacturingTest
{
    class USB_I2C_Bridge
    {
        PSoCProgrammerCOM_Object pp;

        private string lastError = "";
        public string LastError
        {
            get { return lastError; }
        }

        private byte deviceAddress;
        public string DeviceAddress
        {
            get { return Convert.ToString(deviceAddress); }
        }

        public USB_I2C_Bridge()
        {
            pp = new PSoCProgrammerCOM_Object();
        }

        public string[] GetPorts()
        {
            object p;
            string[] ports;
            pp.GetPorts(out p, out lastError);
            ports = p as string[];
            pp.ClosePort(out lastError);
            return ports;
        }

        public int OpenPort(string progID)
        {
            int ok = 1;
            ok = pp.OpenPort(progID, out lastError);
            return ok;
        }

        public int ClosePort()
        {
            int ok = 1;
            ok = pp.ClosePort(out lastError);
            return ok;
        }

        public int SetPower(string power)
        {
            int ok = 1;
            ok = pp.SetPowerVoltage(power, out lastError);
            return ok;
        }

        public int PowerOn()
        {
            int ok = 1;
            ok = pp.PowerOn(out lastError);
            return ok;
        }

        public int PowerOff()
        {
            int ok = 1;
            ok = pp.PowerOff(out lastError);
            return ok;
        }

        public int SetI2CSpeed(byte speed)
        {
            int ok = 1;
            switch (speed)
            {
                case 0:
                    ok = pp.I2C_SetSpeed(enumI2Cspeed.CLK_400K, out lastError);
                    break;
                case 1:
                    ok = pp.I2C_SetSpeed(enumI2Cspeed.CLK_100K, out lastError);
                    break;
                case 2:
                    ok = pp.I2C_SetSpeed(enumI2Cspeed.CLK_50K, out lastError);
                    break;
            }
            ok = pp.I2C_SetTimeout(1000, out lastError);
            return ok;
        }

        public int GetDeviceAddress()
        {
            int ok = 1;
            object DeviceList;
            ok = pp.I2C_GetDeviceList(out DeviceList, out lastError);
            if (ok == 0)
            {
                byte[] devices = DeviceList as byte[];
                if (devices.Length <= 0)
                {
                    lastError = "No Device Found";
                    ok = 1;
                }
                else
                    deviceAddress = devices[0];
            }
            return ok;
        }

        public byte[] ReadWrite(byte command, byte nToRead)
        {
            int ok = 1;
            byte[] data;
            object dataIn;
            ok = pp.I2C_SendData(deviceAddress, new byte[] { 0x00, command }, out lastError);
            ok += pp.I2C_ReadData(deviceAddress, nToRead, out dataIn, out lastError);

            if (ok > 0)
            {
                throw new Exception("USB_I2C Error: " + lastError);
            }
            data = dataIn as byte[];
            return data;
        }

        public byte[] ReadWrite(byte register, byte command, byte nToRead)
        {
            int ok = 1;
            byte[] data;
            object dataIn;
            ok = pp.I2C_SendData(deviceAddress, new byte[] { register, command }, out lastError);
            ok += pp.I2C_ReadData(deviceAddress, nToRead, out dataIn, out lastError);

            if (ok > 0)
            {
                throw new Exception("USB_I2C Error: " + lastError);
            }
            data = dataIn as byte[];
            return data;
        }



        public byte[] ReadWriteOffset(byte offset, byte nToRead)
        {
            int ok = 1;
            byte[] data;
            object dataIn;
            ok = pp.I2C_SendData(deviceAddress, new byte[] { offset }, out lastError);
            ok += pp.I2C_ReadData(deviceAddress, nToRead, out dataIn, out lastError);

            if (ok > 0)
            {
                throw new Exception("USB_I2C Error: " + lastError);
            }

            data = dataIn as byte[];
            return data;
        }
    }
}
