using CliWrap.Buffered;
using CliWrap;
using Colorify;
using System;
using System.IO;
using System.IO.Compression;
using System.Net;
using ToolBox.Platform;
using System.Threading;

namespace Ci4.Process
{
    class GitHub
    {
        public static void Download(string username, string repository, string project)
        {
            var versionDown = "v4.0.3";
            var pathDownload = Environment.CurrentDirectory + @"\" + project + @"\";
            var fileName = repository + "-"+ versionDown + ".zip";
            var localZipFile = pathDownload + fileName;
            var pathDir = pathDownload + repository + "-" + versionDown.Replace("v", "");

            var remoteUri = "https://codeload.github.com/" + username + "/" + repository + "/zip/" + versionDown;

            if (!Directory.Exists(pathDownload))
                Directory.CreateDirectory(pathDownload);
   
            if (File.Exists(localZipFile))
                File.Delete(localZipFile);

            WebClient wb = new WebClient();
            Program._colorify.WriteLine("Downloading File \"" + fileName + "\" From \"" + remoteUri + "\"", Colors.bgMuted);
            wb.DownloadFile(new Uri(remoteUri), localZipFile);
            Program._colorify.WriteLine("Successfully Downloaded File \"" + fileName + "\"", Colors.bgMuted);
            Program._colorify.WriteLine("Downloaded file saved in the following file system folder:", Colors.bgMuted);
            Program._colorify.WriteLine(pathDownload + fileName, Colors.bgMuted);
            Program._colorify.WriteLine("Unzipping File \"" + pathDownload + fileName + "\"", Colors.bgMuted);
            Unzip(pathDownload + fileName, pathDownload, true);
            Program._colorify.WriteLine("Delete File \"" + pathDownload + fileName + "\"" , Colors.bgMuted);
            File.Delete(pathDownload + fileName);
            if (Directory.Exists(pathDir))
            {
                foreach (string dirPath in Directory.GetDirectories(pathDir, "*", SearchOption.AllDirectories))
                    Directory.CreateDirectory(dirPath.Replace(pathDir, pathDownload));

                foreach (string newPath in Directory.GetFiles(pathDir, "*.*", SearchOption.AllDirectories))
                    File.Copy(newPath, newPath.Replace(pathDir, pathDownload), true);
            }
            else
            {
                Program._colorify.WriteLine("Source path does not exist", Colors.bgDanger);
            }
            Program._colorify.WriteLine("Delete Folder \"" + pathDir + "\"", Colors.bgMuted);
            Directory.Delete(pathDir, true);
            Program._colorify.WriteLine("Wait Configuring...", Colors.bgMuted);
            Cli.Wrap("php").WithArguments("-r \"file_exists('" + project + "/.env') || copy('" + project + "/env', '" + project + "/.env');\"").ExecuteBufferedAsync().Select(r => r.StandardOutput);
            Thread.Sleep(3000);
            var php = Cli.Wrap("php").WithArguments("-r \"echo file_exists('" + project + "/.env') ? '1' : '0';\"").ExecuteBufferedAsync().Select(r => r.StandardOutput);
            if (php.Task.Result.ToString() != "1")
            {
                Program._colorify.WriteLine("Please install php and execute the command \"php -r \"file_exists('.env') || copy('env', '.env');\"\" in the directory \"" + pathDownload + "\"", Colors.bgWarning);
            }
            Program._colorify.WriteLine("Project \"" + project + "\" successfully created!", Colors.bgSuccess);
        }

        private static void Unzip(string archive, string destination, bool overwrite)
        {
            if (!overwrite)
            {
                ZipFile.ExtractToDirectory(archive, destination);
                return;
            }

            using (FileStream zipToOpen = new FileStream(archive, FileMode.Open))
            {
                using (ZipArchive zipfile = new ZipArchive(zipToOpen, ZipArchiveMode.Update))
                {
                    DirectoryInfo di = Directory.CreateDirectory(destination);
                    string destinationDirectoryFullPath = di.FullName;

                    foreach (ZipArchiveEntry file in zipfile.Entries)
                    {
                        string completeFileName = Path.GetFullPath(Path.Combine(destinationDirectoryFullPath, file.FullName));

                        if (!completeFileName.StartsWith(destinationDirectoryFullPath, StringComparison.OrdinalIgnoreCase))
                        {
                            throw new IOException("Trying to extract file outside of destination directory.");
                        }

                        if (file.Name == "")
                        {
                            Directory.CreateDirectory(Path.GetDirectoryName(completeFileName));
                            continue;
                        }
                        file.ExtractToFile(completeFileName, true);
                    }
                }
            }
        }
    }
}
