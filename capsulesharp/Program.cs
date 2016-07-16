using System;
using capsulesharp.Gingers;
using System.Collections;

namespace capsulesharp
{
    class Program
    {
        static void Main(string[] args)
        {
            string mProvider;
            string mName;
            bool mPrivate;

            if (args.Length < 1)
            {
                Console.WriteLine("Usage:");
                Console.WriteLine("  gut shell\t- Add gut to path");
                Console.WriteLine("  gut init\t- Start the repo creation wizard");
                Console.WriteLine("  gut user\t- Start user conf creation wizard");
                return;
            }

            switch (args[0])
            {
                case "shell":
                    {
                        foreach (DictionaryEntry kvp in Environment.GetEnvironmentVariables())
                        {
                            if (kvp.Key.ToString().ToLower() == "path")
                            {
                                string lPath = AppDomain.CurrentDomain.BaseDirectory;
                                lPath = lPath.Substring(0, lPath.Length - 1);

                                if (kvp.Value.ToString().Contains(lPath))
                                {
                                    Console.WriteLine("Environment path already set");
                                    return;
                                }
                            }
                        }
                        System.Diagnostics.Process.Start("cmd.exe", "/c setx PATH \"%PATH%;" + AppDomain.CurrentDomain.BaseDirectory + "\"");
                        Console.WriteLine("Environment Path set");
                        break;
                    }
                case "init":
                    {
                        do
                        {
                            Console.Write("Select provider (1 for GitHub, 2 for BitBucket): ");
                            mProvider = Console.ReadLine();
                        } while (CheckProvider(mProvider));

                        do
                        {
                            Console.Write("Will it be a private repo (true/false): ");
                        } while (!bool.TryParse(Console.ReadLine(), out mPrivate));

                        Console.Write("Name of the new repo: ");
                        mName = Console.ReadLine();

                        if (mProvider == "1")
                        {
                            new GitHub(mName, mPrivate);
                            break;
                        }
                        if (mProvider == "2")
                        {
                            new BitBucket(mName, mPrivate);
                            break;
                        }
                        break;
                    }
                case "user":
                    {
                        Console.Write("Choose provider (1 for GitHub, 2 for BitBucket): ");
                        mProvider = Console.ReadLine();

                        Console.Write("Enter username: ");
                        string lUser = Console.ReadLine();

                        Console.Write("Enter password: ");
                        ConsoleKeyInfo key;
                        string lPass = "";

                        do
                        {
                            key = Console.ReadKey(true);

                            if (key.Key != ConsoleKey.Backspace && key.Key != ConsoleKey.Enter)
                            {
                                lPass += key.KeyChar;
                                Console.Write("*");
                            }
                            else
                            {
                                if (key.Key == ConsoleKey.Backspace && lPass.Length > 0)
                                {
                                    lPass = lPass.Substring(0, (lPass.Length - 1));
                                    Console.Write("\b \b");
                                }
                            }
                        } while (key.Key != ConsoleKey.Enter);

                        User.CreateUser(mProvider, lUser, lPass);

                        break;
                    }
                default:
                    {
                        break;
                    }
            }
        }

        private static bool CheckProvider(string pProvider)
        {
            if (pProvider == "1" || pProvider == "2")
            {
                return false;
            }

            return true;
        }
    }
}
