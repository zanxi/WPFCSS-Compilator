using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using helper;
using AggModbus;
namespace SplitAndMerge
{
    class ScDumper
    {

        static Dictionary<string, Dumper> dumps = new Dictionary<string, Dumper>();
        static Dictionary<string, MasterModbus> masters = new Dictionary<string, MasterModbus>();
        static public void AddDumper(string nameSubs)
        {
            if (!nameSubs.Contains("."))
            {
                if (!dumps.ContainsKey(nameSubs))
                {
                    Dumper dump = new Dumper(MainWindow.project,nameSubs, MainWindow.project.subs[nameSubs].main);
                    if (!dump.Status)
                    {
                        throw new ArgumentException("Subsystem " + nameSubs + " not supported!");
                    }
                    dump.Connect();
                    if (!dump.isConnected())
                    {
                        throw new ArgumentException("Subsystem " + nameSubs + " not connected!");
                    }
                    dumps.Add(nameSubs, dump);
                }
                else
                {
                    throw new ArgumentException("Subsystem " + nameSubs + " is used!");
                }
                return;
            }
            if (masters.ContainsKey(nameSubs))
            {
                throw new ArgumentException("Subsystem and modbus" + nameSubs + " is used!");
            }
            string sub = nameSubs.Substring(0, nameSubs.IndexOf("."));
            string modbus = nameSubs.Substring(nameSubs.IndexOf(".") + 1);
            if (!MainWindow.project.subs.ContainsKey(sub))
            {
                throw new ArgumentException("Subsystem " + sub + " is not loaded!");

            }
            Subsystem Sub = MainWindow.project.subs[sub];
            foreach(ModbusDevice md in Sub.modbuses)
            {
                if (md.isMaster()) continue;
                if (md.name.CompareTo(modbus) != 0) continue;
                MasterModbus master = new MasterModbus(Sub,md);
                master.Connect();
                if (!master.IsConnected())
                {
                    throw new ArgumentException("Subsystem " + sub + " modbus " + modbus + " not connected!");
                }
                masters.Add(nameSubs,master);
                return;
            }
            throw new ArgumentException("Subsystem and modbus " + nameSubs + " not founded!");

        }
        static public string GetValue(NameVar var)
        {
            if (var.modbus == null)
            {
                if (!dumps.ContainsKey(var.subsystem))
                {
                    throw new ArgumentException("Subsystem " + var.subsystem + " not supported!");
                }
                Dumper dump = dumps[var.subsystem];
                string str = dump.GetValueStr(var.name);
                if (str == null)
                {
                    throw new ArgumentException("Subsystem " + var.subsystem + " var " + var.name + "not found!");

                }
                if (str.Contains("true")) str = "1";
                if (str.Contains("false")) str = "0";
                return str;

            }
            if (!masters.ContainsKey(var.subsystem + "." + var.modbus))
            {
                throw new ArgumentException("Subsystem " + var.subsystem +" and modbus"+var.modbus+" not supported!");
            }
            MasterModbus master=masters[var.subsystem + "." + var.modbus];
            string result = master.GetValueString(var.name);
            if (result == null)
            {
                throw new ArgumentException("Subsystem " + var.subsystem + " modbus "+var.modbus+" var " + var.name + "not found!");

            }
            if (result.Contains("true")) result = "1";
            if (result.Contains("false")) result = "0";
            return result;
        }
        static public void SetValue(NameVar var, string value)
        {
            if (var.modbus == null)
            {
                if (!dumps.ContainsKey(var.subsystem))
                {
                    throw new ArgumentException("Subsystem " +var.subsystem + " not supported!");
                }
                Dumper dump = dumps[var.subsystem];
                string str = dump.GetValueStr(var.name);
                if (str == null)
                {
                    throw new ArgumentException("Subsystem " + var.subsystem + " var " + var.name + " not found!");

                }
                dump.SetValue(var.name, value);
                return;
            }
            if (!masters.ContainsKey(var.subsystem + "." + var.modbus))
            {
                throw new ArgumentException("Subsystem " + var.subsystem + " and modbus" + var.modbus + " not supported!");
            }
            MasterModbus master = masters[var.subsystem + "." + var.modbus];
            string result = master.GetValueString(var.name);
            if (result == null)
            {
                throw new ArgumentException("Subsystem " + var.subsystem + " modbus " + var.modbus + " var " + var.name + " not found!");
            }
            if (master.IsReadOnly(var.name))
            {
                throw new ArgumentException("Subsystem " + var.subsystem + " modbus " + var.modbus + " var " + var.name + " read only!");
            }
            master.SetValueString(var.name, value);
        }
        static public void Close(string nameSubs)
        {
            if (!nameSubs.Contains("."))
            {
                if (!dumps.ContainsKey(nameSubs))
                {
                    throw new ArgumentException("Subsystem " + nameSubs + " not supported!");
                }
                Dumper dump = dumps[nameSubs];
                dump.Close();
                dumps.Remove(nameSubs);
                return;
            }
            if (!masters.ContainsKey(nameSubs))
            {
                throw new ArgumentException("Subsystem and modbus " + nameSubs + " not supported!");
            }
            MasterModbus master = masters[nameSubs];
            master.Close();
            masters.Remove(nameSubs);

        }
    }
}