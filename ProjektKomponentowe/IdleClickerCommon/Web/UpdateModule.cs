﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Drawing;
using System.IO;
using System.Net;
using System.Diagnostics;

namespace IdleClickerCommon
{
    public delegate void OnStatusTextChangedDelegate(string newStatusText, bool isError);
   

    public static class UpdateModule
    {
        private static string statusText;


        public static event OnStatusTextChangedDelegate OnStatusTextChanged;
        
        public static string StatusText
        {
            get
            {
                return statusText;
            }
            private set
            {
                statusText = value;
                OnStatusTextChanged(value, isError);
            }
        }

        public static bool CheckIfGameInPath(string path)
        {
            return (File.Exists(path + @"\IdleClicker.exe"));
        }

        public static bool CheckIfGameRunning()
        {
            Process[] pname = Process.GetProcessesByName("IdleClicker");
            return (pname.Length > 0);
        }

        public delegate void OnProgressDelegate(double progress);

        private static int allFiles = 0;
        private static int filesCopied = 0;

        public static bool Install()
        {
            try
            {
                allFiles = CountFiles();
                CreateDirs();
                CopyFiles();
                return true;
            }
            catch (Exception e)
            {
                //MessageBox.Show(e.Message);
                 return false;
            }
        }

        public static event OnProgressDelegate Progress;
        public static event OnProgressDelegate FileProgress;

        private static void CreateDirs()
        {
            CreateDirs(Program.GamePath + @"\Update", Program.GamePath);
        }

        private static void CreateDirs(string updateDirs, string gameDirs)
        {
            DirectoryInfo updateDir = new DirectoryInfo(updateDirs);

            foreach (DirectoryInfo item in updateDir.GetDirectories())
            {
                string dir = gameDirs + @"\" + item;
                Directory.CreateDirectory(dir);
                CreateDirs(updateDirs + @"\" + item, dir);
            }
        }

        private static int CountFiles()
        {
            return CountFiles(Program.GamePath + @"\Update");
        }

        private static int CountFiles(string dir)
        {
            DirectoryInfo updateDir = new DirectoryInfo(dir);

            int allFiles = updateDir.GetFiles().Length;

            foreach (DirectoryInfo item in updateDir.GetDirectories())
            {
                allFiles += CountFiles(dir + @"\" + item);
            }
            return allFiles;
        }

       private static void CopyFiles()
       {
            CopyFiles(Program.GamePath + @"\Update", Program.GamePath);
        }

        private static void CopyFiles(string updateDirs, string gameDirs)
        {
            DirectoryInfo updateDir = new DirectoryInfo(updateDirs);

            foreach (FileInfo item in updateDir.GetFiles())
            {
                CustomFileCopier cfc = new CustomFileCopier(item.FullName, gameDirs + @"\" + item);
                cfc.OnProgressChanged += (c) =>
                {
                    if (FileProgress != null)
                        FileProgress(c);
                };
                cfc.OnCompleted += () =>
                {
                    if (FileProgress != null)
                       FileProgress(100.0);
                };
                cfc.Copy();

                filesCopied++;
                Progress((double)filesCopied / (double)allFiles * 100.0);
            }

            foreach (DirectoryInfo item in updateDir.GetDirectories())
            {
                string dir = gameDirs + @"\" + item;
                Directory.CreateDirectory(dir);
                CreateDirs(updateDirs + @"\" + item, dir);
            }
        }

        public static void DownloadInstallFile(string destination)
        {
            try
            {
                isError = false;
                StatusText = "Pobieranie pliku instalacyjnego...";

                FileToDownload ftd = new FileToDownload() { File = Program.WebSite + @"/Download/IdleClicker Install.exe" };

                DownloadFile(ftd, destination);

                isError = false;
                StatusText = "Pomyślnie zaktualizowano. Możesz teraz zainstalować...";
            }
            catch (Exception e)
            {
                isError = true;
                StatusText = e.Message;
            }
        }

        public static bool CheckIfInstalled()
        {
            string path = Program.GamePath;
            bool check = CheckIfGameInPath(path);
            if(!check)
            {
                path = Program.ApplicationExecutablePath;
                check = CheckIfGameInPath(path);
                if (check)
                    Program.GamePath = path;     
            }

            return check;
        }

        public static bool isError { get; private set; }

        public static void CheckIfUpToDate()
        {       
            List<ProgramVersion> versions = GetVersions();
            bool upToDate = CheckIfUpToDatePriv(versions);
        }

        public static void UpToDate()
        {
            try
            {
                List<ProgramVersion> versions = GetVersions();
                bool checkUpToDate = CheckIfUpToDatePriv(versions);

                if (checkUpToDate)
                {
                    List<FileToDownload> filesToDownload = GetFilesToDownload(versions);               
                    ClearUpdateFolder();
                    DownloadFile(filesToDownload, Program.ApplicationExecutablePath + @"/Update");
                    StatusText = "Kopiowanie plików...";
                    CopyFiles();

                    isError = false;
                    StatusText = "Pomyślnie zaktualizowano. Możesz teraz zainstalować...";

                    Program.UpdateToVersion = versions.First<ProgramVersion>();
                }
            }
            catch(Exception e)
            {
                isError = true;
                StatusText = e.Message;
            }
        }

        private static void ClearUpdateFolder()
        {
            try
            {
                isError = false;
                StatusText = "Usuwanie poprzednich aktualizacji";

                DirectoryInfo di = new DirectoryInfo(Program.ApplicationExecutablePath + @"\Update\");

                foreach (FileInfo file in di.GetFiles())
                {
                    file.Delete();
                }
                foreach (DirectoryInfo dir in di.GetDirectories())
                {
                    dir.Delete(true);
                }
            }
            catch//(Exception e)
            {
                throw new Exception("Błąd przy usuwaniu poprzenich aktualizacji");
            }
        }

        private static List<FileToDownload> GetFilesToDownload(List<ProgramVersion> versions)
        {
            isError = false;
            StatusText = "Sprawdzanie plików do pobrania";

            List<FileToDownload> filesToDownload = new List<FileToDownload>();

            try
            {
                for (int i = 0; versions[i].CompareTo(Program.Version) > 0; i++)
                {
                    WebClient webClient;
                    webClient = new WebClient();

                    string url = "http://www.IdleClicker.hexcore.pl/FilesInfo/VersionsFiles/" + versions[i] + ".php";
                    //string url = "http://localhost/IdleClicker/Files/FilesInfo/" + versions[i] + ".php";
                    string getStringTask = webClient.DownloadString(url);

                    string[] files = getStringTask.Split('|');
                    foreach (string item in files)
                    {
                        FileToDownload fileToDownload = new FileToDownload() { File = "http://www.IdleClicker.hexcore.pl/Files/" + versions[i] + "/" + item };
                        if (!filesToDownload.Contains(fileToDownload))
                        {
                            filesToDownload.Add(fileToDownload);
                        }
                    }
                }
                return filesToDownload;
            }
            catch// (Exception e)
            {
                throw new Exception("Błąd pobierania plików zmian");
            }
        }

        private static bool DownloadFile(FileToDownload file, string destinationfolder)
        {
            List<FileToDownload> files = new List<FileToDownload>();
            files.Add(file);
            return DownloadFile(files,destinationfolder);
        }

        private static bool DownloadFile(List<FileToDownload> files, string destinationfolder)
        {
            try
            {
                WebClient webClient;
                webClient = new WebClient();

                for (int i = 0; i < files.Count; i++)
                {
                    isError = false;
                    StatusText = "Pobieranie pliku " + (i+1) + " z " + (files.Count);
                    Uri url = new Uri(files[i].File);
                    string path = destinationfolder + @"\IdleClicker Installer.exe";
                    webClient.DownloadFile(url, path);
                }

                return true;
            }
            catch(Exception e)
            {
                throw new Exception("Błąd podczas pobierania aktualizacji");
            }
        }

        private static bool CheckIfUpToDatePriv(List<ProgramVersion> versions)
        {
            isError = false;
            StatusText = "Sprawdzanie wersji";

            ProgramVersion newestVersion = versions.First<ProgramVersion>();
            Program.NewestVersion = newestVersion;

            if (newestVersion.CompareTo(Program.Version) > 0)
            {
                string t1 = GetChangeLogs();

                isError = false;
                StatusText = "Dostępna nowa wersja!";
                return true;
            }
            else
            {
                isError = false;
                StatusText = "Program aktualny!";
                return false;
            }
        } 

        private static List<ProgramVersion> GetVersions()
        {
            try
            {
                WebClient webClient;
                webClient = new WebClient();

                string getStringTask = webClient.DownloadString("http://www.IdleClicker.hexcore.pl/FilesInfo/NewestVersion.php");
                //Task<string> getStringTask = webClient.GetStringAsync("http://localhost/IdleClicker/FilesInfo/NewestVersion.php");

                List<ProgramVersion> versions = new List<ProgramVersion>();
                string[] stringVersions = getStringTask.Split('|');
                foreach (string item in stringVersions)
                {
                    ProgramVersion newVersion = new ProgramVersion(0,0,0);
                    newVersion.FromString(item);
                    versions.Add(newVersion);
                }
                
                return versions;
            }
            catch// (Exception e)
            {
                throw new Exception("Błąd sprawdzania wersji");
            }
        }

        private static string GetChangeLogs()
        {
            try
            {
                WebClient webClient;
                webClient = new WebClient();
                string getStringTask = webClient.DownloadString("http://www.IdleClicker.hexcore.pl/FilesInfo/ChangeLogs.php");
                Program.ChangeLogs = getStringTask;
                //Task<string> getStringTask = webClient.GetStringAsync("http://localhost/IdleClicker/FilesInfo/ChangeLogs.php");

                return getStringTask;
            }
            catch// (Exception e)
            {
                throw new Exception("Błąd pobierania Changelogów");
            }
        }

        class FileToDownload
        {
            public string File { get; set; }
        }
    }
}
