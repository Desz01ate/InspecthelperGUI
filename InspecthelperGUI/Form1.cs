
/*
 * Currently working on PACKING all file in one process (single execution)
 * v1.2 note, FUCK YOU GARENA!
 * v1.3 working as it should be to serve to lord of nvidiainspector
 * 
 * */
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Dynamic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InspecthelperGUI
{
    public partial class Form1 : Form
    {
        bool Flag = false;
        bool GoingOn = false;
        string ProcessFlag = "";
        Process p = new Process();
        string settingfile = $@"{Path.Combine(Path.GetTempPath(), Properties.Settings.Default.SettngsFilePath)}";
        string accessToken = "YOUR_APP_TOKEN";
        Facebook.FacebookClient facebookClient;
        struct INSPECTPROCESS
        {
            public string process_name { get; set; }
            public string real_process { get; set; }
        }
        public Form1()
        {
            InitializeComponent();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            Thread t = new Thread(startPy);
            Thread scanner = new Thread(() => ProcessScanner());
            if (Flag == false)
            {
                t.Start();
                GoingOn = true;
                scanner.Start();
                fuckTheseObject(label1, Color.Green, "inspecthelper is running");
                button1.Text = "STOP";
                textBox1.Enabled = false;
            }
            else
            {
                t.Abort();
                GoingOn = false;
                p.Kill();
                processExecuted("python");
                button3.Visible = false;
                button1.Text = "START";
                textBox1.Enabled = true;
            }
            Flag = !Flag;
        }
        private void startPy()
        {
            p.StartInfo.FileName = $@"{Properties.Settings.Default.pythonPath}";
            p.StartInfo.Arguments = $@"{Path.Combine(Path.GetTempPath(), Properties.Settings.Default.pythonFile)} {Properties.Settings.Default.PreferencesPath}";
            p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.CreateNoWindow = true;
            p.Start();
        }
        public bool ControlInvokeRequired(Control c, Action a)
        {
            if (c.InvokeRequired) c.Invoke(new MethodInvoker(delegate { a(); }));
            else return false;

            return true;
        }
        private void fuckTheseObject(Label f, Color c, string text)
        {
            f.ForeColor = c;
            if (ControlInvokeRequired(f, () => fuckTheseObject(f, c, text))) return;
            f.Text = text;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            File.SetAttributes(settingfile, File.GetAttributes(settingfile) & ~FileAttributes.ReadOnly);
            //Process.Start("notepad.exe", $@"{settingfile}");
            AppSettings frm = new AppSettings();
            frm.Show();
        }
        private List<INSPECTPROCESS> fileReader(string path)
        {
            try
            {
                var seperator = new List<INSPECTPROCESS>();
                var shouldRewrite = false;
                var line = "";
                //var tempwriter = new List<string> { $"### ONLY PROCESSES NAME ARE GIVEN BELOW THIS LINE IN FORM OF NAME,PROCESS NAME.EXE , ANY LINE WITH SHARP(#) WOULD BE IGNORED [Note that the order of the processes is affect the performance, please set the first to be what often use] ###" };
                var tempwriter = new List<string> { $"#Process Name,Application Name" };
                using (var reader = new StreamReader(path))
                {
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (line == "")
                        {
                            shouldRewrite = true;
                            continue;
                        }
                        if (line.IndexOf("#") == -1) //line contain no # inside
                        {
                            var inspect = new INSPECTPROCESS();
                            inspect.process_name = line.Split(',')[0];
                            inspect.real_process = line.Split(',')[1];
                            seperator.Add(inspect);
                            tempwriter.Add(string.Format("{0}", line));
                        }
                    }
                }
                if (shouldRewrite)
                {
                    using (var wr = new StreamWriter(settingfile))
                    {
                        tempwriter.ForEach(x => wr.WriteLine(x));
                    }
                }
                return seperator;
            }
            catch (FileNotFoundException ex)
            {
                MessageBox.Show("Settings file has not been generated yet, please restart the program.");
                Environment.Exit(0);
                return null;
            };
        }
        private void ProcessScanner()
        {
            INSPECTPROCESS latestProcess = default(INSPECTPROCESS);
            while (true)
            {
                if (GoingOn)
                {
                    var dataset = fileReader($@"{Path.Combine(Path.GetTempPath(), Properties.Settings.Default.SettngsFilePath)}"); //reloading the settings file
                    INSPECTPROCESS currentProcess = default(INSPECTPROCESS);
                    var shouldShowing = false;
                    foreach (var proc in Process.GetProcesses())
                    {
                        currentProcess = dataset.Find(process => process.real_process == $"{proc.ProcessName}.exe");
                        if (dataset.Contains(currentProcess))
                        {
                            shouldShowing = true;
                            if (!latestProcess.Equals(currentProcess))
                            {
                                DeletePreviousPost();
                                dynamic param = new ExpandoObject();
                                param.message = $"Currently playing {currentProcess.process_name} on {Environment.MachineName}, started from {DateTime.Now.ToString("hh:mm tt")}!";
                                facebookClient.Post("me/feed",param);
                                    //facebookService.PostOnWallAsync(accessToken, $"Currently playing {currentProcess.process_name} on {Environment.MachineName}, started from {DateTime.Now.ToString("hh:mm tt")}!");
                                latestProcess = currentProcess;
                            }
                            break;
                        }
                    }
                    fuckTheseObject(label1, Color.Green, $"Currently on {currentProcess.process_name ?? "Desktop"}");
                    ProcessFlag = currentProcess.real_process;
                    ControlInvokeRequired(button3, () => button3.Visible = shouldShowing);
                }
                else
                {
                    fuckTheseObject(label1, Color.Red, "inspecthelper is not running");
                }
                Thread.Sleep(1000);
            }
        }

        private void DeletePreviousPost()
        {
            dynamic param = new ExpandoObject();
            param.fields = "posts.since(" + DateTime.Now.ToString("ddMMMMyyyy") + "){id,message}";
            var result = facebookClient.Get("me", param);
            var data = Convert.ToString(result.posts.data);
            List<Post> jsonArray = JsonConvert.DeserializeObject<List<Post>>(data);
            jsonArray.ForEach(post => {
                if (post.message.Contains(Environment.MachineName))
                {
                    facebookClient.Delete(post.id);
                }
            });
        }

        private void button3_Click(object sender, EventArgs e)
        {
            processExecuted(ProcessFlag.Substring(0, ProcessFlag.Length - 4));
        }
        private bool processExecuted(string what_to_kill)
        {
            try
            {
                fileGenerate();
                var p = new Process();
                p.StartInfo.FileName = Path.Combine(Path.GetTempPath(), "kill.bat");
                p.StartInfo.Arguments = what_to_kill;
                p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.CreateNoWindow = true;
                p.Start();
                p.Close();
                Thread.Sleep(300);
                File.Delete(Path.Combine(Path.GetTempPath(), "kill.bat"));
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        private bool processExecuted(List<string> what_to_kill)
        {
            try
            {
                fileGenerate();
                what_to_kill.ForEach(x =>
                {
                    var p = new Process();
                    p.StartInfo.FileName = Path.Combine(Path.GetTempPath(), "kill.bat");
                    p.StartInfo.Arguments = x;
                    p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                    p.StartInfo.UseShellExecute = false;
                    p.StartInfo.CreateNoWindow = true;
                    p.Start();
                    Thread.Sleep(300);
                });
                File.Delete(Path.Combine(Path.GetTempPath(), "kill.bat"));
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        private void fileGenerate()
        {
            string killcommand = Properties.Resources.KillBatchCommand;
            using (StreamWriter killbat = new StreamWriter(Path.Combine(Path.GetTempPath(), "kill.bat")))
            {
                killbat.Write(killcommand);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            processExecuted(new List<string> { "Garena", "GarenaMessenger", "ggdllhost" });
            MessageBox.Show("Garena has fucked up, cheers!");
        }

        private void button5_Click(object sender, EventArgs e)
        {
            var Path_to_the_lord = "nvidiainspector.exe";
            var The_lord_helper = "nvidiaProfileInspector.exe";
            if (!File.Exists(Path_to_the_lord) || !File.Exists(The_lord_helper))
            {
                File.WriteAllBytes(Path_to_the_lord, Properties.Resources.nvidiaInspector);
                File.WriteAllBytes(The_lord_helper, Properties.Resources.nvidiaProfileInspector);
            }
            Process.Start(Path_to_the_lord);
        }
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            File.SetAttributes(settingfile, File.GetAttributes(settingfile) | FileAttributes.ReadOnly);
            if (GoingOn == true)
            {
                if (MessageBox.Show("InspecthelperGUI will be terminate if you close it now, continue?", Application.ProductName, MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    try
                    {
                        p.Kill();
                    }
                    catch (Exception)
                    {

                    }
                    finally
                    {
                        DeletePreviousPost();
                        Dispose(true);
                        Environment.Exit(0);
                    }
                }
                else
                {
                    e.Cancel = true;
                }
                //Environment.Exit(0);
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.PreferencesPath = textBox1.Text;
            Properties.Settings.Default.Save();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            label2.Text = "Status : ";
            fuckTheseObject(label1, Color.Red, "inspecthelper is not running");
            button1.Text = "START";
            Directory.CreateDirectory($@"{Path.Combine(Path.GetTempPath(), "inspecthelper")}");
            if (!File.Exists($@"{Properties.Settings.Default.pythonPath}"))
            {
                if (MessageBox.Show($@"Python 2.7 is required to run this application.{"\n"}Do you want to go to official Python website for installer?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Error) == DialogResult.Yes)
                {
                    Process.Start("https://www.python.org/downloads/");
                }
                Environment.Exit(0);
            }
            if (!File.Exists(settingfile))
            {

                var tempFile = File.Create(settingfile);
                using (StreamWriter sw = new StreamWriter(tempFile))
                {
                    //sw.WriteLine($@"### ONLY PROCESSES NAME ARE GIVEN BELOW THIS LINE IN FORM OF NAME,PROCESS NAME.EXE , ANY LINE WITH SHARP(#) WOULD BE IGNORED [Note that the order of the processes is affect the performance, please set the first to be what often use] ###");
                    //sw.WriteLine($@"#ExampleName,example.exe");
                    sw.WriteLine($"#Application Name,Process Name");
                }
            }
            File.SetAttributes(settingfile, File.GetAttributes(settingfile) | FileAttributes.ReadOnly);
            if (!File.Exists($@"{Path.Combine(Path.GetTempPath(), Properties.Settings.Default.pythonFile)}"))
            {
                var tempFile = File.Create(Path.Combine(Path.GetTempPath(), Properties.Settings.Default.pythonFile));
                using (StreamWriter sw = new StreamWriter(tempFile))
                {
                    sw.WriteLine(Properties.Resources.Pysrc);
                }
            }

            this.Text = "InspecthelperGUI";
            this.MaximizeBox = false;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            var processes = Process.GetProcessesByName("python");
            if (processes.Length > 0)
            {
                if (MessageBox.Show("Inspecthelper instance is already running, do you want to terminate it?", Application.ProductName, MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    Process.Start("kill.bat", "python");
                }
                else
                {
                    Environment.Exit(0);
                }
            }
            textBox1.Text = Properties.Settings.Default.PreferencesPath;
            facebookClient = new Facebook.FacebookClient(accessToken);
            button1.PerformClick();
        }
    }
}