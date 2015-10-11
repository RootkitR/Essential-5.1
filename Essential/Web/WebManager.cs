using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Essential.Net;
using System.Threading;
using System.Diagnostics;
using Essential.API;

namespace Essential.Web
{
    class WebManager
    {
        public WebManager()
        {

        }
        public void HandleRequest(string Request, SocketConnection socket)
        {
            //Yes, i know, that's shitty coded, but it works. At least for me.. Do it better & don't hate me :)
            bool f = false;
            if (Request.StartsWith("GET"))
            {

                string[] Splits = Request.Split(' ');
                if (Request.Split(' ')[1].Equals("/"))
                {
                    socket.SendFile("web/index.html");

                }

                else if (Request.Split(' ')[1].StartsWith("/emu/console"))
                {
                    List<string> alreadySent = new List<string>();
                    if (Splits[1].Split('?')[1].StartsWith("key") && Splits[1].Split('?')[1].Split('=')[1] == Essential.GetConfig().data["web.key"])
                    {
                        try
                        {
                            string[] asdf = Essential.GetConsoleWriter().GetLines();
                            foreach (string line in asdf.ToList<string>())
                            {
                                socket.SendMessage(line + "\n");
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.ToString());
                        }
                    }
                    else
                    {
                        this.Close(socket);
                    }
                }
                else if (Request.Split(' ')[1].StartsWith("/emu/exceptions"))
                {
                    if (Splits[1].Split('?')[1].StartsWith("key") && Splits[1].Split('?')[1].Split('=')[1] == Essential.GetConfig().data["web.key"])
                    {
                        socket.SendFile("exceptions.err");
                    }
                    else
                    {
                        this.Close(socket);
                    }
                }
                else if (Request.Split(' ')[1].StartsWith("/emu/restart"))
                {
                    if (Splits[1].Split('?')[1].StartsWith("key") && Splits[1].Split('?')[1].Split('=')[1] == Essential.GetConfig().data["web.key"])
                    {
                        socket.SendMessage("Bye!");
                        this.Close(socket);
                        Console.Clear();
                        Core.Logging.Disable();
                        Essential.Destroy("Disposing.......", false);
                        Process.Start(System.Reflection.Assembly.GetExecutingAssembly().Location);
                        Environment.Exit(0);
                    }
                    else
                    {
                        this.Close(socket);
                    }
                }
                else if (Request.Split(' ')[1].StartsWith("/emu/console/clear"))
                {
                    if (Splits[1].Split('?')[1].StartsWith("key") && Splits[1].Split('?')[1].Split('=')[1] == Essential.GetConfig().data["web.key"])
                    {
                        Console.Clear();
                        Essential.GetConsoleWriter().ClearIt();
                    }
                    else
                    {
                        this.Close(socket);
                    }
                }
                else if(Request.Split(' ')[1].StartsWith("/api/furni"))
                {
                    if(Splits[1].Split('?')[1].StartsWith("name"))
                    {
                        FurniImage.HandleRequest(Splits[1].Split('?')[1].Split('=')[1], socket);
                    }
                    else
                    {
                        this.Close(socket);
                    }
                }
                {
                    socket.SendFile("web" + Request.Split(' ')[1]);
                }

            }
            else if (Request.StartsWith("POST"))
            {
                if (Request.Split(' ')[1].Equals("/"))
                {
                    socket.SendFile("web/index.html");

                }
                else
                {
                    socket.SendFile("web" + Request.Split(' ')[1]);
                }
            }
            if (!f)
            {
                this.Close(socket);
            }
        }
        public void Close(SocketConnection socket)
        {
            socket.Dispose();
        }
    }
}
