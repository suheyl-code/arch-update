using HtmlAgilityPack;
public static class Functions
{

    public static void MainLoop()
    {
        string result = string.Empty;
        string userNum = string.Empty;
        bool checkedState = true;
        do
        {
            Console.WriteLine("1. Grap Latest News Title & Date");
            Console.WriteLine("2. Update Base System");
            Console.WriteLine("3. Update AUR");
            Console.WriteLine("4. Exit");
            Console.Write("Select: ");
            userNum = Console.ReadLine();
            switch (userNum)
            {
                case "1":
                    Console.Clear();
                    Functions.HeaderFinder();
                    checkedState = true;
                    break;
                case "2":
                    Console.Clear();
                    result = Functions.UpdateSystem("base");
                    Console.WriteLine(result);
                    Functions.CheckRebootState(result);
                    checkedState = true;
                    break;
                case "3":
                    Console.Clear();
                    result = Functions.UpdateSystem("AUR");
                    Console.WriteLine(result);
                    Functions.CheckRebootState(result);
                    checkedState = true;
                    break;
                default:
                    checkedState = false;
                    Console.WriteLine("\tGoodBye...");
                    break;
            }
        } while (checkedState);
    }

    // Finding last header and date
    public static void HeaderFinder()
    {
        HtmlWeb web = new HtmlWeb();
        HtmlDocument doc = web.Load("https://archlinux.org/");

        var headerNames = doc.DocumentNode.SelectNodes("//h4");
        var headerTimes = doc.DocumentNode.SelectNodes("//p[@class='timestamp']");

        Console.Write($"{headerNames[0].InnerText}({headerTimes[0].InnerText})\n");
        Console.WriteLine();
    }

    // Updating Arch Linux base system
    public static string UpdateSystem(string baseSystem)
    {
        string command;
        if (baseSystem == "base")
        {
            command = "pacman -Syu";
        }
        else
        {
            command = "yay -Syu";
        }
        string result = string.Empty;
        using (System.Diagnostics.Process proc = new System.Diagnostics.Process())
        {
            proc.StartInfo.FileName = "sudo";
            proc.StartInfo.Arguments = $"{command}";
            proc.StartInfo.UseShellExecute = false;
            proc.StartInfo.RedirectStandardOutput = true;
            proc.StartInfo.RedirectStandardError = true;
            proc.Start();

            result += proc.StandardOutput.ReadToEnd();
            result += proc.StandardError.ReadToEnd();

            proc.WaitForExit();
        }
        return result;
    }

    public static void CheckRebootState(string result)
    {
        bool containsSearchResult = result.Contains("linux");
        if (containsSearchResult)
            Console.WriteLine("You need to Reboot your system!");
    }
}

