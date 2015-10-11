using Essential.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Threading;
using System.Diagnostics;
using System.Xml;
using System.Drawing;
using System.Drawing.Imaging;

namespace Essential.API
{
    class FurniImage
    {
        public static void HandleRequest(string furniname, SocketConnection sConnection)
        {
            //This shit doesn't work.
            if (furniname.Contains("*"))
                furniname = furniname.Remove(furniname.IndexOf("*"));

            bool isHandled = false;
            if (!Directory.Exists("API\\" + furniname))
                Directory.CreateDirectory("API\\" + furniname);
            else if (File.Exists("API\\" + furniname + "\\" + furniname + ".png"))
            {
                sConnection.SendFile("API\\" + furniname + "\\" + furniname + ".png");
                return;
            }

            if (DownloadFurni(furniname))
            {
                try
                {
                    ProcessStartInfo startInfo2 = new ProcessStartInfo("swfdump.exe", "API\\" + furniname + "\\" + furniname + ".swf")
                    {
                        WindowStyle = ProcessWindowStyle.Hidden,
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        CreateNoWindow = true
                    };
                    bool isEnding = false;
                    Dictionary<string, string> fiiList = new Dictionary<string, string>();
                    Process process2 = Process.Start(startInfo2);
                    process2.OutputDataReceived += (sender, e) => ReadLine(e.Data, process2, fiiList, ref isEnding);
                    process2.BeginOutputReadLine();
                    process2.Start();
                    //Thread.Sleep(1000);
                    while (!isEnding)
                    {

                    }
                    process2.Close();
                    startInfo2 = new ProcessStartInfo("swfbinexport.exe", "API\\" + furniname + "\\" + furniname + ".swf")
                    {
                        WindowStyle = ProcessWindowStyle.Hidden,
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        CreateNoWindow = true
                    };
                    process2 = Process.Start(startInfo2);
                    process2.BeginOutputReadLine();
                    process2.Start();
                    process2.WaitForExit();
                    process2.Close();
                    List<FurniImageAsset> fiaList = new List<FurniImageAsset>();
                    foreach (KeyValuePair<string, string> kvp in fiiList.ToList<KeyValuePair<string, string>>())
                    {
                        string fileend = ".png";

                        string kchar = "p";
                        if (kvp.Value.Contains("_assets"))// || kvp.Value.Contains("_visualization"))
                        {
                            XmlTextReader reader = new XmlTextReader("API\\" + furniname + "\\" + furniname + "-" + int.Parse(kvp.Key) + ".bin");
                            while (reader.Read())
                            {
                                if (reader.Name == "asset" && reader.NodeType == XmlNodeType.Element)
                                {

                                    FurniImageAsset fia = new FurniImageAsset("", 0, 0, furniname);
                                    while (reader.MoveToNextAttribute()) // Read the attributes.
                                    {
                                        if (reader.Name == "name")
                                            fia.Name = reader.Value;
                                        else if (reader.Name == "x")
                                            fia.X = int.Parse(reader.Value);
                                        else if (reader.Name == "y")
                                            fia.Y = int.Parse(reader.Value);
                                    }
                                    if (fia.Name != "" && fia.Name.Contains(furniname + "_64")&& File.Exists("API\\" + furniname +"\\" + furniname + "_" + fia.Name + ".png"))
                                        fiaList.Add(fia);
                                }
                            }
                        }
                        else if (kvp.Value.Contains("_" + furniname + "_64"))
                        {
                            startInfo2 = new ProcessStartInfo("swfextract.exe", "-" + kchar + " " + kvp.Key + " -o " + "API\\" + furniname + "\\" + kvp.Value + fileend + " API\\" + furniname + "\\" + furniname + ".swf")
                            {
                                WindowStyle = ProcessWindowStyle.Hidden,
                                UseShellExecute = false,
                                RedirectStandardOutput = true,
                                CreateNoWindow = true
                            };
                            process2 = Process.Start(startInfo2);
                            process2.BeginOutputReadLine();
                            process2.Start();
                            process2.WaitForExit();
                            process2.Close();
                        }
                    }
                    string direction = "0";
                    FurniImageAsset first = fiaList.First();
                    direction = first.Name.Substring(first.Name.LastIndexOf("_") - 1, 1);
                    FurniImageAsset biggest = fiaList.OrderByDescending(o=>o.Height).First();
                    
                    Bitmap bmp = new Bitmap(biggest.Width+  200, biggest.Height +200);
                    using (Graphics g = Graphics.FromImage(bmp))
                    {

                        //g.DrawImage(biggest.Image, 0, 0);
                        foreach (FurniImageAsset furniImageAsset in fiaList)
                        {
                            try
                            {
                                /*if(furniImageAsset.X == 30 && furniImageAsset.Y == 80)
                                    g.DrawImage(furniImageAsset.Image, new Rectangle(0, 0, furniImageAsset.Width, furniImageAsset.Height));
                                else*/
                                furniImageAsset.X = furniImageAsset.X == 30 ? 0 : furniImageAsset.X;
                                furniImageAsset.Y = furniImageAsset.Y == 80 ? 0 : furniImageAsset.Y;
                                if(furniImageAsset.Name.Substring(furniImageAsset.Name.LastIndexOf("_") -1,1) == direction)
                                    g.DrawImage(furniImageAsset.Image, new Rectangle(biggest.Width / 2 - furniImageAsset.Width / 2, furniImageAsset.X - (furniImageAsset.Y / 2), furniImageAsset.Width, furniImageAsset.Height));
                            }
                            catch { }
                        }
                    }
                    bmp.Save("API\\"+furniname + "\\"+furniname +".png", ImageFormat.Png);
                    sConnection.SendFile("API\\" + furniname + "\\" + furniname + ".png");
                }catch(Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
            sConnection.SendFile("API//placeholder.png");
        }
        public static FurniRectangle GetFurniRectangle(List<FurniImageAsset> fiaList)
        {
            FurniRectangle rect = new FurniRectangle();
            Point p;
            bool setted = false;
            foreach (FurniImageAsset fia2 in fiaList.OrderByDescending(o=>o.Height))
            {
                if(fia2.X == 30 && fia2.Y == 80 && rect.Height <= fia2.Height && rect.Width <= fia2.Width)
                {
                    rect.Height = fia2.Height;
                    rect.Width = fia2.Width;
                }
            }
            return rect;
        }
        public static bool DownloadFurni(string furniname)
        {
            try
            {
                new WebClient().DownloadFile(Essential.SWFDirectory + furniname + ".swf", "API\\" + furniname + "\\" + furniname + ".swf");
                return true;
            }
            catch (Exception ex) { Console.WriteLine(ex.ToString()); }
            return false;
        }
        public static void ReadLine(string data, Process p, Dictionary<string,string> fiiList,ref bool isEnding)
        {
            //Console.WriteLine(data);
            if (data != null && data.Contains("exports ") && !fiiList.ContainsKey(data.Substring(data.IndexOf("exports "), 13).Substring(8, 4)))
                fiiList.Add(data.Substring(data.IndexOf("exports "), 13).Substring(8, 4), data.Substring(data.IndexOf("as \""), data.Length - data.IndexOf("as \"")).Replace("as \"", "").Replace("\"", ""));
            if (data != null && data.Contains("0 END"))
                isEnding = true;//Console.WriteLine("it's the end!");// p.Close();
        }
    }
}